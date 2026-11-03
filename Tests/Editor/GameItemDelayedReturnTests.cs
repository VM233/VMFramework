using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using VMFramework.GameLogicArchitecture;
using VMFramework.Timers;
using Object = UnityEngine.Object;

namespace VMFramework.Tests
{
    public sealed class GameItemDelayedReturnTests
    {
        private sealed class TimedItem : GameItem { }
        private readonly List<GameObject> objects = new();
        private GameItemManager items;
        private TimerManager timers;
        private GameItemDelayedReturner returner;
        private string identity;
        private IGameItem item;
        private const BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;

        [SetUp]
        public void CreateActualTimerAndPoolOwners()
        {
            identity = "delayed_return_" + Guid.NewGuid().ToString("N");
            GamePrefabManager.RegisterGamePrefab(new VMFramework.Effects.EffectConfig { id = identity });
            items = Manager<GameItemManager>();
            timers = Manager<TimerManager>();
            returner = Manager<GameItemDelayedReturner>();
            typeof(GameItemManager).GetField("createGameItemHandler", Private).SetValue(items,
                new Func<string, IGameItem>(_ => new TimedItem()));
            item = items.Get(identity);
        }

        private T Manager<T>() where T : Component
        {
            var host = new GameObject(typeof(T).Name);
            host.SetActive(false);
            objects.Add(host);
            var result = host.AddComponent<T>();
            typeof(T).GetMethod("Awake", Private).Invoke(result, null);
            return result;
        }

        private IDictionary Pending => (IDictionary)typeof(GameItemDelayedReturner)
            .GetField("timersLookup", Private).GetValue(returner);

        [TearDown]
        public void DestroyExactManagers()
        {
            if (!item.IsDestroyed) items.Return(item);
            foreach (var component in new Component[] { returner, timers, items })
                component.GetType().GetMethod("OnDestroy", Private).Invoke(component, null);
            foreach (var host in objects) Object.DestroyImmediate(host);
            objects.Clear();
            GamePrefabManager.UnregisterGamePrefab(identity);
        }

        [Test]
        public void RepeatedRequestsOwnOnlyOneTimerAndOneRetirement()
        {
            returner.DelayedReturn(item, 10);
            returner.DelayedReturn(item, 20);
            Assert.That(Pending.Count, Is.EqualTo(1));
            AdvanceTo(11);
            Assert.That(item.IsDestroyed, Is.True);
            Assert.That(Pending.Count, Is.Zero);
            AdvanceTo(21);
        }

        [Test]
        public void ReturnAndRerentCannotKeepAnEarlierDeadline()
        {
            returner.DelayedReturn(item, 10);
            items.Return(item);
            Assert.That(Pending.Count, Is.Zero);
            var next = items.Get(identity);
            Assert.That(next, Is.SameAs(item));
            AdvanceTo(11);
            Assert.That(next.IsDestroyed, Is.False);
            returner.DelayedReturn(next, 20);
            AdvanceTo(32);
            Assert.That(next.IsDestroyed, Is.True);
        }

        [Test]
        public void CallbackReentrancyCannotDeleteItsReplacementSchedule()
        {
            bool replace = true;
            items.OnGameItemDestroyed += retired =>
            {
                if (!replace) return;
                replace = false;
                item = items.Get(identity);
                returner.DelayedReturn(item, 20);
            };
            returner.DelayedReturn(item, 10);
            AdvanceTo(11);
            Assert.That(item.IsDestroyed, Is.False);
            Assert.That(Pending.Count, Is.EqualTo(1));
            AdvanceTo(32);
            Assert.That(item.IsDestroyed, Is.True);
            Assert.That(Pending.Count, Is.Zero);
        }

        [Test]
        public void CancelUnhooksTheTimerAndAllowsANewRequest()
        {
            returner.DelayedReturn(item, 10);
            returner.CancelReturn(item);
            AdvanceTo(11);
            Assert.That(item.IsDestroyed, Is.False);
            Assert.That(Pending.Count, Is.Zero);
            returner.DelayedReturn(item, 10);
            AdvanceTo(22);
            Assert.That(item.IsDestroyed, Is.True);
        }

        [Test]
        public void ReturnerRetirementRemovesEveryOutstandingTimer()
        {
            returner.DelayedReturn(item, 10);
            typeof(GameItemDelayedReturner).GetMethod("OnDestroy", Private).Invoke(returner, null);
            Assert.That(Pending.Count, Is.Zero);
            AdvanceTo(11);
            Assert.That(item.IsDestroyed, Is.False);
        }

        private void AdvanceTo(double time)
        {
            typeof(TimerManager).GetField("currentTime", Private).SetValue(timers, time);
            typeof(TimerManager).GetMethod("Update", Private).Invoke(timers, null);
        }
    }
}
