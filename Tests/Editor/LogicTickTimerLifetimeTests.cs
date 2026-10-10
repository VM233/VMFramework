using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using VMFramework.Timers;

namespace VMFramework.Tests
{
    [Category("LogicTickTimerLifetime")]
    public sealed class LogicTickTimerLifetimeTests
    {
        private GameObject clockObject;
        private LogicTickManager clock;
        private GameObject timerObject;
        private LogicTickTimerManager timers;

        [SetUp]
        public void SetUp()
        {
            Assert.That(ReferenceEquals(LogicTickManager.Instance, null), Is.True);
            Assert.That(ReferenceEquals(LogicTickTimerManager.Instance, null), Is.True);
            CreateClock();
            timerObject = new GameObject(nameof(LogicTickTimerLifetimeTests));
            timers = timerObject.AddComponent<LogicTickTimerManager>();
            Invoke(timers, "Awake");
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                if (timerObject != null) RetireTimers();
            }
            finally
            {
                if (clockObject != null) RetireClock();
            }
            Assert.That(ReferenceEquals(LogicTickManager.Instance, null), Is.True);
            Assert.That(ReferenceEquals(LogicTickTimerManager.Instance, null), Is.True);
        }

        [Test]
        public void PendingTimerStopsAgainstItsClockAfterPublicationReplacement()
        {
            Invoke(timers, "OnBeforeInitStart");
            clock.IncreaseTick();
            var timer = new TimerProbe();
            timers.Add(timer, 10);
            RetireClock();
            CreateClock();

            Assert.That(clock.Tick, Is.Zero);
            Assert.That(timers.TryStop(timer), Is.True);
            Assert.That(timer.StoppedAt, Is.EqualTo(1ul));
            Assert.That(timer.StopCount, Is.EqualTo(1));
            Assert.That(timers.Contains(timer), Is.False);
        }

        [Test]
        public void PendingTimerCanRetireAfterItsManagerPublicationEnds()
        {
            Invoke(timers, "OnBeforeInitStart");
            clock.IncreaseTick();
            var timer = new TimerProbe();
            timers.Add(timer, 10);
            var originalOwner = timers;
            RetireTimers();

            Assert.That(ReferenceEquals(LogicTickTimerManager.Instance, null), Is.True);
            Assert.That(originalOwner.TryStop(timer), Is.True);
            Assert.That(timer.StoppedAt, Is.EqualTo(1ul));
            Assert.That(timer.StopCount, Is.EqualTo(1));
        }

        [Test]
        public void TimerManagerRetirementEndsTheAcquiredTickSubscription()
        {
            Invoke(timers, "OnBeforeInitStart");
            var timer = new TimerProbe();
            timers.Add(timer, 1);
            var originalOwner = timers;
            RetireTimers();
            clock.IncreaseTick();

            Assert.That(timer.TimedCount, Is.Zero);
            Assert.That(originalOwner.Contains(timer), Is.True);
            Assert.That(originalOwner.TryStop(timer), Is.True);
            Assert.That(timer.StoppedAt, Is.EqualTo(1ul));
        }

        [Test]
        public void OwnerWithoutAnAcquiredClockSubscriptionReleasesPublication()
        {
            RetireTimers();

            Assert.That(ReferenceEquals(LogicTickTimerManager.Instance, null), Is.True);
            Assert.That(LogicTickManager.Instance, Is.SameAs(clock));
        }

        private void CreateClock()
        {
            clockObject = new GameObject(nameof(LogicTickTimerLifetimeTests) + " Clock");
            clock = clockObject.AddComponent<LogicTickManager>();
            Invoke(clock, "Awake");
        }

        private void RetireClock()
        {
            try { Invoke(clock, "OnDestroy"); }
            finally
            {
                Object.DestroyImmediate(clockObject);
                clockObject = null;
                clock = null;
            }
        }

        private void RetireTimers()
        {
            try { Invoke(timers, "OnDestroy"); }
            finally
            {
                Object.DestroyImmediate(timerObject);
                timerObject = null;
                timers = null;
            }
        }

        private static void Invoke(object owner, string method)
        {
            owner.GetType().GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(owner, null);
        }

        private sealed class TimerProbe : ITimer<ulong>
        {
            public ulong Priority { get; set; }
            public int QueueIndex { get; set; }
            public long InsertionIndex { get; set; }
            public ulong StoppedAt { get; private set; }
            public int StopCount { get; private set; }
            public int TimedCount { get; private set; }

            public void OnTimed() => TimedCount++;

            public void OnStopped(ulong time)
            {
                StoppedAt = time;
                StopCount++;
            }
        }
    }
}
