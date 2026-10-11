using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;
using Object = UnityEngine.Object;

namespace VMFramework.Tests
{
    public sealed class GameItemAcquisitionTests
    {
        private sealed class Prefab : GamePrefab
        {
            public Prefab(string identity) { id = identity; }
        }
        private class Item : GameItem
        {
            public Action BeforeReturnAction, ReturnedAction, CopyAction;
            public int Created, Returned, Copied, Cleared;
            protected override void OnCreate() { base.OnCreate(); Created++; }
            protected override void OnReturn()
            { Returned++; BeforeReturnAction?.Invoke(); base.OnReturn(); ReturnedAction?.Invoke(); }
            protected override void OnClear() => Cleared++;
            public override void CloneFrom(IStateCloner source, StateCloneContext context)
            { Copied++; CopyAction?.Invoke(); }
        }
        private sealed class OtherItem : Item { }
        private sealed class MissingClonerItem : Item, IStateClonerOwner
        {
            IStateCloner IStateClonerOwner.StateCloner => null;
        }

        private const BindingFlags PRIVATE_INSTANCE = BindingFlags.Instance | BindingFlags.NonPublic;
        private readonly List<Item> created = new();
        private GameObject root;
        private GameItemManager manager;
        private string id;
        private Func<Item> factory;

        [SetUp]
        public void SetUp()
        {
            Assert.That(ReferenceEquals(GameItemManager.Instance, null), Is.True);
            id = "rental_fixture_" + Guid.NewGuid().ToString("N");
            Assert.That(GamePrefabManager.RegisterGamePrefab(new Prefab(id)), Is.True);
            root = new GameObject("Rental Acquisition Fixture");
            manager = root.AddComponent<GameItemManager>();
            typeof(GameItemManager).GetMethod("Awake", PRIVATE_INSTANCE).Invoke(manager, null);
            typeof(GameItemManager).GetField("createGameItemHandler", PRIVATE_INSTANCE).SetValue(manager,
                new Func<string, IGameItem>(_ =>
                {
                    var item = factory == null ? new Item() : factory();
                    created.Add(item);
                    return item;
                }));
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var item in created) { item.BeforeReturnAction = null; item.ReturnedAction = null; item.CopyAction = null; }
            foreach (var pool in Pools.Values) pool.Clear();
            typeof(GameItemManager).GetMethod("OnDestroy", PRIVATE_INSTANCE).Invoke(manager, null);
            Object.DestroyImmediate(root);
            GamePrefabManager.UnregisterGamePrefab(id);
            created.Clear();
            factory = null;
            Assert.That(ReferenceEquals(GameItemManager.Instance, null), Is.True);
        }

        private Dictionary<string, CreatablePoolItemsPool<IGameItem, string>> Pools =>
            (Dictionary<string, CreatablePoolItemsPool<IGameItem, string>>)typeof(GameItemManager)
                .GetField("pools", PRIVATE_INSTANCE).GetValue(manager);

        [TestCase(false, false)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(true, true)]
        public void CreationNotificationFailureReturnsTheUnpublishedRental(bool typed, bool reused)
        {
            Item earlier = null;
            if (reused) { earlier = manager.Get<Item>(id); manager.Return(earlier); }
            var primary = new InvalidOperationException("creation notification");
            Action<IGameItem> observer = _ => throw primary;
            manager.OnGameItemCreated += observer;
            IGameItem unpublished = null;
            Assert.That(Assert.Throws<InvalidOperationException>(() =>
                unpublished = typed ? manager.Get<Item>(id) : manager.Get(id)), Is.SameAs(primary));
            manager.OnGameItemCreated -= observer;
            Assert.That(unpublished, Is.Null);
            Assert.That(created, Has.Count.EqualTo(1));
            var item = created[0];
            Assert.That(item.Returned, Is.EqualTo(reused ? 2 : 1));
            Assert.That(item.IsDestroyed, Is.True);
            Assert.That(Pools[id].Count, Is.EqualTo(1));
            var control = manager.Get<Item>(id);
            Assert.That(control, Is.SameAs(item));
            Assert.That(control.Created, Is.EqualTo(1));
            if (reused) Assert.That(control, Is.SameAs(earlier));
            manager.Return(control);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void TypedMismatchReturnsTheUnpublishedRental(bool reused)
        {
            if (reused) manager.Return(manager.Get<Item>(id));
            int notifications = 0;
            manager.OnGameItemCreated += _ => notifications++;
            Assert.Throws<InvalidCastException>(() => manager.Get<OtherItem>(id));
            Assert.That(notifications, Is.EqualTo(1));
            Assert.That(Pools[id].Count, Is.EqualTo(1));
            var control = manager.Get<Item>(id);
            Assert.That(control, Is.SameAs(created[0]));
            Assert.That(control.Created, Is.EqualTo(1));
            manager.Return(control);
        }

        [Test]
        public void CloneCopyFailureReturnsOnlyItsUnpublishedRental()
        {
            var source = manager.Get<Item>(id);
            var primary = new InvalidOperationException("state copy");
            factory = () => new Item { CopyAction = () => throw primary };
            Assert.That(Assert.Throws<InvalidOperationException>(() => manager.Clone(source, StateCloneContext.Empty)),
                Is.SameAs(primary));
            Assert.That(source.IsDestroyed, Is.False);
            Assert.That(created[1].Returned, Is.EqualTo(1));
            Assert.That(Pools[id].Count, Is.EqualTo(1));
            created[1].CopyAction = null;
            var control = manager.Clone(source, StateCloneContext.Empty);
            Assert.That(control, Is.SameAs(created[1]));
            manager.Return(control);
            manager.Return(source);
        }

        [Test]
        public void CloneNotificationFailureReturnsOnlyItsUnpublishedRental()
        {
            var source = manager.Get<Item>(id);
            var primary = new InvalidOperationException("clone notification");
            Action<IGameItem> observer = _ => throw primary;
            manager.OnGameItemCreated += observer;
            Assert.That(Assert.Throws<InvalidOperationException>(() => manager.Clone(source, StateCloneContext.Empty)),
                Is.SameAs(primary));
            manager.OnGameItemCreated -= observer;
            Assert.That(source.IsDestroyed, Is.False);
            Assert.That(created[1].Copied, Is.EqualTo(1));
            Assert.That(created[1].Returned, Is.EqualTo(1));
            manager.Return(source);
        }

        [Test]
        public void MissingClonerFailureReturnsTheUnpublishedRental()
        {
            IGameItem source = manager.Get<Item>(id);
            factory = () => new MissingClonerItem();
            Assert.Throws<InvalidOperationException>(() => manager.Clone(source, StateCloneContext.Empty));
            Assert.That(source.IsDestroyed, Is.False);
            Assert.That(created[1].Returned, Is.EqualTo(1));
            manager.Return(source);
        }

        [Test]
        public void GetPublicationAndReturnFailuresRetainBothOriginalErrors()
        {
            var primary = new InvalidOperationException("creation notification");
            var secondary = new InvalidOperationException("return");
            factory = () => new Item { ReturnedAction = () => throw secondary };
            manager.OnGameItemCreated += _ => throw primary;
            var product = Assert.Throws<PoolItemAcquisitionException>(() => manager.Get(id));
            Assert.That(product.InnerExceptions[0], Is.SameAs(primary));
            Assert.That(product.InnerExceptions[1], Is.SameAs(secondary));
            Assert.That(product.Pool, Is.SameAs(Pools[id]));
            Assert.That(product.Item, Is.SameAs(created[0]));
            Assert.That(product.Phase, Is.EqualTo("game-item.get.return"));
            Assert.That(created[0].Returned, Is.EqualTo(1));
            Assert.That(Pools[id].Count, Is.Zero);
        }

        [Test]
        public void CloneCopyAndReturnFailuresRetainBothOriginalErrors()
        {
            var source = manager.Get<Item>(id);
            var primary = new InvalidOperationException("state copy");
            var secondary = new InvalidOperationException("return");
            factory = () => new Item { CopyAction = () => throw primary, ReturnedAction = () => throw secondary };
            var product = Assert.Throws<PoolItemAcquisitionException>(() => manager.Clone(source, StateCloneContext.Empty));
            Assert.That(product.InnerExceptions[0], Is.SameAs(primary));
            Assert.That(product.InnerExceptions[1], Is.SameAs(secondary));
            Assert.That(product.Pool, Is.SameAs(Pools[id]));
            Assert.That(product.Item, Is.SameAs(created[1]));
            Assert.That(product.Phase, Is.EqualTo("game-item.clone.return"));
            Assert.That(created[1].Returned, Is.EqualTo(1));
            Assert.That(source.IsDestroyed, Is.False);
            Assert.That(Pools[id].Count, Is.Zero);
            manager.Return(source);
        }

        [Test]
        public void PrewarmPublicationFailureRetiresTheCurrentAndEarlierRentals()
        {
            var primary = new InvalidOperationException("second notification");
            int notifications = 0;
            manager.OnGameItemCreated += _ => { if (++notifications == 2) throw primary; };
            int buffers = ListPool<IGameItem>.Default.Count;
            Assert.That(Assert.Throws<InvalidOperationException>(() => manager.PrewarmUntil(id, 3)), Is.SameAs(primary));
            Assert.That(created, Has.Count.EqualTo(2));
            Assert.That(created[0].Returned, Is.EqualTo(1));
            Assert.That(created[1].Returned, Is.EqualTo(1));
            Assert.That(Pools[id].Count, Is.EqualTo(2));
            Assert.That(ListPool<IGameItem>.Default.Count, Is.EqualTo(Math.Max(1, buffers)));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void PrewarmContinuesIndependentReturnsAfterTheFirstReturnFailure(bool secondAlsoFails)
        {
            var primary = new InvalidOperationException("second notification");
            var firstReturn = new InvalidOperationException("first return");
            var secondReturn = new InvalidOperationException("second return");
            factory = () => new Item
            {
                ReturnedAction = created.Count == 0 ? () => throw firstReturn :
                    secondAlsoFails ? () => throw secondReturn : null
            };
            int notifications = 0;
            manager.OnGameItemCreated += _ => { if (++notifications == 2) throw primary; };
            int buffers = ListPool<IGameItem>.Default.Count;
            var product = Assert.Throws<AggregateException>(() => manager.PrewarmUntil(id, 3));
            Assert.That(product.InnerExceptions[0], Is.SameAs(primary));
            Assert.That(product.InnerExceptions, Has.Count.EqualTo(secondAlsoFails ? 3 : 2));
            var first = (PoolItemAcquisitionException)product.InnerExceptions[1];
            Assert.That(first.Pool, Is.SameAs(Pools[id]));
            Assert.That(first.Item, Is.SameAs(created[0]));
            Assert.That(first.Phase, Is.EqualTo("prewarm.return"));
            Assert.That(first.InnerExceptions[0], Is.SameAs(firstReturn));
            if (secondAlsoFails)
            {
                var second = (PoolItemAcquisitionException)product.InnerExceptions[2];
                Assert.That(second.Pool, Is.SameAs(Pools[id]));
                Assert.That(second.Item, Is.SameAs(created[1]));
                Assert.That(second.Phase, Is.EqualTo("prewarm.return"));
                Assert.That(second.InnerExceptions[0], Is.SameAs(secondReturn));
            }
            Assert.That(created[0].Returned, Is.EqualTo(1));
            Assert.That(created[1].Returned, Is.EqualTo(1));
            Assert.That(Pools[id].Count, Is.EqualTo(secondAlsoFails ? 0 : 1));
            Assert.That(ListPool<IGameItem>.Default.Count, Is.EqualTo(Math.Max(1, buffers)));
        }

        [Test]
        public void PrewarmReturnFailureRetainsTheExactFailedRentalIdentity()
        {
            var primary = new InvalidOperationException("first return");
            factory = () => new Item { ReturnedAction = created.Count == 0 ? () => throw primary : null };
            var product = Assert.Throws<PoolItemAcquisitionException>(() => manager.PrewarmUntil(id, 2));
            Assert.That(product.Pool, Is.SameAs(Pools[id]));
            Assert.That(product.Item, Is.SameAs(created[0]));
            Assert.That(product.Phase, Is.EqualTo("prewarm.return"));
            Assert.That(product.InnerExceptions, Has.Count.EqualTo(1));
            Assert.That(product.InnerExceptions[0], Is.SameAs(primary));
            Assert.That(created[0].Returned, Is.EqualTo(1));
            Assert.That(created[1].Returned, Is.EqualTo(1));
            Assert.That(Pools[id].Count, Is.EqualTo(1));
        }

        [Test]
        public void PrewarmReturnNotificationFailureDoesNotSkipLaterReturns()
        {
            var primary = new InvalidOperationException("first return notification");
            manager.OnGameItemDestroyed += item => { if (ReferenceEquals(item, created[0])) throw primary; };
            var product = Assert.Throws<PoolItemAcquisitionException>(() => manager.PrewarmUntil(id, 2));
            Assert.That(product.Pool, Is.SameAs(Pools[id]));
            Assert.That(product.Item, Is.SameAs(created[0]));
            Assert.That(product.Phase, Is.EqualTo("prewarm.return"));
            var publication = (PoolItemAcquisitionException)product.InnerExceptions[0];
            Assert.That(publication.Pool, Is.SameAs(Pools[id]));
            Assert.That(publication.Item, Is.SameAs(created[0]));
            Assert.That(publication.Phase, Is.EqualTo("return.retirement"));
            Assert.That(publication.InnerExceptions[0], Is.SameAs(primary));
            Assert.That(created[0].Returned, Is.EqualTo(1));
            Assert.That(created[1].Returned, Is.EqualTo(1));
            Assert.That(created[0].Cleared, Is.EqualTo(1));
            Assert.That(Pools[id].Count, Is.EqualTo(1));
        }

        [Test]
        public void PrewarmFactoryFailureRetiresOnlyEarlierAcquisitions()
        {
            var primary = new InvalidOperationException("third factory");
            factory = () => created.Count == 2 ? throw primary : new Item();
            Assert.That(Assert.Throws<InvalidOperationException>(() => manager.PrewarmUntil(id, 3)), Is.SameAs(primary));
            Assert.That(created, Has.Count.EqualTo(2));
            Assert.That(created[0].Returned, Is.EqualTo(1));
            Assert.That(created[1].Returned, Is.EqualTo(1));
            Assert.That(Pools[id].Count, Is.EqualTo(2));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void FailedPublicationAndRootRetirementStillWithdrawEveryActualConsumer(bool reused)
        {
            if (reused) manager.Return(manager.Get<Item>(id));
            var primary = new InvalidOperationException("later created listener");
            var rootFailure = new InvalidOperationException("root before base return");
            var listenerFailure = new InvalidOperationException("first destroyed listener");
            var held = new HashSet<IGameItem>();
            Action<IGameItem> adopt = item => held.Add(item);
            Action<IGameItem> fail = _ => throw primary;
            Action<IGameItem> first = _ => throw listenerFailure;
            int later = 0;
            Action<IGameItem> withdraw = item =>
            { Assert.That(item.IsDestroyed, Is.True); Assert.That(held.Remove(item), Is.True); later++; };
            if (reused) created[0].BeforeReturnAction = () => throw rootFailure;
            else factory = () => new Item { BeforeReturnAction = () => throw rootFailure };
            manager.OnGameItemCreated += adopt; manager.OnGameItemCreated += fail;
            manager.OnGameItemDestroyed += first; manager.OnGameItemDestroyed += withdraw;
            IGameItem unpublished = null;
            var product = Assert.Throws<PoolItemAcquisitionException>(() => unpublished = manager.Get(id));
            manager.OnGameItemCreated -= adopt; manager.OnGameItemCreated -= fail;
            manager.OnGameItemDestroyed -= first; manager.OnGameItemDestroyed -= withdraw;
            Assert.That(unpublished, Is.Null);
            Assert.That(product.Pool, Is.SameAs(Pools[id]));
            Assert.That(product.Item, Is.SameAs(created[0]));
            Assert.That(product.InnerExceptions[0], Is.SameAs(primary));
            var retirement = (PoolItemAcquisitionException)product.InnerExceptions[1];
            Assert.That(retirement.Pool, Is.SameAs(Pools[id]));
            Assert.That(retirement.Item, Is.SameAs(created[0]));
            Assert.That(retirement.InnerExceptions, Is.EqualTo(new Exception[] { rootFailure, listenerFailure }));
            Assert.That(held, Is.Empty);
            Assert.That(later, Is.EqualTo(1));
            Assert.That(created[0].IsDestroyed, Is.True);
            Assert.That(created[0].Returned, Is.EqualTo(reused ? 2 : 1));
            Assert.That(created[0].Cleared, Is.EqualTo(1));
            Assert.That(Pools[id].Count, Is.Zero);
            factory = null;
            var control = manager.Get<Item>(id);
            Assert.That(control, Is.Not.SameAs(created[0]));
            Assert.That(control.IsDestroyed, Is.False);
            manager.Return(control);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void DestroyedReentryCannotReuseTheRentalBeingPublished(bool firstAlsoFails)
        {
            var old = manager.Get<Item>(id);
            Item incoming = null;
            var failure = new InvalidOperationException("first destroyed listener");
            var order = new List<int>();
            Action<IGameItem> first = item =>
            {
                order.Add(1);
                Assert.That(item, Is.SameAs(old));
                incoming = manager.Get<Item>(id);
                Assert.That(incoming, Is.Not.SameAs(old));
                if (firstAlsoFails) throw failure;
            };
            Action<IGameItem> last = item =>
            {
                order.Add(2);
                Assert.That(item, Is.SameAs(old));
                Assert.That(item.IsDestroyed, Is.True);
                Assert.That(incoming.IsDestroyed, Is.False);
            };
            manager.OnGameItemDestroyed += first; manager.OnGameItemDestroyed += last;
            if (firstAlsoFails)
            {
                var product = Assert.Throws<PoolItemAcquisitionException>(() => manager.Return(old));
                Assert.That(product.Pool, Is.SameAs(Pools[id]));
                Assert.That(product.Item, Is.SameAs(old));
                Assert.That(product.InnerExceptions[0], Is.SameAs(failure));
                Assert.That(Pools[id].Count, Is.Zero);
                Assert.That(old.Cleared, Is.EqualTo(1));
            }
            else
            {
                manager.Return(old);
                Assert.That(Pools[id].Count, Is.EqualTo(1));
                Assert.That(old.Cleared, Is.Zero);
            }
            manager.OnGameItemDestroyed -= first; manager.OnGameItemDestroyed -= last;
            Assert.That(order, Is.EqualTo(new[] { 1, 2 }));
            Assert.That(old.Returned, Is.EqualTo(1));
            Assert.That(incoming.IsDestroyed, Is.False);
            manager.Return(incoming);
            Assert.That(incoming.Returned, Is.EqualTo(1));
        }

        [Test]
        public void ReentrantNewReturnUsesCurrentCapacityWithoutRepeatingLeaseExit()
        {
            var actualFactory = (Func<string, IGameItem>)typeof(GameItemManager)
                .GetField("createGameItemHandler", PRIVATE_INSTANCE).GetValue(manager);
            Pools.Add(id, new CreatablePoolItemsPool<IGameItem, string>(id, actualFactory, 1));
            var old = manager.Get<Item>(id);
            Item incoming = null;
            int oldNotifications = 0;
            Action<IGameItem> publisher = item =>
            {
                if (!ReferenceEquals(item, old)) return;
                oldNotifications++;
                incoming = manager.Get<Item>(id);
                manager.Return(incoming);
            };
            manager.OnGameItemDestroyed += publisher;
            manager.Return(old);
            manager.OnGameItemDestroyed -= publisher;
            Assert.That(Pools[id].Count, Is.EqualTo(1));
            Assert.That(old.Returned, Is.EqualTo(1));
            Assert.That(old.Cleared, Is.EqualTo(1));
            Assert.That(oldNotifications, Is.EqualTo(1));
            Assert.That(incoming, Is.Not.SameAs(old));
            var control = manager.Get<Item>(id);
            Assert.That(control, Is.SameAs(incoming));
            Assert.That(control.IsDestroyed, Is.False);
            manager.Return(control);
            Assert.That(old.Cleared, Is.EqualTo(1));
        }
    }
}
