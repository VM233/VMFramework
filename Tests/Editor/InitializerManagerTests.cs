using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine.TestTools;
using VMFramework.Procedure;

namespace VMFramework.Tests
{
    public sealed class InitializerManagerTests
    {
        private sealed class TestInitializer : IInitializer
        {
            private readonly (int order, InitActionHandler action)[] actionDefinitions;

            public bool EnableInitializationDebugLog => false;

            public TestInitializer(params (int order, InitActionHandler action)[] actionDefinitions)
            {
                this.actionDefinitions = actionDefinitions;
            }

            public void GetInitializationActions(ICollection<InitializationAction> actions)
            {
                foreach (var (order, action) in actionDefinitions)
                {
                    actions.Add(new InitializationAction(order, action, this));
                }
            }
        }

        [UnityTest]
        public IEnumerator SameOrderRunsConcurrentlyAndOrdersRunSequentially()
        {
            return UniTask.ToCoroutine(async () =>
            {
                var firstStarted = false;
                var secondStarted = false;
                var firstCompleted = false;
                var secondCompleted = false;
                var laterRan = false;

                InitActionHandler first = async cancellationToken =>
                {
                    firstStarted = true;
                    await UniTask.WaitUntil(() => secondStarted, cancellationToken: cancellationToken);
                    firstCompleted = true;
                };
                InitActionHandler second = async cancellationToken =>
                {
                    secondStarted = true;
                    await UniTask.Yield(cancellationToken);
                    secondCompleted = true;
                };
                InitActionHandler later = cancellationToken =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Assert.That(firstStarted && secondStarted && firstCompleted && secondCompleted, Is.True);
                    laterRan = true;
                    return UniTask.CompletedTask;
                };

                var manager = CreateManager(
                    (0, first),
                    (0, second),
                    (1, later));

                await manager.Initialize();

                Assert.That(laterRan, Is.True);
                Assert.That(manager.IsInitialized, Is.True);
                Assert.That(manager.IsInitializing, Is.False);
                Assert.That(manager.CurrentOrder, Is.EqualTo(1));
                Assert.That(manager.CurrentOrderExecutions, Has.Count.EqualTo(1));
                Assert.That(manager.CurrentOrderExecutions[0].Status,
                    Is.EqualTo(InitializationActionStatus.Succeeded));
            });
        }

        [UnityTest]
        public IEnumerator DuplicateDelegateRegistrationsBothRun()
        {
            return UniTask.ToCoroutine(async () =>
            {
                var invocationCount = 0;
                InitActionHandler action = cancellationToken =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    invocationCount++;
                    return UniTask.CompletedTask;
                };

                var manager = CreateManager((0, action), (0, action));
                await manager.Initialize();

                Assert.That(invocationCount, Is.EqualTo(2));
                Assert.That(manager.CurrentOrderExecutions, Has.Count.EqualTo(2));
                Assert.That(manager.CurrentOrderExecutions,
                    Has.All.Property(nameof(InitializationActionExecution.Status))
                        .EqualTo(InitializationActionStatus.Succeeded));
            });
        }

        [UnityTest]
        public IEnumerator FailurePropagatesCancelsSiblingsAndStopsLaterOrders()
        {
            return UniTask.ToCoroutine(async () =>
            {
                var expectedException = new InvalidOperationException("Expected initialization failure.");
                var siblingObservedCancellation = false;
                var laterRan = false;

                InitActionHandler failing = async _ =>
                {
                    await UniTask.Yield();
                    throw expectedException;
                };
                InitActionHandler sibling = async cancellationToken =>
                {
                    try
                    {
                        await UniTask.Delay(TimeSpan.FromMinutes(1), DelayType.Realtime,
                            cancellationToken: cancellationToken);
                    }
                    finally
                    {
                        siblingObservedCancellation = cancellationToken.IsCancellationRequested;
                    }
                };
                InitActionHandler later = _ =>
                {
                    laterRan = true;
                    return UniTask.CompletedTask;
                };

                var manager = CreateManager(
                    (0, failing),
                    (0, sibling),
                    (1, later));

                Exception caughtException = null;
                try
                {
                    await manager.Initialize();
                }
                catch (Exception exception)
                {
                    caughtException = exception;
                }

                Assert.That(caughtException, Is.SameAs(expectedException));
                Assert.That(manager.LastException, Is.SameAs(expectedException));
                Assert.That(manager.IsInitialized, Is.False);
                Assert.That(manager.IsInitializing, Is.False);
                Assert.That(siblingObservedCancellation, Is.True);
                Assert.That(laterRan, Is.False);
                Assert.That(manager.CurrentOrderExecutions[0].Status,
                    Is.EqualTo(InitializationActionStatus.Failed));
                Assert.That(manager.CurrentOrderExecutions[1].Status,
                    Is.EqualTo(InitializationActionStatus.Canceled));
            });
        }

        [UnityTest]
        public IEnumerator CallerCancellationPropagatesAndResetsManagerState()
        {
            return UniTask.ToCoroutine(async () =>
            {
                using var cancellation = new CancellationTokenSource();
                using var cancellationTimer = cancellation.CancelAfterSlim(TimeSpan.FromMilliseconds(50),
                    DelayType.Realtime);

                InitActionHandler action = async cancellationToken =>
                {
                    await UniTask.Delay(TimeSpan.FromMinutes(1), DelayType.Realtime,
                        cancellationToken: cancellationToken);
                };

                var manager = CreateManager((0, action));
                OperationCanceledException caughtException = null;
                try
                {
                    await manager.Initialize(cancellation.Token);
                }
                catch (OperationCanceledException exception)
                {
                    caughtException = exception;
                }

                Assert.That(caughtException, Is.Not.Null);
                Assert.That(manager.LastException, Is.SameAs(caughtException));
                Assert.That(manager.IsInitialized, Is.False);
                Assert.That(manager.IsInitializing, Is.False);
                Assert.That(manager.CurrentOrderExecutions[0].Status,
                    Is.EqualTo(InitializationActionStatus.Canceled));
            });
        }

        [UnityTest]
        public IEnumerator TimeoutStopsWaitingForANonCooperativeAction()
        {
            return UniTask.ToCoroutine(async () =>
            {
                var neverCompletes = new UniTaskCompletionSource();
                InitActionHandler action = _ => neverCompletes.Task;

                var manager = CreateManager((0, action));
                manager.InitializationTimeout = TimeSpan.FromMilliseconds(50);

                TimeoutException caughtException = null;
                try
                {
                    await manager.Initialize();
                }
                catch (TimeoutException exception)
                {
                    caughtException = exception;
                }
                finally
                {
                    neverCompletes.TrySetResult();
                }

                Assert.That(caughtException, Is.Not.Null);
                Assert.That(caughtException.Message, Does.Contain("Current order: 0"));
                Assert.That(manager.LastException, Is.SameAs(caughtException));
                Assert.That(manager.IsInitialized, Is.False);
                Assert.That(manager.IsInitializing, Is.False);
                Assert.That(manager.CurrentOrderExecutions[0].Status,
                    Is.EqualTo(InitializationActionStatus.Canceled));
            });
        }

        private static InitializerManager CreateManager(
            params (int order, InitActionHandler action)[] actionDefinitions)
        {
            var manager = new InitializerManager
            {
                InitializationTimeout = TimeSpan.FromSeconds(2),
            };
            manager.Set(new[] { new TestInitializer(actionDefinitions) });
            return manager;
        }
    }
}
