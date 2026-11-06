using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine.TestTools;
using VMFramework.Procedure;

namespace VMFramework.Tests
{
    public sealed class InitializationDeadlineTests
    {
        [UnityTest]
        public IEnumerator InactiveTimeDoesNotConsumeTheDeadlineAndActiveTimeStillExpires()
        {
            return UniTask.ToCoroutine(async () =>
            {
                var activity = new TestInitializationActivity(false);
                using var deadline = new InitializationDeadline(TimeSpan.FromMilliseconds(80), activity);
                await UniTask.Delay(160, DelayType.Realtime);
                Assert.That(deadline.IsCancellationRequested, Is.False);
                activity.Set(true);
                await UniTask.WaitUntil(() => deadline.IsCancellationRequested);
                Assert.That(deadline.IsCancellationRequested, Is.True);
            });
        }

        [UnityTest]
        public IEnumerator PauseAndResumePreserveTheRemainingActiveBudget()
        {
            return UniTask.ToCoroutine(async () =>
            {
                var activity = new TestInitializationActivity(true);
                using var deadline = new InitializationDeadline(TimeSpan.FromMilliseconds(400), activity);
                await UniTask.Delay(30, DelayType.Realtime);
                activity.Set(false);
                await UniTask.Delay(500, DelayType.Realtime);
                Assert.That(deadline.IsCancellationRequested, Is.False);
                activity.Set(true);
                await UniTask.Yield();
                Assert.That(deadline.IsCancellationRequested, Is.False);
                await UniTask.WaitUntil(() => deadline.IsCancellationRequested);
            });
        }

        [UnityTest]
        public IEnumerator DisposalRetiresTheActivityAndTimer()
        {
            return UniTask.ToCoroutine(async () =>
            {
                var activity = new TestInitializationActivity(true);
                var deadline = new InitializationDeadline(TimeSpan.FromMilliseconds(20), activity);
                deadline.Dispose();
                Assert.That(activity.Disposed, Is.True);
                activity.Set(false);
                activity.Set(true);
                await UniTask.Delay(50, DelayType.Realtime);
            });
        }
    }
}
