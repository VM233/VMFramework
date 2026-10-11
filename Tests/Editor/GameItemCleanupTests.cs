using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;
using Assert = NUnit.Framework.Assert;
using Object = UnityEngine.Object;

namespace VMFramework.Tests
{
    public sealed class GameItemCleanupTests
    {
        private sealed class Prefab : GamePrefab
        {
            public Prefab(string identity) { id = identity; }
        }

        private sealed class ManagedItem : GameItem
        {
            public Action ReturnAction;
            public int Returned, Cleared;
            protected override void OnReturn() { Returned++; base.OnReturn(); ReturnAction?.Invoke(); }
            protected override void OnClear() => Cleared++;
        }

        private readonly List<GameObject> nativeProducts = new();
        private string id;
        private GameItemManager manager;
        private const BindingFlags PRIVATE_INSTANCE = BindingFlags.Instance | BindingFlags.NonPublic;

        [SetUp]
        public void SetUp()
        {
            id = "cleanup_fixture_" + Guid.NewGuid().ToString("N");
            Assert.That(GamePrefabManager.RegisterGamePrefab(new Prefab(id)), Is.True);
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var product in nativeProducts)
                if (product != null) Object.DestroyImmediate(product);
            nativeProducts.Clear();
            if (!ReferenceEquals(manager, null))
            {
                Assert.That(ReferenceEquals(GameItemManager.Instance, null), Is.True);
                manager = null;
            }
            GamePrefabManager.UnregisterGamePrefab(id);
        }

        private GameObject Native(string name)
        {
            var product = new GameObject(name);
            nativeProducts.Add(product);
            return product;
        }

        private ControllerCleanupTestItem NativeItem() => Native("Cleanup Rental").AddComponent<ControllerCleanupTestItem>();

        private CreatablePoolItemsPool<ControllerCleanupTestItem, string> Pool(Func<ControllerCleanupTestItem> factory) =>
            new(id, _ => factory(), 1);

        [TestCase(false)]
        [TestCase(true)]
        public void FailedFreshGetPrefixClearsOnlyHeldResources(bool cleanupFails)
        {
            var primary = new InvalidOperationException("second get callback");
            var retirement = new InvalidOperationException("second return callback");
            var items = new List<ControllerCleanupTestItem>();
            var held = new List<GameObject[]>();
            var releases = new List<int[]>();
            var pool = Pool(() =>
            {
                int rental = items.Count;
                var item = NativeItem(); items.Add(item);
                var resources = new GameObject[3]; held.Add(resources);
                var counts = new int[3]; releases.Add(counts);
                for (int index = 0; index < 3; index++)
                {
                    int slot = index;
                    item.GetEvents.Add(PriorityDefines.MEDIUM, _ =>
                    {
                        resources[slot] = Native("Held Callback Resource");
                        if (rental == 0 && slot == 1) throw primary;
                    });
                    item.ReturnEvents.Add(PriorityDefines.MEDIUM, _ =>
                    {
                        var actual = resources[slot]; resources[slot] = null;
                        if (actual != null) { counts[slot]++; Object.DestroyImmediate(actual); }
                        if (rental == 0 && slot == 1 && cleanupFails) throw retirement;
                    });
                }
                return item;
            });
            ControllerCleanupTestItem unpublished = null;
            if (cleanupFails)
            {
                var product = Assert.Throws<PoolItemAcquisitionException>(() => unpublished = pool.Get(out _));
                Assert.That(product.Pool, Is.SameAs(pool));
                Assert.That(product.Item, Is.SameAs(items[0]));
                Assert.That(product.Phase, Is.EqualTo("create.clear"));
                Assert.That(product.InnerExceptions[0], Is.SameAs(primary));
                Assert.That(product.InnerExceptions[1], Is.SameAs(retirement));
            }
            else Assert.That(Assert.Throws<InvalidOperationException>(() => unpublished = pool.Get(out _)), Is.SameAs(primary));
            Assert.That(unpublished, Is.Null);
            Assert.That(pool.Count, Is.Zero);
            Assert.That(releases[0], Is.EqualTo(new[] { 1, 1, 0 }));
            Assert.That(held[0], Is.EqualTo(new GameObject[3]));
            Assert.That(items[0].Returned, Is.EqualTo(1));
            Assert.That(items[0].Cleared, Is.EqualTo(1));
            Assert.That(items[0].NativeDestroyed, Is.EqualTo(1));
            Assert.That(items[0] == null, Is.True);
            var control = pool.Get(out _);
            Assert.That(control.Created, Is.EqualTo(1));
            pool.Return(control);
            pool.Clear();
            Assert.That(releases[1], Is.EqualTo(new[] { 1, 1, 1 }));
            Assert.That(control.Cleared, Is.EqualTo(1));
        }

        [Test]
        public void ReturnSnapshotContinuesSamePriorityAfterRemovalAndFailure()
        {
            var pool = Pool(NativeItem);
            var item = pool.Get(out _);
            var order = new List<int>();
            var first = new InvalidOperationException("first callback");
            var second = new InvalidOperationException("second callback");
            IReturnEventProvider.ReturnHandler later = _ => { order.Add(2); throw second; };
            item.ReturnEvents.Add(PriorityDefines.MEDIUM, _ =>
            { order.Add(1); item.ReturnEvents.Remove(later); throw first; });
            item.ReturnEvents.Add(PriorityDefines.MEDIUM, later);
            item.ReturnEvents.Add(PriorityDefines.MEDIUM, _ => order.Add(3));
            bool deactivated = false;
            item.ReturnAction = () => deactivated = !item.gameObject.activeSelf;
            int buffers = ListPool<IReturnEventProvider.ReturnHandler>.Default.Count;
            var product = Assert.Throws<GameItemCleanupException>(() => pool.Return(item));
            Assert.That(product.Item, Is.SameAs(item));
            Assert.That(product.Phase, Is.EqualTo("pool.return"));
            Assert.That(product.InnerExceptions, Is.EqualTo(new Exception[] { first, second }));
            Assert.That(order, Is.EqualTo(new[] { 1, 2, 3 }));
            Assert.That(item.Returned, Is.EqualTo(1));
            Assert.That(deactivated, Is.True);
            Assert.That(item.Cleared, Is.EqualTo(1));
            Assert.That(item.NativeDestroyed, Is.EqualTo(1));
            Assert.That(item == null, Is.True);
            Assert.That(item.IsDestroyed, Is.True);
            Assert.That(pool.Count, Is.Zero);
            Assert.That(ListPool<IReturnEventProvider.ReturnHandler>.Default.Count, Is.EqualTo(Math.Max(1, buffers)));
        }

        [Test]
        public void ClearAttemptsRootCleanupAfterCallbackAndReturnFailures()
        {
            var item = Pool(NativeItem).Get(out _);
            var first = new InvalidOperationException("first callback");
            var second = new InvalidOperationException("second callback");
            var rootReturn = new InvalidOperationException("root return");
            var rootClear = new InvalidOperationException("root clear");
            int last = 0;
            item.ReturnEvents.Add(PriorityDefines.MEDIUM, _ => throw first);
            item.ReturnEvents.Add(PriorityDefines.MEDIUM, _ => throw second);
            item.ReturnEvents.Add(PriorityDefines.MEDIUM, _ => last++);
            item.ReturnAction = () => throw rootReturn;
            item.ClearAction = () => throw rootClear;
            var product = Assert.Throws<GameItemCleanupException>(() => ((IPoolItem)item).OnClear());
            Assert.That(product.Item, Is.SameAs(item));
            Assert.That(product.Phase, Is.EqualTo("pool.clear"));
            Assert.That(product.InnerExceptions, Is.EqualTo(new Exception[] { first, second, rootReturn, rootClear }));
            Assert.That(last, Is.EqualTo(1));
            Assert.That(item.Returned, Is.EqualTo(1));
            Assert.That(item.Cleared, Is.EqualTo(1));
            Assert.That(item.NativeDestroyed, Is.EqualTo(1));
            Assert.That(item == null, Is.True);
        }

        [Test]
        public void ClearPreservesSingleRootFailureAndDestroysNativeObject()
        {
            var pool = Pool(NativeItem);
            var item = pool.Get(out _);
            var original = new InvalidOperationException("root return");
            item.ReturnAction = () => throw original;
            Assert.That(Assert.Throws<InvalidOperationException>(() => ((IPoolItem)item).OnClear()), Is.SameAs(original));
            Assert.That(item.Returned, Is.EqualTo(1));
            Assert.That(item.Cleared, Is.EqualTo(1));
            Assert.That(item == null, Is.True);
            var control = pool.Get(out _);
            Assert.DoesNotThrow(() => ((IPoolItem)control).OnClear());
            Assert.That(control.Returned, Is.EqualTo(1));
            Assert.That(control.Cleared, Is.EqualTo(1));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void DestructFailuresDoNotSkipOtherSubscribersOrControllerDeactivation(bool native)
        {
            IGameItem item = native ? Pool(NativeItem).Get(out _) :
                new CreatablePoolItemsPool<ManagedItem, string>(id, _ => new ManagedItem(), 1).Get(out _);
            var first = new InvalidOperationException("first subscriber");
            var last = new InvalidOperationException("last subscriber");
            var order = new List<int>();
            item.OnDestructed += _ => { order.Add(1); throw first; };
            item.OnDestructed += _ => order.Add(2);
            item.OnDestructed += _ => { order.Add(3); throw last; };
            var product = Assert.Throws<GameItemCleanupException>(() => ((IPoolItem)item).OnReturn());
            Assert.That(product.Item, Is.SameAs(item));
            Assert.That(product.Phase, Is.EqualTo("return.notification"));
            Assert.That(product.InnerExceptions, Is.EqualTo(new Exception[] { first, last }));
            Assert.That(order, Is.EqualTo(new[] { 1, 2, 3 }));
            Assert.That(item.IsDestroyed, Is.True);
            if (native) Assert.That(((ControllerCleanupTestItem)item).gameObject.activeSelf, Is.False);
        }

        [Test]
        public void ManagedClearAttemptsRootClearAfterReturnFailure()
        {
            var pool = new CreatablePoolItemsPool<ManagedItem, string>(id, _ => new ManagedItem(), 1);
            var item = pool.Get(out _);
            var original = new InvalidOperationException("managed root return");
            item.ReturnAction = () => throw original;
            Assert.That(Assert.Throws<InvalidOperationException>(() => ((IPoolItem)item).OnClear()), Is.SameAs(original));
            Assert.That(item.Returned, Is.EqualTo(1));
            Assert.That(item.Cleared, Is.EqualTo(1));
            var control = pool.Get(out _);
            Assert.DoesNotThrow(() => ((IPoolItem)control).OnClear());
            Assert.That(control.Returned, Is.EqualTo(1));
            Assert.That(control.Cleared, Is.EqualTo(1));
        }

        [Test]
        public void NativeDestroyPublishesLivenessBeforeOwnerCallbacksAndRejectsReturnClone()
        {
            Assert.That(ReferenceEquals(GameItemManager.Instance, null), Is.True);
            manager = Native("Cleanup Manager").AddComponent<GameItemManager>();
            typeof(GameItemManager).GetMethod("Awake", PRIVATE_INSTANCE).Invoke(manager, null);
            int factories = 0;
            typeof(GameItemManager).GetField("createGameItemHandler", PRIVATE_INSTANCE).SetValue(manager,
                new Func<string, IGameItem>(_ => { factories++; return NativeItem(); }));
            var item = manager.Get<ControllerCleanupTestItem>(id);
            bool seen = false;
            item.NativeDestroyedAction = () => seen = item.IsDestroyed;
            Object.DestroyImmediate(item.gameObject);
            Assert.That(seen, Is.True);
            Assert.That(item.IsDestroyed, Is.True);
            Assert.That(item.NativeDestroyed, Is.EqualTo(1));
            Assert.Throws<InvalidOperationException>(() => manager.Return(item));
            Assert.Throws<InvalidOperationException>(() => manager.Clone(item, StateCloneContext.Empty));
            var pools = (Dictionary<string, CreatablePoolItemsPool<IGameItem, string>>)typeof(GameItemManager)
                .GetField("pools", PRIVATE_INSTANCE).GetValue(manager);
            Assert.That(pools[id].Count, Is.Zero);
            Assert.That(factories, Is.EqualTo(1));
            // EditMode test components use the same explicit singleton retirement as the existing focused fixture.
            typeof(GameItemManager).GetMethod("OnDestroy", PRIVATE_INSTANCE).Invoke(manager, null);
        }

        [Test]
        public void QueuedClearAttemptsEveryNativeStorageWithoutRepeatingReturn()
        {
            var products = new List<ControllerCleanupTestItem>();
            var failure = new InvalidOperationException("first native storage");
            var pool = new CreatablePoolItemsPool<ControllerCleanupTestItem, string>(id,
                _ => { var product = NativeItem(); products.Add(product); return product; }, 2);
            var first = pool.Get(out _); var second = pool.Get(out _);
            int returns = 0;
            first.ReturnEvents.Add(PriorityDefines.MEDIUM, _ => returns++);
            second.ReturnEvents.Add(PriorityDefines.MEDIUM, _ => returns++);
            pool.Return(first); pool.Return(second);
            first.ClearAction = () => throw failure;
            var product = Assert.Throws<PoolItemAcquisitionException>(() => pool.Clear());
            Assert.That(product.Pool, Is.SameAs(pool));
            Assert.That(product.Item, Is.SameAs(first));
            Assert.That(product.Phase, Is.EqualTo("clear.storage"));
            Assert.That(product.InnerExceptions[0], Is.SameAs(failure));
            Assert.That(first.Returned, Is.EqualTo(1));
            Assert.That(second.Returned, Is.EqualTo(1));
            Assert.That(returns, Is.EqualTo(2));
            Assert.That(first.Cleared, Is.EqualTo(1));
            Assert.That(second.Cleared, Is.EqualTo(1));
            Assert.That(first == null, Is.True); Assert.That(second == null, Is.True);
            Assert.That(pool.Count, Is.Zero);
            var control = pool.Get(out bool fresh);
            Assert.That(fresh, Is.True);
            Assert.That(ReferenceEquals(control, first), Is.False);
            Assert.That(ReferenceEquals(control, second), Is.False);
            pool.Return(control); pool.Clear();
            Assert.That(control.Returned, Is.EqualTo(1));
            Assert.That(control.Cleared, Is.EqualTo(1));
        }
    }
}
