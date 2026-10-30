using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.FSM;

namespace VMFramework.Procedure
{
    [ManagerCreationProvider(ManagerType.ProcedureCore)]
    public sealed class ProcedureManager : ManagerBehaviour<IProcedureManager>, IProcedureManager
    {
        private static readonly ProfilerMarker requestMarker = new("VMFramework.Procedure.Request.Run");
        private static readonly ProfilerMarker loadingMarker = new("VMFramework.Procedure.Loading.Start");
        private static readonly ProfilerMarker enterStateMarker = new("VMFramework.Procedure.State.Enter");
        private static readonly ProfilerMarker exitStateMarker = new("VMFramework.Procedure.State.Exit");
        private static readonly ProfilerMarker enterPublicationMarker = new("VMFramework.Procedure.Entry.Publish");
        private static readonly ProfilerMarker exitPublicationMarker = new("VMFramework.Procedure.Exit.Publish");
        [ShowInInspector] private IMultiFSM<string, ProcedureManager> fsm;
        [ShowInInspector] private List<IManagerBehaviour> managerBehaviours = new();
        private readonly Dictionary<string, IProcedure> procedures = new();
        private readonly Queue<ProcedureTransitionRequest> procedureSwitchQueue = new();
        private readonly CancellationTokenSource lifetime = new();
        private ProcedureTransitionRequest activeRequest;
        private TaskCompletionSource<bool> retirement;
        public CancellationToken LifetimeToken => lifetime.Token;
        public ProcedureResourceRetirement RetirementProduct { get; private set; }
        [ShowInInspector] public IReadOnlyList<IProcedure> Procedures => procedures.Values.ToList();
        [ShowInInspector] public IReadOnlyList<string> CurrentProcedureIDs => fsm?.CurrentStates.Keys.ToList();
        public event Action<string> OnEnterProcedureEvent;
        public event Action<string> OnExitProcedureEvent;

        protected override void Awake()
        {
            base.Awake();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeChanged;
            UnityEditor.AssemblyReloadEvents.beforeAssemblyReload += OnBeforeReload;
#endif
        }
        private void Start()
        {
            procedures.Clear();
            fsm = new MultiFSM<string, ProcedureManager>();
            string startProcedureID = null;
            int startProcedurePriority = 0;
            foreach (var procedureType in typeof(IProcedure).GetDerivedInstantiableClasses(false))
            {
                var procedure = (IProcedure)procedureType.CreateInstance();
                procedures.Add(procedure.id, procedure);
                fsm.AddState(procedure);
                if (procedureType.TryGetAttribute<StartProcedureAttribute>(false, out var startProcedureAttribute))
                {
                    if (startProcedureID == null || startProcedureAttribute.Priority > startProcedurePriority)
                    { startProcedureID = procedure.id; startProcedurePriority = startProcedureAttribute.Priority; }
                }
            }
            if (startProcedureID == null) throw new InvalidOperationException("No start procedure found.");
            fsm.Init(this);
            OnEnterProcedureEvent += procedureID => Debug.Log($"Enter Procedure:<color=orange>{procedureID}</color>");
            OnExitProcedureEvent += procedureID => Debug.Log($"Exit Procedure:<color=orange>{procedureID}</color>");
            ProcedureAutoSwitchBinder.Init(procedures.Values);
            OnEnterProcedureEvent += procedureID =>
            {
                if (ProcedureAutoSwitchBinder.TryGetNextProcedureID(procedureID, out var nextProcedureID))
                    ObserveAutomaticTransition(EnterProcedure(procedureID, nextProcedureID));
            };
            CollectGameInitializers();
            ObserveAutomaticTransition(EnterProcedure(startProcedureID));
        }
        private async void ObserveAutomaticTransition(Task actual)
        {
            try { await actual; }
            catch (OperationCanceledException) when (actual.IsCanceled) { }
            catch (Exception error) { Debug.LogException(error, this); }
        }
        private void Update()
        {
            if (procedureSwitchQueue.Count == 0 || IsLoading || activeRequest != null) return;
            RunProcedureTransition(procedureSwitchQueue.Dequeue());
        }
        private void AdmitRequest(string fromID, string toID)
        {
            if (retirement != null) throw new InvalidOperationException("The procedure owner is retiring.");
            lifetime.Token.ThrowIfCancellationRequested();
            if (IsLoading) throw new InvalidOperationException("Procedure loading owns the current transition.");
            if (fromID != null && !procedures.ContainsKey(fromID))
                throw new ArgumentException($"Procedure '{fromID}' does not exist.", nameof(fromID));
            if (toID != null && !procedures.ContainsKey(toID))
                throw new ArgumentException($"Procedure '{toID}' does not exist.", nameof(toID));
            if (fromID != null && !fsm.HasCurrentState(fromID))
                throw new InvalidOperationException($"Procedure '{fromID}' is not the actual current source.");
        }
        private Task QueueRequest(string fromID, string toID)
        {
            AdmitRequest(fromID, toID);
            var request = new ProcedureTransitionRequest(fromID, toID);
            procedureSwitchQueue.Enqueue(request);
            return request.Completion;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task EnterProcedure(string procedureID) => QueueRequest(null, procedureID);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task EnterProcedure(string fromProcedureID, string toProcedureID) => QueueRequest(fromProcedureID, toProcedureID);
        public Task EnterProcedureImmediately(string procedureID)
        {
            AdmitRequest(null, procedureID);
            return RunImmediate(new ProcedureTransitionRequest(null, procedureID));
        }
        public Task ExitProcedureImmediately(string procedureID)
        {
            AdmitRequest(procedureID, null);
            return RunImmediate(new ProcedureTransitionRequest(procedureID, null));
        }
        private Task RunImmediate(ProcedureTransitionRequest request)
        {
            if (activeRequest?.Completion is { IsCompleted: false } || procedureSwitchQueue.Count != 0)
                throw new InvalidOperationException("An accepted procedure request already owns this transition producer.");
            RunProcedureTransition(request);
            return request.Completion;
        }
        private void RunProcedureTransition(ProcedureTransitionRequest request)
        {
            using var sample = requestMarker.Auto();
            activeRequest = request;
            _ = request.RunAsync(token => ExecuteTransition(request, token), () => activeRequest = null, lifetime.Token);
        }
        private async UniTask ExecuteTransition(ProcedureTransitionRequest request, CancellationToken token)
        {
            var phase = ProcedureTransitionPhase.Admitted;
            try
            {
                if (request.FromID != null)
                {
                    token.ThrowIfCancellationRequested();
                    if (!fsm.HasCurrentState(request.FromID) || !fsm.CanExitState(request.FromID))
                        throw new InvalidOperationException($"Procedure '{request.FromID}' is not an exitable current source.");
                    phase = ProcedureTransitionPhase.ExitingSourceLoading;
                    await StartLoading(request.FromID, ProcedureLoadingType.OnExit, token);
                    token.ThrowIfCancellationRequested();
                    if (!fsm.CanExitState(request.FromID))
                        throw new InvalidOperationException($"Procedure '{request.FromID}' cannot exit after loading.");
                    phase = ProcedureTransitionPhase.ExitingSourceState;
                    using (exitStateMarker.Auto())
                    {
                        if (!fsm.ExitState(request.FromID))
                            throw new InvalidOperationException($"Procedure '{request.FromID}' did not exit its actual current state.");
                    }
                    phase = ProcedureTransitionPhase.PublishingSourceExit;
                    PublishProcedureExit(request.FromID);
                }
                if (request.ToID != null)
                {
                    token.ThrowIfCancellationRequested();
                    if (fsm.HasCurrentState(request.ToID) || !fsm.CanEnterState(request.ToID))
                        throw new InvalidOperationException($"Procedure '{request.ToID}' is not an enterable target.");
                    phase = ProcedureTransitionPhase.EnteringTargetLoading;
                    await StartLoading(request.ToID, ProcedureLoadingType.OnEnter, token);
                    token.ThrowIfCancellationRequested();
                    if (!fsm.CanEnterState(request.ToID))
                        throw new InvalidOperationException($"Procedure '{request.ToID}' cannot enter after loading.");
                    phase = ProcedureTransitionPhase.EnteringTargetState;
                    using (enterStateMarker.Auto())
                    {
                        if (!fsm.EnterState(request.ToID))
                            throw new InvalidOperationException($"Procedure '{request.ToID}' did not adopt its target state.");
                    }
                    phase = ProcedureTransitionPhase.PublishingTargetEntry;
                    using (enterPublicationMarker.Auto()) OnEnterProcedureEvent?.Invoke(request.ToID);
                }
            }
            catch (Exception primary)
            {
                var failures = new List<ProcedureTransitionRetirementFailure>(9);
                bool targetStarted = phase is ProcedureTransitionPhase.EnteringTargetLoading or
                    ProcedureTransitionPhase.EnteringTargetState or ProcedureTransitionPhase.PublishingTargetEntry;
                if (targetStarted)
                    await RetireProcedureAsync(request.ToID, true, true, failures);
                else if (phase is ProcedureTransitionPhase.ExitingSourceLoading or
                    ProcedureTransitionPhase.ExitingSourceState or ProcedureTransitionPhase.PublishingSourceExit)
                    await RetireProcedureAsync(request.FromID, false,
                        phase != ProcedureTransitionPhase.PublishingSourceExit, failures);
                if (request.FromID != null && !fsm.HasCurrentState(request.FromID) &&
                    phase != ProcedureTransitionPhase.Admitted && retirement == null && !token.IsCancellationRequested)
                    await RestoreSourceAsync(request.FromID, token, failures);
                if (primary is OperationCanceledException cancellation && failures.Count == 0 &&
                    token.IsCancellationRequested && cancellation.CancellationToken == token)
                    System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(primary).Throw();
                throw new ProcedureTransitionException(request.FromID, request.ToID, phase, primary,
                    failures.AsReadOnly(), fsm.CurrentStates.Keys.ToList().AsReadOnly());
            }
        }
        private void PublishProcedureExit(string procedureID)
        {
            using var sample = exitPublicationMarker.Auto();
            var callbacks = OnExitProcedureEvent;
            if (callbacks == null) return;
            var failures = new List<Exception>();
            foreach (Action<string> callback in callbacks.GetInvocationList())
            {
                try { callback(procedureID); }
                catch (Exception failure) { failures.Add(failure); }
            }
            if (failures.Count == 1)
                System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(failures[0]).Throw();
            if (failures.Count > 1)
                throw new AggregateException($"Procedure '{procedureID}' exit publication failed.", failures);
        }
        private async UniTask RetireProcedureAsync(string procedureID, bool retireLoading, bool publishExit,
            List<ProcedureTransitionRetirementFailure> failures)
        {
            if (retireLoading)
            {
                try { await StartLoading(procedureID, ProcedureLoadingType.OnExit, CancellationToken.None); }
                catch (Exception error)
                { failures.Add(new ProcedureTransitionRetirementFailure(procedureID, ProcedureTransitionPhase.RetiringLoading, error)); }
            }
            try
            {
                if (fsm.HasCurrentState(procedureID)) fsm.RetireState(procedureID);
            }
            catch (Exception error)
            { failures.Add(new ProcedureTransitionRetirementFailure(procedureID, ProcedureTransitionPhase.RetiringState, error)); }
            if (!publishExit) return;
            var callbacks = OnExitProcedureEvent;
            if (callbacks == null) return;
            foreach (Action<string> callback in callbacks.GetInvocationList())
            {
                try { callback(procedureID); }
                catch (Exception error)
                { failures.Add(new ProcedureTransitionRetirementFailure(procedureID, ProcedureTransitionPhase.PublishingRetirement, error)); }
            }
        }
        private async UniTask RestoreSourceAsync(string procedureID, CancellationToken token,
            List<ProcedureTransitionRetirementFailure> failures)
        {
            var phase = ProcedureTransitionPhase.RestoringSourceLoading;
            try
            {
                await StartLoading(procedureID, ProcedureLoadingType.OnEnter, token);
                token.ThrowIfCancellationRequested();
                phase = ProcedureTransitionPhase.RestoringSourceState;
                if (!fsm.EnterState(procedureID))
                    throw new InvalidOperationException($"Procedure '{procedureID}' cannot adopt its restored source.");
                phase = ProcedureTransitionPhase.PublishingSourceRestoration;
                OnEnterProcedureEvent?.Invoke(procedureID);
            }
            catch (Exception error)
            {
                failures.Add(new ProcedureTransitionRetirementFailure(procedureID, phase, error));
                await RetireProcedureAsync(procedureID, true, true, failures);
            }
        }
        public Task RetireAsync()
        {
            if (retirement != null) return retirement.Task;
            retirement = new TaskCompletionSource<bool>();
            CompleteRetirementAsync(retirement);
            return retirement.Task;
        }
        private async void CompleteRetirementAsync(TaskCompletionSource<bool> completion)
        {
            var token = lifetime.Token;
            var running = activeRequest?.Completion;
            bool released = true;
            var failures = new List<Exception>();
            try { lifetime.Cancel(); }
            catch (Exception error) { failures.Add(error); }
            while (procedureSwitchQueue.Count != 0)
            {
                var queued = procedureSwitchQueue.Dequeue();
                try { queued.CancelQueued(token); }
                catch (Exception error) { released = false; failures.Add(error); }
            }
            try { if (running != null) await running; }
            catch (OperationCanceledException error) when (token.IsCancellationRequested && error.CancellationToken == token) { }
            catch (Exception error)
            {
                failures.Add(error);
                if (error is ProcedureTransitionException transition && transition.RetirementFailures.Count != 0)
                    released = false;
            }
            try { lifetime.Dispose(); }
            catch (Exception error) { released = false; failures.Add(error); }
            var failure = failures.Count == 0 ? null : failures.Count == 1 ? failures[0] :
                new AggregateException("Procedure source and owned queue retirement failed.", failures);
            RetirementProduct = new ProcedureResourceRetirement(released, failure);
            if (failures.Count == 1) completion.SetException(failures[0]);
            else if (failures.Count > 1) completion.SetException(new AggregateException("Procedure source and owned queue retirement failed.", failures));
            else completion.SetResult(true);
        }
#if UNITY_EDITOR
        private void OnPlayModeChanged(UnityEditor.PlayModeStateChange state)
        {
            if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode)
                ObserveAutomaticTransition(RetireAsync());
        }
        private void OnBeforeReload() => ObserveAutomaticTransition(RetireAsync());
#endif
        protected override void OnDestroy()
        {
            try
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeChanged;
                UnityEditor.AssemblyReloadEvents.beforeAssemblyReload -= OnBeforeReload;
#endif
                ObserveAutomaticTransition(RetireAsync());
            }
            finally { base.OnDestroy(); }
        }

        [ShowInInspector]
        public bool IsLoading => initializerManager is { IsInitializing: true };

        public IReadOnlyInitializerManager Initialization => initializerManager;

        [ShowInInspector]
        private Dictionary<string, Dictionary<ProcedureLoadingType, List<IGameInitializer>>>
            gameInitializers = new();

        [ShowInInspector]
        private readonly InitializerManager initializerManager = new();

        public void CollectGameInitializers()
        {
            initializerManager.Set(Array.Empty<IInitializer>());
            gameInitializers.Clear();

            foreach (var derivedClass in typeof(IGameInitializer).GetDerivedInstantiableClasses(false))
            {
                if (derivedClass.TryGetAttribute<GameInitializerRegister>(false,
                        out var register) == false)
                {
                    continue;
                }

                if (register.ProcedureID.IsNullOrEmpty())
                {
                    Debug.LogError(
                        $"{derivedClass}'s {nameof(GameInitializerRegister)} Attribute " +
                        $"has an empty {nameof(GameInitializerRegister.ProcedureID)}");
                    continue;
                }

                if (HasProcedure(register.ProcedureID) == false)
                {
                    Debug.LogError(
                        $"{nameof(ProcedureManager)} does not have a {nameof(IProcedure)} " +
                        $"with ID {register.ProcedureID} " +
                        $"required by {derivedClass}'s {nameof(GameInitializerRegister)} Attribute");
                    continue;
                }

                if (derivedClass.CreateInstance() is not IGameInitializer initializer)
                {
                    Debug.LogError(
                        $"Failed to create instance of {derivedClass} as an {nameof(IGameInitializer)}");
                    continue;
                }

                var initializersByLoadingType = gameInitializers.GetOrCreate(register.ProcedureID);

                var initializers = initializersByLoadingType.GetOrCreate(register.LoadingType);

                initializers.Add(initializer);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IReadOnlyList<IGameInitializer> GetGameInitializers(string procedureID,
            ProcedureLoadingType loadingType)
        {
            if (gameInitializers.TryGetValue(procedureID, out var initializersByLoadingType) == false)
            {
                return Array.Empty<IGameInitializer>();
            }

            if (initializersByLoadingType.TryGetValue(loadingType, out var initializers) == false)
            {
                return Array.Empty<IGameInitializer>();
            }

            return initializers;
        }

        private async UniTask StartLoading(IReadOnlyList<IGameInitializer> initializers,
            CancellationToken cancellationToken)
        {
            if (IsLoading)
            {
                throw new InvalidOperationException("Loading already in progress.");
            }

            initializerManager.Set(initializers);
            await initializerManager.Initialize(cancellationToken);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private UniTask StartLoading(string procedureID, ProcedureLoadingType loadingType,
            CancellationToken cancellationToken)
        {
            using var sample = loadingMarker.Auto();
            var initializers = GetGameInitializers(procedureID, loadingType);
            return StartLoading(initializers, cancellationToken);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasProcedure(string procedureID)
        {
            return procedures.ContainsKey(procedureID);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasCurrentProcedure(string procedureID)
        {
            return fsm.HasCurrentState(procedureID);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IProcedure GetProcedure(string procedureID)
        {
            return procedures.GetValueOrDefault(procedureID);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IProcedure GetProcedureStrictly(string procedureID)
        {
            if (procedures.TryGetValue(procedureID, out var procedure) == false)
            {
                throw new ArgumentException($"Procedure with ID:{procedureID} does not exist.");
            }

            return procedure;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetProcedure(string procedureID, out IProcedure procedure)
        {
            return procedures.TryGetValue(procedureID, out procedure);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetProcedureWithWarning(string procedureID, out IProcedure procedure)
        {
            if (procedures.TryGetValue(procedureID, out procedure) == false)
            {
                UnityEngine.Debug.LogWarning($"Procedure with ID:{procedureID} does not exist.");
                return false;
            }
            
            return true;
        }
#if UNITY_EDITOR

        private static readonly TypeCollector<IProcedure> procedureCollector = new()
        {
            IncludingSelf = false,
            IncludingAbstract = false,
            IncludingInterface = false,
            IncludingGenericDefinition = false
        };

        public static IEnumerable<ValueDropdownItem> GetNameList()
        {
            if (procedureCollector.Count == 0)
            {
                procedureCollector.Collect();
            }

            foreach (var procedureType in procedureCollector.GetCollectedTypes())
            {
                var id = procedureType.GetStaticFieldValueByName<string>("ID");

                if (id.IsNullOrEmpty() == false)
                {
                    yield return new ValueDropdownItem(procedureType.Name, id);
                }
            }
        }
#endif
    }
}
