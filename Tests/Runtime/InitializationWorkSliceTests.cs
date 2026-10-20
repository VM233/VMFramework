using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VMFramework.Procedure;

namespace VMFramework.Tests
{
    public sealed class InitializationWorkSliceTests
    {
        [UnityTest]
        public IEnumerator ExpiredBudgetReturnsToTheActualPlayerLoop()
        {
            return UniTask.ToCoroutine(async () =>
            {
                var slice = new InitializationWorkSlice();
                ConsumeOneSlice();
                int frame = Time.frameCount;
                await slice.YieldIfBudgetExpired(CancellationToken.None);
                Assert.That(Time.frameCount, Is.GreaterThan(frame));
            });
        }

        [UnityTest]
        public IEnumerator CancellationRetiresThePendingFrameWait()
        {
            return UniTask.ToCoroutine(async () =>
            {
                using var cancellation = new CancellationTokenSource();
                var slice = new InitializationWorkSlice();
                ConsumeOneSlice();
                var actual = slice.YieldIfBudgetExpired(cancellation.Token);
                Assert.That(actual.Status, Is.EqualTo(UniTaskStatus.Pending));
                cancellation.Cancel();
                OperationCanceledException failure = null;
                try { await actual; }
                catch (OperationCanceledException error) { failure = error; }
                Assert.That(failure, Is.Not.Null);
                Assert.That(failure.CancellationToken, Is.EqualTo(cancellation.Token));
            });
        }

        private static void ConsumeOneSlice()
        {
            var timer = Stopwatch.StartNew();
            while (timer.Elapsed.TotalSeconds <= InitializationWorkSlice.BudgetSeconds * 2)
                Thread.SpinWait(32);
        }
    }
}
