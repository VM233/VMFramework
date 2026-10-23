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
        [UnitySetUp]
        public IEnumerator AdmitActualFrameCredit() => UniTask.ToCoroutine(async () =>
            await new InitializationWorkSlice().YieldIfBudgetExpired(CancellationToken.None));

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
        public IEnumerator TimeoutJoinsTheActualNonCooperativeSourceBeforeTerminal()
        {
            return UniTask.ToCoroutine(async () =>
            {
                var release = new UniTaskCompletionSource();
                var cancellationObserved = new UniTaskCompletionSource();
                InitActionHandler action = async token =>
                {
                    using var registration = token.Register(() => cancellationObserved.TrySetResult());
                    await release.Task;
                };
                var manager = CreateManager((0, action));
                manager.InitializationTimeout = TimeSpan.FromMilliseconds(50);
                var actual = manager.Initialize().AsTask();
                try
                {
                    await cancellationObserved.Task;
                    Assert.That(actual.IsCompleted, Is.False, "Timeout does not retire the still-running source.");
                    Assert.That(manager.IsInitializing, Is.True);
                }
                finally { release.TrySetResult(); }
                TimeoutException failure = null;
                try { await actual; } catch (TimeoutException error) { failure = error; }
                Assert.That(failure, Is.Not.Null);
                Assert.That(manager.LastException, Is.SameAs(failure));
                Assert.That(manager.IsInitializing, Is.False);
                Assert.That(manager.CurrentOrderExecutions[0].Status, Is.EqualTo(InitializationActionStatus.Canceled));

                var control = CreateManager((0, _ => UniTask.CompletedTask));
                await control.Initialize();
                Assert.That(control.IsInitialized, Is.True);
                Assert.That(control.IsInitializing, Is.False);
            });
        }

        [UnityTest]
        public IEnumerator SiblingCancellationCallbackFailureDoesNotDetachItsActualSource()
        {
            return UniTask.ToCoroutine(async () =>
            {
                var primary = new InvalidOperationException("Actual failing initializer.");
                var callbackFailure = new InvalidOperationException("Actual cancellation callback.");
                var release = new UniTaskCompletionSource();
                var siblingReady = new UniTaskCompletionSource();
                var cancellationObserved = new UniTaskCompletionSource();
                InitActionHandler failing = async _ => { await siblingReady.Task; throw primary; };
                InitActionHandler sibling = async token =>
                {
                    using var registration = token.Register(() =>
                    {
                        cancellationObserved.TrySetResult();
                        throw callbackFailure;
                    });
                    siblingReady.TrySetResult();
                    await release.Task;
                };
                var manager = CreateManager((0, failing), (0, sibling));
                var actual = manager.Initialize().AsTask();
                try
                {
                    await cancellationObserved.Task;
                    Assert.That(actual.IsCompleted, Is.False, "The callback failure cannot publish early terminal state.");
                    Assert.That(manager.IsInitializing, Is.True);
                }
                finally { release.TrySetResult(); }
                AggregateException failure = null;
                try { await actual; } catch (AggregateException error) { failure = error; }
                Assert.That(failure, Is.Not.Null);
                Assert.That(failure.Flatten().InnerExceptions, Does.Contain(primary));
                Assert.That(failure.Flatten().InnerExceptions, Does.Contain(callbackFailure));
                Assert.That(manager.LastException, Is.SameAs(failure));
                Assert.That(manager.IsInitializing, Is.False);
            });
        }

        [UnityTest]
        public IEnumerator ForcedCancellationJoinsAndPreservesTheLateSourceFailure()
        {
            return UniTask.ToCoroutine(async () =>
            {
                using var cancellation = new CancellationTokenSource();
                var release = new UniTaskCompletionSource();
                var primary = new InvalidOperationException("Actual source failed after cancellation.");
                var manager = CreateManager((0, _ => release.Task));
                var actual = manager.Initialize(cancellation.Token).AsTask();
                cancellation.Cancel();
                try
                {
                    Assert.That(actual.IsCompleted, Is.False);
                    Assert.That(manager.IsInitializing, Is.True);
                }
                finally { release.TrySetException(primary); }
                AggregateException failure = null;
                try { await actual; } catch (AggregateException error) { failure = error; }
                Assert.That(failure, Is.Not.Null);
                Assert.That(failure.Flatten().InnerExceptions, Does.Contain(primary));
                Assert.That(manager.LastException, Is.SameAs(failure));
                Assert.That(manager.IsInitializing, Is.False);
            });
        }

        [UnityTest]
        public IEnumerator ForeignCancellationRemainsTheActualFailureWhenCallerAlsoCancels()
        {
            return UniTask.ToCoroutine(async () =>
            {
                using var caller = new CancellationTokenSource();
                using var foreign = new CancellationTokenSource();
                foreign.Cancel();
                var release = new UniTaskCompletionSource();
                var foreignFailure = new OperationCanceledException(foreign.Token);
                var manager = CreateManager((0, async _ =>
                {
                    await release.Task;
                    throw foreignFailure;
                }));
                var actual = manager.Initialize(caller.Token).AsTask();
                caller.Cancel();
                try { Assert.That(actual.IsCompleted, Is.False); }
                finally { release.TrySetResult(); }
                AggregateException failure = null;
                try { await actual; } catch (AggregateException error) { failure = error; }
                Assert.That(failure, Is.Not.Null);
                Assert.That(failure.Flatten().InnerExceptions, Does.Contain(foreignFailure));
                Assert.That(manager.LastException, Is.SameAs(failure));
                Assert.That(manager.IsInitializing, Is.False);
            });
        }

        [UnityTest]
        public IEnumerator ConsumedCreditSeparatesRealActionInvocations()
        {
            return UniTask.ToCoroutine(async () =>
            {
                int nativeTick = 0, firstTick = -1, secondTick = -1;
                void Tick() => nativeTick++;
                UnityEditor.EditorApplication.update += Tick;
                try
                {
                    var manager = CreateManager((0, _ =>
                    {
                        firstTick = nativeTick;
                        ConsumeFrameCredit();
                        return UniTask.CompletedTask;
                    }), (0, _ =>
                    {
                        secondTick = nativeTick;
                        return UniTask.CompletedTask;
                    }));
                    await manager.Initialize();
                    Assert.That(firstTick, Is.GreaterThanOrEqualTo(0));
                    Assert.That(secondTick, Is.GreaterThan(firstTick));
                    Assert.That(manager.IsInitialized, Is.True);
                    Assert.That(manager.CurrentOrderExecutions,
                        Has.All.Property(nameof(InitializationActionExecution.Status)).EqualTo(InitializationActionStatus.Succeeded));
                }
                finally { UnityEditor.EditorApplication.update -= Tick; }
            });
        }

        [UnityTest]
        public IEnumerator AdmissionCancellationJoinsAcceptedSourceBeforeTerminal()
        {
            return UniTask.ToCoroutine(async () =>
            {
                using var cancellation = new CancellationTokenSource();
                var release = new UniTaskCompletionSource();
                var started = new UniTaskCompletionSource();
                bool laterRan = false;
                var manager = CreateManager((0, async _ =>
                {
                    ConsumeFrameCredit();
                    started.TrySetResult();
                    await release.Task;
                }), (0, _ =>
                {
                    laterRan = true;
                    return UniTask.CompletedTask;
                }));
                var actual = manager.Initialize(cancellation.Token).AsTask();
                await started.Task;
                cancellation.Cancel();
                try
                {
                    Assert.That(actual.IsCompleted, Is.False);
                    Assert.That(manager.IsInitializing, Is.True);
                    Assert.That(laterRan, Is.False);
                    Assert.That(manager.CurrentOrderExecutions[1].Status, Is.EqualTo(InitializationActionStatus.Pending));
                }
                finally { release.TrySetResult(); }
                OperationCanceledException failure = null;
                try { await actual; } catch (OperationCanceledException error) { failure = error; }
                Assert.That(failure, Is.Not.Null);
                Assert.That(failure.CancellationToken, Is.EqualTo(cancellation.Token));
                Assert.That(manager.LastException, Is.SameAs(failure));
                Assert.That(manager.IsInitializing, Is.False);
                Assert.That(manager.CurrentOrderExecutions[0].Status, Is.EqualTo(InitializationActionStatus.Canceled));
                Assert.That(laterRan, Is.False);
            });
        }

        [UnityTest]
        public IEnumerator SourceFailureDuringAdmissionRemainsTheActualFailure()
        {
            return UniTask.ToCoroutine(async () =>
            {
                var release = new UniTaskCompletionSource();
                var started = new UniTaskCompletionSource();
                var primary = new InvalidOperationException("Actual source failed during sibling admission.");
                bool laterRan = false;
                var manager = CreateManager((0, async _ =>
                {
                    ConsumeFrameCredit();
                    started.TrySetResult();
                    await release.Task;
                }), (0, _ =>
                {
                    laterRan = true;
                    return UniTask.CompletedTask;
                }));
                var actual = manager.Initialize().AsTask();
                await started.Task;
                release.TrySetException(primary);
                Exception failure = null;
                try { await actual; } catch (Exception error) { failure = error; }
                Assert.That(failure, Is.SameAs(primary));
                Assert.That(manager.LastException, Is.SameAs(primary));
                Assert.That(laterRan, Is.False);
                Assert.That(manager.CurrentOrderExecutions[0].Status, Is.EqualTo(InitializationActionStatus.Failed));
                Assert.That(manager.CurrentOrderExecutions[1].Status, Is.EqualTo(InitializationActionStatus.Canceled));
            });
        }

        private static void ConsumeFrameCredit()
        {
            var timer = System.Diagnostics.Stopwatch.StartNew();
            while (timer.Elapsed.TotalSeconds <= InitializationWorkSlice.BudgetSeconds * 2) Thread.SpinWait(32);
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
