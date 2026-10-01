using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using VMFramework.GameEvents;

namespace VMFramework.Tests
{
    public sealed class PriorityEventsTests
    {
        [TestCase(1)]
        [TestCase(77)]
        [TestCase(256)]
        public void CombinedPublicationPreservesEveryRegistrationInOrder(int count)
        {
            var events = new PriorityEvents<Action>();
            var observed = new List<int>();
            for (int index = 0; index < count; index++)
            {
                int captured = index;
                events.Add(0, () => observed.Add(captured));
            }
            Assert.That(events.Count, Is.EqualTo(count));
            var first = events.GetCombinedCallbacks().Single();
            first();
            CollectionAssert.AreEqual(Enumerable.Range(0, count), observed);
            Assert.That(first.GetInvocationList().Length, Is.EqualTo(count));
            Assert.That(events.GetCombinedCallbacks().Single(), Is.SameAs(first));
        }

        [Test]
        public void NumericPriorityAndDuplicateAdmissionPreserveOriginalMembership()
        {
            var events = new PriorityEvents<Action>();
            var observed = new List<string>();
            Action first = () => observed.Add("first");
            Action second = () => observed.Add("second");
            Action earlier = () => observed.Add("earlier");
            events.Add(8, first);
            events.Add(8, second);
            events.Add(-4, earlier);
            events.Add(-100, first);
            foreach (var callback in events.GetCombinedCallbacks()) callback();
            CollectionAssert.AreEqual(new[] {"earlier", "first", "second"}, observed);
            Assert.That(events.Count, Is.EqualTo(3));
            Assert.That(events.GetCombinedCallbacks().Count, Is.EqualTo(2));
            CollectionAssert.AreEquivalent(new[] {first, second, earlier}, events);
        }

        [Test]
        public void RemovingOverlappingMulticastRetiresTheExactRegistration()
        {
            var events = new PriorityEvents<Action>();
            var observed = new List<string>();
            Action a = () => observed.Add("a");
            Action b = () => observed.Add("b");
            Action c = () => observed.Add("c");
            Action ab = a + b;
            events.Add(0, a);
            events.Add(0, c);
            events.Add(0, ab);
            events.Remove(a);
            events.GetCombinedCallbacks().Single()();
            CollectionAssert.AreEqual(new[] {"c", "a", "b"}, observed);
            Assert.That(events.Count, Is.EqualTo(2));
            observed.Clear();
            events.Remove(ab);
            events.GetCombinedCallbacks().Single()();
            CollectionAssert.AreEqual(new[] {"c"}, observed);
            events.Remove(ab);
            Assert.That(events.Count, Is.EqualTo(1));
        }

        [Test]
        public void LiveViewAndImmutableSnapshotsHaveIndependentLifetimes()
        {
            var events = new PriorityEvents<Action>();
            var observed = new List<int>();
            Action a = () => observed.Add(1);
            Action b = () => observed.Add(2);
            Action c = () => observed.Add(3);
            events.Add(0, a);
            events.Add(0, b);
            events.Add(1, c);
            var view = events.GetCombinedCallbacks();
            var previous = view.ToArray();
            events.Remove(a);
            var current = view.ToArray();
            Assert.That(current[0], Is.Not.SameAs(previous[0]));
            Assert.That(current[1], Is.SameAs(previous[1]));
            previous[0]();
            current[0]();
            CollectionAssert.AreEqual(new[] {1, 2, 2}, observed);
            events.Clear();
            Assert.That(events.Count, Is.Zero);
            Assert.That(view.Count, Is.Zero);
            Assert.That(view, Is.Empty);
            events.Add(2, a);
            Assert.That(view.Count, Is.EqualTo(1));
            Assert.That(view.Single(), Is.SameAs(a));
            events.Remove(a);
            Assert.That(view.Count, Is.Zero);
        }

        [Test]
        public void MembershipChangeInvalidatesActivePriorityEnumeration()
        {
            var events = new PriorityEvents<Action>();
            events.Add(0, () => {});
            events.Add(1, () => {});
            using var iterator = events.GetCombinedCallbacks().GetEnumerator();
            Assert.That(iterator.MoveNext(), Is.True);
            events.Add(0, () => {});
            Assert.Throws<InvalidOperationException>(() => iterator.MoveNext());
        }

        [Test]
        public void ReturnValuesAndCallbackExceptionsRetainDelegateSemantics()
        {
            var events = new PriorityEvents<Func<int, int>>();
            events.Add(0, value => value * 2);
            events.Add(0, value => value + 1);
            Assert.That(events.GetCombinedCallbacks().Single()(5), Is.EqualTo(6));
            var failure = new InvalidOperationException("callback owner failure");
            bool invokedAfterFailure = false;
            var throwing = new PriorityEvents<Action>();
            throwing.Add(0, () => throw failure);
            throwing.Add(0, () => invokedAfterFailure = true);
            Assert.That(Assert.Throws<InvalidOperationException>(() => throwing.GetCombinedCallbacks().Single()()),
                Is.SameAs(failure));
            Assert.That(invokedAfterFailure, Is.False);
        }

        private delegate void Mutate(ref int value);

        [Test]
        public void RefArgumentsFlowThroughTheOrderedInvocationList()
        {
            var events = new PriorityEvents<Mutate>();
            events.Add(0, (ref int value) => value *= 2);
            events.Add(0, (ref int value) => value += 1);
            int value = 5;
            events.GetCombinedCallbacks().Single()(ref value);
            Assert.That(value, Is.EqualTo(11));
        }

        [Test]
        public void InvalidInputFailsAtAdmissionAndAbsentRemovalIsIdempotent()
        {
            var events = new PriorityEvents<Action>();
            Assert.Throws<ArgumentNullException>(() => events.Add(0, null));
            Assert.Throws<ArgumentNullException>(() => events.Remove(null));
            Action absent = () => {};
            events.Remove(absent);
            Assert.That(events.Count, Is.Zero);
            Assert.That(events.GetCombinedCallbacks(), Is.Empty);
        }
    }
}
