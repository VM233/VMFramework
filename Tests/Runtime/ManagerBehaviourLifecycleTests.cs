using System;
using System.Collections;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace VMFramework.Tests
{
    [Category("ManagerSingletonLifetime")]
    public sealed class ManagerBehaviourLifecycleTests
    {
        private GameObject first;
        private GameObject second;

        [SetUp]
        public void SetUp()
        {
            Assert.That(Application.isPlaying, Is.True);
            Assert.That(ReferenceEquals(ManagerPublicationTestOwner.Instance, null), Is.True);
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                if (second != null) Object.DestroyImmediate(second);
            }
            finally
            {
                if (first != null) Object.DestroyImmediate(first);
                Assert.That(ReferenceEquals(ManagerPublicationTestOwner.Instance, null), Is.True);
            }
        }

        [Test]
        public void ReplacingDestroyedBaseComponentPublishesTheDerivedInterfaceOwner()
        {
            first = new GameObject("Manager Interface Replacement");
            var original = first.AddComponent<ManagerPublicationTestOwner>();
            Assert.That(ReferenceEquals(ManagerPublicationTestOwner.Instance, original), Is.True);
            Object.DestroyImmediate(original);
            Assert.That(ReferenceEquals(ManagerPublicationTestOwner.Instance, null), Is.True);
            var replacement = first.AddComponent<DerivedManagerPublicationTestOwner>();
            Assert.That(ReferenceEquals(ManagerPublicationTestOwner.Instance, replacement), Is.True);
            Object.DestroyImmediate(first);
            Assert.That(ReferenceEquals(ManagerPublicationTestOwner.Instance, null), Is.True);
        }

        [UnityTest]
        public IEnumerator DeferredDestructionReleasesTheInterfaceBeforeRecreation()
        {
            first = new GameObject("Deferred Manager Interface Owner");
            var original = first.AddComponent<ManagerPublicationTestOwner>();
            Assert.That(ReferenceEquals(ManagerPublicationTestOwner.Instance, original), Is.True);
            Object.Destroy(first);
            yield return null;
            Assert.That(ReferenceEquals(ManagerPublicationTestOwner.Instance, null), Is.True);
            second = new GameObject("Recreated Manager Interface Owner");
            var replacement = second.AddComponent<DerivedManagerPublicationTestOwner>();
            Assert.That(ReferenceEquals(ManagerPublicationTestOwner.Instance, replacement), Is.True);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ASecondLiveOwnerIsRejectedWithoutReplacingOrRetiringTheFirst(bool derivedCandidate)
        {
            first = new GameObject("Original Live Manager Interface Owner");
            var original = first.AddComponent<ManagerPublicationTestOwner>();
            second = new GameObject("Duplicate Live Manager Interface Owner");
            LogAssert.Expect(LogType.Exception,
                new Regex("InvalidOperationException: Manager singleton publication rejected at Awake:.*different physical owner"));
            if (derivedCandidate) second.AddComponent<DerivedManagerPublicationTestOwner>();
            else second.AddComponent<ManagerPublicationTestOwner>();
            Assert.That(ReferenceEquals(ManagerPublicationTestOwner.Instance, original), Is.True);
            Object.DestroyImmediate(second);
            Assert.That(ReferenceEquals(ManagerPublicationTestOwner.Instance, original), Is.True);
            Object.DestroyImmediate(first);
            Assert.That(ReferenceEquals(ManagerPublicationTestOwner.Instance, null), Is.True);
        }

        [Test]
        public void ThrowingDerivedCleanupStillReleasesItsExactInterfaceOwner()
        {
            first = new GameObject("Failing Manager Interface Retirement");
            var original = first.AddComponent<ManagerPublicationTestOwner>();
            original.RetirementFailure = new InvalidOperationException("Manager publication fixture retirement failure.");
            LogAssert.Expect(LogType.Exception,
                new Regex("InvalidOperationException: Manager publication fixture retirement failure"));
            Object.DestroyImmediate(first);
            Assert.That(ReferenceEquals(ManagerPublicationTestOwner.Instance, null), Is.True);
            second = new GameObject("Manager Interface Owner After Retirement Failure");
            var replacement = second.AddComponent<DerivedManagerPublicationTestOwner>();
            Assert.That(ReferenceEquals(ManagerPublicationTestOwner.Instance, replacement), Is.True);
        }

        [Test]
        public void AnInvalidClosedGenericContractIsRejectedBeforePublication()
        {
            first = new GameObject("Invalid Manager Interface Candidate");
            LogAssert.Expect(LogType.Exception,
                new Regex("InvalidOperationException: Manager singleton publication rejected at Awake:.*does not implement"));
            first.AddComponent<InvalidManagerPublicationTestOwner>();
            Assert.That(ReferenceEquals(ManagerPublicationTestOwner.Instance, null), Is.True);
        }
    }
}
