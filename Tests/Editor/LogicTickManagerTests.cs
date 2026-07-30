using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using VMFramework.Timers;

namespace VMFramework.Tests
{
    public sealed class LogicTickManagerTests
    {
        private GameObject managerObject;
        private LogicTickManager manager;

        [SetUp]
        public void SetUp()
        {
            LogicTickManager.Instance = null;
            managerObject = new GameObject(nameof(LogicTickManagerTests));
            manager = managerObject.AddComponent<LogicTickManager>();
        }

        [TearDown]
        public void TearDown()
        {
            if (managerObject != null)
            {
                UnityEngine.Object.DestroyImmediate(managerObject);
            }

            LogicTickManager.Instance = null;
        }

        [Test]
        public void IncreaseTickRunsSimulationPhasesAfterNextTickActions()
        {
            var order = new List<string>();
            Action deferredFromSimulation = () => order.Add("deferred");

            manager.OnPreTick += () => order.Add("pre");
            manager.OnTick += () => order.Add("logic");
            manager.OnNextTick += () => order.Add("next");
            manager.OnPreSimulationTick += () => order.Add("pre-simulation");
            manager.OnSimulationTick += () =>
            {
                order.Add("simulation");
                manager.OnNextTick += deferredFromSimulation;
            };
            manager.OnPostSimulationTick += () =>
                order.Add("post-simulation");
            manager.OnPostTick += () => order.Add("post");

            manager.IncreaseTick();

            Assert.That(order, Is.EqualTo(new[]
            {
                "pre",
                "logic",
                "next",
                "pre-simulation",
                "simulation",
                "post-simulation",
                "post"
            }));

            order.Clear();
            manager.IncreaseTick();

            Assert.That(order, Is.EqualTo(new[]
            {
                "pre",
                "logic",
                "deferred",
                "pre-simulation",
                "simulation",
                "post-simulation",
                "post"
            }));
        }

        [Test]
        public void AdvanceTimeUsesTheActiveTickGap()
        {
            manager.SetTickGap(0.25);
            manager.StartTick();

            var tickCount = 0;
            manager.OnTick += () => tickCount++;

            var advanced = manager.AdvanceTime(0.6);

            Assert.That(advanced, Is.EqualTo(2));
            Assert.That(tickCount, Is.EqualTo(2));
            Assert.That(manager.Tick, Is.EqualTo(2));
            Assert.That(manager.TickDeltaTime, Is.EqualTo(0.25f));
            Assert.That(manager.TimeLeftOver, Is.EqualTo(0.1).Within(0.000001));
            Assert.That(manager.TickInterpolationAlpha,
                Is.EqualTo(0.4f).Within(0.000001f));

            manager.StopTick();
            Assert.That(manager.AdvanceTime(1), Is.EqualTo(0));
            Assert.That(manager.Tick, Is.EqualTo(2));
        }

        [Test]
        public void TickGapChangesApplyAfterTheAdmittedTick()
        {
            manager.SetTickGap(0.25);
            manager.StartTick();
            manager.OnTick += () =>
            {
                if (manager.Tick == 1)
                {
                    manager.SetTickGap(0.5);
                }
            };

            var advanced = manager.AdvanceTime(0.6);

            Assert.That(advanced, Is.EqualTo(1));
            Assert.That(manager.Tick, Is.EqualTo(1));
            Assert.That(manager.TimeLeftOver,
                Is.EqualTo(0.35).Within(0.000001));
            Assert.That(manager.TickGap, Is.EqualTo(0.5));
        }

        [TestCase(0)]
        [TestCase(-0.1)]
        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        public void SetTickGapRejectsInvalidValues(double tickGap)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => manager.SetTickGap(tickGap));
        }
    }
}
