using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VMFramework.Procedure;
using VMFramework.Configuration;
using VMFramework.Core;
using Assert = NUnit.Framework.Assert;

namespace VMFramework.Tests
{
    public sealed class InitializationWorkSliceTests
    {
        [UnityTest]
        public IEnumerator NewSiblingCannotResetConsumedFrameCredit()
        {
            return UniTask.ToCoroutine(async () =>
            {
                _ = new InitializationWorkSlice();
                ConsumeOneSlice();
                var sibling = new InitializationWorkSlice();
                int frame = Time.frameCount;
                var actual = sibling.YieldIfBudgetExpired(CancellationToken.None);
                Assert.That(actual.Status, Is.EqualTo(UniTaskStatus.Pending));
                await actual;
                Assert.That(Time.frameCount, Is.GreaterThan(frame));
            });
        }

        [UnityTest]
        public IEnumerator CooperativeDiscoveryPreservesTheActualTypeDomain()
        {
            return UniTask.ToCoroutine(async () =>
            {
                var expected = typeof(IAsyncCheckableConfig).GetDerivedInstantiableClasses(false).ToArray();
                var actual = await InitializationTypeDiscovery.FindAsync(typeof(IAsyncCheckableConfig), false,
                    CancellationToken.None);
                Assert.That(actual, Is.EquivalentTo(expected));
            });
        }

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
