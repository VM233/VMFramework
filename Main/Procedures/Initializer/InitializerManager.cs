using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Unity.Profiling;
using VMFramework.Core;

namespace VMFramework.Procedure
{
    public class InitializerManager : IReadOnlyInitializerManager
    {
        public static readonly TimeSpan DefaultInitializationTimeout = TimeSpan.FromMinutes(2);

        private readonly List<IInitializer> initializers = new();

        private readonly List<InitializationActionExecution> currentOrderExecutions = new();

        #region Properties

        [ShowInInspector]
        public IReadOnlyList<IInitializer> Initializers => initializers;

        [ShowInInspector]
        public IReadOnlyList<InitializationActionExecution> CurrentOrderExecutions => currentOrderExecutions;

        [ShowInInspector]
        public int? CurrentOrder { get; private set; }

        [ShowInInspector]
        public TimeSpan InitializationTimeout { get; set; } = DefaultInitializationTimeout;

        [ShowInInspector]
        public Exception LastException { get; private set; }

        [ShowInInspector]
        public bool IsInitializing { get; private set; }

        [ShowInInspector]
        public bool IsInitialized { get; private set; }

        #endregion

        public void Set(IEnumerable<IInitializer> initializers)
        {
            if (IsInitializing)
            {
                throw new InvalidOperationException($"Cannot reset {nameof(InitializerManager)}, " +
                                                    $"while it is still initializing.");
            }

            if (initializers == null)
            {
                throw new ArgumentNullException(nameof(initializers));
            }

            this.initializers.Clear();
            foreach (var initializer in initializers)
            {
                this.initializers.Add(initializer ??
                                      throw new ArgumentException("An initializer cannot be null.",
                                          nameof(initializers)));
            }

            currentOrderExecutions.Clear();
            CurrentOrder = null;
            LastException = null;
            IsInitialized = false;
        }

        public async UniTask Initialize(CancellationToken cancellationToken = default)
        {
            if (IsInitializing)
            {
                throw new InvalidOperationException($"Cannot initialize {nameof(InitializerManager)}, " +
                                                    $"while it is still initializing.");
            }

            var initializationTimeout = InitializationTimeout;
            if (initializationTimeout <= TimeSpan.Zero)
            {
                throw new InvalidOperationException($"{nameof(InitializationTimeout)} must be greater than zero.");
            }

            IsInitialized = false;
            IsInitializing = true;
            LastException = null;
            CurrentOrder = null;
            currentOrderExecutions.Clear();

            using var timeoutCancellation = new CancellationTokenSource();
            using var timeoutRegistration = timeoutCancellation.CancelAfterSlim(initializationTimeout,
                DelayType.Realtime);
            using var initializationCancellation = CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken, timeoutCancellation.Token);

            try
            {
                var initializersName = initializers
                    .Where(initializer => initializer.EnableInitializationDebugLog)
                    .Select(initializer => initializer.GetType().ToString());
                var initializersNameWithTag = initializersName.Select(name => name.ColorTag("green")).ToList();

                if (initializersNameWithTag.Count > 0)
                {
                    var names = initializersNameWithTag.Join(", ");
                    UnityEngine.Debug.Log($"Initializer: {names} Started!");
                }

                foreach (var (order, listOfActions) in initializers.GetInitializationActions())
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    initializationCancellation.Token.ThrowIfCancellationRequested();

                    CurrentOrder = order;
                    currentOrderExecutions.Clear();

                    foreach (var actionInfo in listOfActions)
                    {
                        currentOrderExecutions.Add(new InitializationActionExecution(actionInfo));
                    }

                    using var orderCancellation = CancellationTokenSource.CreateLinkedTokenSource(
                        initializationCancellation.Token);
                    Exception firstActionException = null;
                    var cancellationFailures = new List<Exception>();

                    void OnActionFailed(Exception exception)
                    {
                        if (Interlocked.CompareExchange(ref firstActionException, exception, null) == null)
                        {
                            try { orderCancellation.Cancel(); }
                            catch (Exception failure)
                            { lock (cancellationFailures) cancellationFailures.Add(failure); }
                        }
                    }

                    var actionTasks = new List<UniTask>(currentOrderExecutions.Count);
                    foreach (var execution in currentOrderExecutions)
                    {
                        LogActionStart(execution.InitializationAction);
                        actionTasks.Add(ExecuteAction(execution, orderCancellation.Token,
                            initializationCancellation.Token, OnActionFailed));
                    }

                    await UniTask.WhenAll(actionTasks);

                    if (firstActionException != null)
                    {
                        if (cancellationFailures.Count > 0)
                            throw new AggregateException("Initializer action and sibling cancellation callbacks failed.",
                                new[] { firstActionException }.Concat(cancellationFailures));
                        ExceptionDispatchInfo.Capture(firstActionException).Throw();
                    }

                    cancellationToken.ThrowIfCancellationRequested();
                    initializationCancellation.Token.ThrowIfCancellationRequested();
                }

                IsInitialized = true;
            }
            catch (OperationCanceledException exception) when (exception.CancellationToken == initializationCancellation.Token &&
                                                               timeoutCancellation.IsCancellationRequested &&
                                                               cancellationToken.IsCancellationRequested == false)
            {
                var timeoutException = new TimeoutException(
                    BuildTimeoutMessage(initializationTimeout), exception);
                LastException = timeoutException;
                throw timeoutException;
            }
            catch (Exception exception)
            {
                LastException = exception;
                throw;
            }
            finally
            {
                IsInitializing = false;
            }
        }

        private static async UniTask ExecuteAction(InitializationActionExecution execution,
            CancellationToken actionCancellationToken, CancellationToken forcedCancellationToken,
            Action<Exception> onFailed)
        {
            execution.MarkRunning();
            Exception failure;
            try
            {
                actionCancellationToken.ThrowIfCancellationRequested();
                var action = execution.InitializationAction;
                UniTask pendingAction;
                using (new ProfilerMarker($"Initialize: {action.initializer.GetType().FullName}.{action.action.Method.Name}").Auto())
                    pendingAction = action.action(actionCancellationToken);
                await pendingAction;
                actionCancellationToken.ThrowIfCancellationRequested();
                forcedCancellationToken.ThrowIfCancellationRequested();
                execution.MarkSucceeded();
                return;
            }
            catch (Exception error) { failure = error; }

            bool OwnsCancellation(Exception error) => error is OperationCanceledException cancellation &&
                cancellation.CancellationToken.IsCancellationRequested &&
                (cancellation.CancellationToken == actionCancellationToken ||
                 cancellation.CancellationToken == forcedCancellationToken);
            if (OwnsCancellation(failure))
            {
                execution.MarkCanceled((OperationCanceledException)failure);
                return;
            }

            try { forcedCancellationToken.ThrowIfCancellationRequested(); }
            catch (OperationCanceledException cancellation)
            {
                failure = new AggregateException("Initializer source and its forced cancellation failed.",
                    failure, cancellation);
            }
            if (failure is OperationCanceledException externalCancellation)
                execution.MarkCanceled(externalCancellation);
            else execution.MarkFailed(failure);
            onFailed(failure);
        }

        private static void LogActionStart(InitializationAction actionInfo)
        {
            if (actionInfo.initializer.EnableInitializationDebugLog == false)
            {
                return;
            }

            var initializerName = actionInfo.initializer.GetType().ToString();
            if (actionInfo.initializer is IIDOwner<string> idOwner)
            {
                initializerName += $": {idOwner.id}";
            }

            UnityEngine.Debug.Log($"Initializing {actionInfo.action.Method.Name} of {initializerName}");
        }

        private string BuildTimeoutMessage(TimeSpan initializationTimeout)
        {
            var incompleteActions = currentOrderExecutions
                .Where(execution => execution.Status != InitializationActionStatus.Succeeded)
                .Select(execution =>
                {
                    var actionInfo = execution.InitializationAction;
                    return $"{actionInfo.initializer.GetType()}.{actionInfo.action.Method.Name} " +
                           $"({execution.Status})";
                })
                .ToList();

            var actionsDescription = incompleteActions.Count == 0 ? "none" : incompleteActions.Join(", ");
            return $"Initialization timed out after {initializationTimeout}. " +
                   $"Current order: {CurrentOrder?.ToString() ?? "none"}. " +
                   $"Incomplete actions: {actionsDescription}.";
        }
    }
}
