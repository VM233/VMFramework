using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using VMFramework.Core.Pools;
using VMFramework.GameEvents;
using VMFramework.GameLogicArchitecture;
using Object = UnityEngine.Object;

namespace VMFramework.Tests
{
    public sealed class GameEventPropagationTests
    {
        [Test]
        public void ParameterlessRepeatedPropagationKeepsOtherPoolLeasesIndependent()
        {
            var gameEvent = new ParameterlessProbe();
            gameEvent.AddCallback(() => {}, 0);
            VerifyIndependentLeases<Action>(gameEvent.Propagate);
            Assert.That(gameEvent.Completions, Is.EqualTo(3));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ParameterizedRepeatedPropagationKeepsOtherPoolLeasesIndependent(bool propagateAction)
        {
            var gameEvent = new ParameterizedProbe();
            gameEvent.AddCallback((int _) => {}, 0);
            gameEvent.AddCallback(() => {}, 1);
            VerifyIndependentLeases<(Action<int>, Action)>(() => gameEvent.Propagate(7, propagateAction));
            Assert.That(gameEvent.Completions, Is.EqualTo(3));
        }

        [Test]
        public void ParameterlessReentryAdoptsNewMembershipWithoutChangingTheOuterSnapshot()
        {
            var gameEvent = new ParameterlessProbe();
            var observed = new List<string>();
            int depth = 0;
            Action late = () => observed.Add($"old:{depth}");
            Action replacement = () => observed.Add($"new:{depth}");
            gameEvent.AddCallback(() =>
            {
                observed.Add($"first:{depth}");
                if (depth == 0)
                {
                    gameEvent.RemoveCallback(late);
                    gameEvent.AddCallback(replacement, 2);
                    depth++;
                    try { gameEvent.Propagate(); }
                    finally { depth--; }
                }
            }, 0);
            gameEvent.AddCallback(late, 1);

            gameEvent.Propagate();

            CollectionAssert.AreEqual(new[] {"first:0", "first:1", "new:1", "old:0"}, observed);
            Assert.That(gameEvent.Completions, Is.EqualTo(2));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ParameterizedReentryPreservesOuterArgumentsAndSnapshot(bool propagateAction)
        {
            var gameEvent = new ParameterizedProbe();
            var observed = new List<string>();
            Action<int> late = argument => observed.Add($"old:{argument}");
            Action<int> replacement = argument => observed.Add($"new:{argument}");
            gameEvent.AddCallback((int argument) =>
            {
                observed.Add($"first:{argument}");
                if (argument == 1)
                {
                    gameEvent.RemoveCallback(late);
                    gameEvent.AddCallback(replacement, 2);
                    gameEvent.Propagate(2, propagateAction);
                }
            }, 0);
            gameEvent.AddCallback(() => observed.Add("action"), 0);
            gameEvent.AddCallback(late, 1);

            gameEvent.Propagate(1, propagateAction);

            var expected = propagateAction
                ? new[] {"first:1", "first:2", "action", "new:2", "action", "old:1"}
                : new[] {"first:1", "first:2", "new:2", "old:1"};
            CollectionAssert.AreEqual(expected, observed);
            Assert.That(gameEvent.Completions, Is.EqualTo(2));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void CallbackMutationIsAdoptedOnTheNextPropagation(bool parameterized)
        {
            var (propagate, add, remove, completions) = CreateActionEvent(parameterized);
            var observed = new List<string>();
            bool changed = false;
            Action old = () => observed.Add("old");
            Action replacement = () => observed.Add("new");
            add(() =>
            {
                observed.Add("first");
                if (!changed)
                {
                    changed = true;
                    remove(old);
                    add(replacement, 1);
                }
            }, 0);
            add(old, 1);

            propagate();
            propagate();

            CollectionAssert.AreEqual(new[] {"first", "old", "first", "new"}, observed);
            Assert.That(completions(), Is.EqualTo(2));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void CallbackFailureKeepsItsIdentityAndDoesNotCompleteTheInvocation(bool parameterized)
        {
            var (propagate, add, remove, completions) = CreateActionEvent(parameterized);
            var failure = new InvalidOperationException("callback owner failure");
            int laterCalls = 0;
            Action throwing = () => throw failure;
            add(throwing, 0);
            add(() => laterCalls++, 1);

            Assert.That(Assert.Throws<InvalidOperationException>(() => propagate()), Is.SameAs(failure));
            Assert.That(laterCalls, Is.Zero);
            Assert.That(completions(), Is.Zero);
            remove(throwing);
            propagate();
            Assert.That(laterCalls, Is.EqualTo(1));
            Assert.That(completions(), Is.EqualTo(1));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ParameterizedPriorityAndActionAdmissionStayAtTheProducer(bool propagateAction)
        {
            var gameEvent = new ParameterizedProbe();
            var observed = new List<string>();
            gameEvent.AddCallback(() => observed.Add("late action"), 10);
            gameEvent.AddCallback((int value) => observed.Add($"early argument:{value}"), -10);
            gameEvent.AddCallback(() => observed.Add("early action"), -10);
            gameEvent.AddCallback((int value) => observed.Add($"late argument:{value}"), 10);

            gameEvent.Propagate(17, propagateAction);

            var expected = propagateAction
                ? new[] {"early argument:17", "early action", "late argument:17", "late action"}
                : new[] {"early argument:17", "late argument:17"};
            CollectionAssert.AreEqual(expected, observed);
            Assert.That(gameEvent.Completions, Is.EqualTo(1));
        }

        [Test]
        public void ColliderReentryKeepsTheOuterEventTypeAndSnapshot()
        {
            var host = new GameObject("Collider Event Snapshot Test");
            try
            {
                var trigger = host.AddComponent<ColliderMouseEventTrigger>();
                var observed = new List<MouseEventType>();
                trigger.AddCallback(MouseEventType.PointerEnter, (sender, eventType) =>
                {
                    observed.Add(eventType);
                    sender.TriggerEvent(MouseEventType.PointerExit);
                    observed.Add(eventType);
                });
                trigger.AddCallback(MouseEventType.PointerExit, (_, eventType) => observed.Add(eventType));

                trigger.TriggerEvent(MouseEventType.PointerEnter);

                CollectionAssert.AreEqual(new[]
                {
                    MouseEventType.PointerEnter, MouseEventType.PointerExit, MouseEventType.PointerEnter
                }, observed);
            }
            finally { Object.DestroyImmediate(host); }
        }

        [Test]
        public void ColliderNestedCallbackFailureDoesNotInvalidateTheOuterSnapshot()
        {
            var host = new GameObject("Collider Event Failure Test");
            try
            {
                var trigger = host.AddComponent<ColliderMouseEventTrigger>();
                var failure = new InvalidOperationException("nested collider callback failure");
                int resumed = 0;
                trigger.AddCallback(MouseEventType.PointerExit, (_, _) => throw failure);
                trigger.AddCallback(MouseEventType.PointerEnter, (sender, _) =>
                {
                    Assert.That(Assert.Throws<InvalidOperationException>(
                        () => sender.TriggerEvent(MouseEventType.PointerExit)), Is.SameAs(failure));
                    resumed++;
                });

                trigger.TriggerEvent(MouseEventType.PointerEnter);

                Assert.That(resumed, Is.EqualTo(1));
            }
            finally { Object.DestroyImmediate(host); }
        }

        private static void VerifyIndependentLeases<T>(Action propagate)
        {
            var pool = ListPool<T>.Default;
            var previous = new List<List<T>>(pool.Count);
            int count = pool.Count;
            Assert.That(count, Is.LessThanOrEqualTo(500));
            for (int index = 0; index < count; index++) previous.Add(pool.Get());
            List<T> first = null, second = null;
            try
            {
                propagate();
                propagate();
                first = pool.Get();
                second = pool.Get();
                Assert.That(second, Is.Not.SameAs(first));
                first.Clear();
                first.Add(default);
                propagate();
                Assert.That(first.Count, Is.EqualTo(1), "An active event cannot mutate another owner's lease.");
            }
            finally
            {
                if (first != null) { first.Clear(); pool.Return(first); }
                if (second != null && !ReferenceEquals(first, second)) { second.Clear(); pool.Return(second); }
                foreach (var list in previous) pool.Return(list);
            }
        }

        private static (Action propagate, Action<Action, int> add, Action<Action> remove, Func<int> completions)
            CreateActionEvent(bool parameterized)
        {
            if (parameterized)
            {
                var gameEvent = new ParameterizedProbe();
                return (() => gameEvent.Propagate(17), gameEvent.AddCallback, gameEvent.RemoveCallback,
                    () => gameEvent.Completions);
            }
            var parameterless = new ParameterlessProbe();
            return (parameterless.Propagate, parameterless.AddCallback, parameterless.RemoveCallback,
                () => parameterless.Completions);
        }

        private static void SetPrefab(GameItem gameEvent)
        {
            typeof(GameItem).GetProperty("GamePrefab", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(gameEvent, new GameEventConfig {id = "propagation_lifetime_test"});
        }

        private sealed class ParameterlessProbe : ParameterlessGameEvent
        {
            public int Completions { get; private set; }

            public ParameterlessProbe() { SetPrefab(this); OnCreate(); }

            protected override void OnPropagationStopped() => Completions++;
        }

        private sealed class ParameterizedProbe : ParameterizedGameEvent<int>
        {
            public int Completions { get; private set; }

            public ParameterizedProbe() { SetPrefab(this); OnCreate(); }

            protected override void OnPropagationStopped() => Completions++;
        }
    }
}
