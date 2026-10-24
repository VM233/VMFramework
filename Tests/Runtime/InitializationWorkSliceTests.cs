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

        [UnityTest]
        public IEnumerator CompleteManagerDiscoveryPreservesActualOrderAndTypeKinds()
            => UniTask.ToCoroutine(async () =>
            {
                var expected = typeof(IManagerBehaviour).GetDerivedClasses(true, false).ToArray();
                var actual = await InitializationTypeDiscovery.FindAllAsync(typeof(IManagerBehaviour), true,
                    CancellationToken.None);
                Assert.That(actual, Is.EqualTo(expected));
                Assert.That(actual.Count, Is.GreaterThan(0));
            });

        [UnityTest]
        public IEnumerator AlreadyCanceledManagerPreparationPreservesPublication()
            => UniTask.ToCoroutine(() => AssertManagerPreparationCancellation(false));

        [UnityTest]
        public IEnumerator PendingManagerPreparationCancellationPreservesPublication()
            => UniTask.ToCoroutine(() => AssertManagerPreparationCancellation(true));

        private static async UniTask AssertManagerPreparationCancellation(bool cancelPendingSource)
        {
            var managers = ManagerCreator.Managers.ToArray();
            var concreteTypes = ManagerCreator.ManagerTypes.ToArray();
            var abstractTypes = ManagerCreator.AbstractManagerTypes.ToArray();
            var interfaceTypes = ManagerCreator.InterfaceManagerTypes.ToArray();
            var container = ManagerCreatorContainers.ManagerContainer;
            var containers = ManagerCreatorContainers.ManagerTypeContainers.ToArray();
            using var cancellation = new CancellationTokenSource();
            if (cancelPendingSource)
            {
                _ = new InitializationWorkSlice();
                ConsumeOneSlice();
            }
            else cancellation.Cancel();
            var source = ManagerCreator.CreateManagersAsync(cancellation.Token);
            if (cancelPendingSource)
            {
                Assert.That(source.Status, Is.EqualTo(UniTaskStatus.Pending));
                cancellation.Cancel();
            }
            OperationCanceledException failure = null;
            try { await source; }
            catch (OperationCanceledException actual) { failure = actual; }
            Assert.That(failure, Is.Not.Null);
            Assert.That(failure.CancellationToken, Is.EqualTo(cancellation.Token));
            Assert.That(ManagerCreator.Managers, Is.EqualTo(managers));
            Assert.That(ManagerCreator.ManagerTypes, Is.EqualTo(concreteTypes));
            Assert.That(ManagerCreator.AbstractManagerTypes, Is.EqualTo(abstractTypes));
            Assert.That(ManagerCreator.InterfaceManagerTypes, Is.EqualTo(interfaceTypes));
            Assert.That(ManagerCreatorContainers.ManagerContainer, Is.SameAs(container));
            Assert.That(ManagerCreatorContainers.ManagerTypeContainers.ToArray(), Is.EqualTo(containers));
        }

        private static void ConsumeOneSlice()
        {
            var timer = Stopwatch.StartNew();
            while (timer.Elapsed.TotalSeconds <= InitializationWorkSlice.BudgetSeconds * 2)
                Thread.SpinWait(32);
        }
    }
}
