using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using VMFramework.GameLogicArchitecture;
using Object = UnityEngine.Object;

namespace VMFramework.Tests
{
    public sealed class GameItemReferenceTests
    {
        private sealed class ReferencePrefab : GamePrefab
        {
            public ReferencePrefab(string identity) { id = identity; }
        }

        private sealed class ManagedItem : GameItem { }
        private sealed class ControllerItem : ControllerGameItem { }

        private sealed class RentalManager : GameItemManager
        {
            public void Initialize(Func<string, IGameItem> factory)
            {
                base.Awake();
                createGameItemHandler = factory;
            }
        }

        private sealed class ReferenceManager : GameItemReferenceManager
        {
            public void Initialize() { base.Awake(); }
            public int PublishedCount => references.Count;
        }

        private readonly List<GameObject> objects = new();
        private IGameItemManager previousRentalManager;
        private GameItemReferenceManager previousReferenceManager;
        private RentalManager rentals;
        private ReferenceManager references;
        private string identity;
        private readonly Vector2 queryPosition = new(20000, 20000);

        [SetUp]
        public void SetUp()
        {
            previousRentalManager = GameItemManager.Instance;
            previousReferenceManager = GameItemReferenceManager.Instance;
            GameItemManager.Instance = null;
            GameItemReferenceManager.Instance = null;
            identity = "reference_fixture_" + Guid.NewGuid().ToString("N");
            Assert.That(GamePrefabManager.RegisterGamePrefab(new ReferencePrefab(identity)), Is.True);
            rentals = CreateObject("Reference Rental Manager").AddComponent<RentalManager>();
            references = CreateObject("Reference Cache Manager").AddComponent<ReferenceManager>();
            references.Initialize();
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var item in objects)
                if (item != null) Object.DestroyImmediate(item);
            objects.Clear();
            GamePrefabManager.UnregisterGamePrefab(identity);
            GameItemManager.Instance = previousRentalManager;
            GameItemReferenceManager.Instance = previousReferenceManager;
        }

        [Test]
        public void ControllerReferenceIsLiveCachedAndAbsentFromNativePhysics()
        {
            rentals.Initialize(_ => CreateController());
            int initialized = 0;
            references.OnInitialize += item =>
            {
                initialized++;
                Assert.That(((ControllerItem)item).gameObject.activeSelf, Is.True);
                ((ControllerItem)item).gameObject.SetActive(true);
            };

            var reference = (ControllerItem)references.Get(identity);
            var referenceCollider = reference.GetComponent<BoxCollider2D>();
            Physics2D.SyncTransforms();
            Assert.That(reference.IsDestroyed, Is.False);
            Assert.That(reference.id, Is.EqualTo(identity));
            Assert.That(reference.gameObject.activeSelf, Is.False);
            Assert.That(referenceCollider.shapeCount, Is.Zero);
            Assert.That(Physics2D.OverlapBox(queryPosition, Vector2.one * 2, 0), Is.Null);
            Assert.That(references.Get(identity), Is.SameAs(reference));
            Assert.That(initialized, Is.EqualTo(1));
            Assert.That(references.PublishedCount, Is.EqualTo(1));

            var independentRental = rentals.Get<ControllerItem>(identity);
            Physics2D.SyncTransforms();
            Assert.That(independentRental, Is.Not.SameAs(reference));
            Assert.That(independentRental.gameObject.activeInHierarchy, Is.True);
            Assert.That(Physics2D.OverlapBox(queryPosition, Vector2.one * 2, 0),
                Is.SameAs(independentRental.GetComponent<BoxCollider2D>()));
            rentals.Return(independentRental);
        }

        [Test]
        public void ManagedReferenceRetainsDataLifetimeAndCacheIdentity()
        {
            rentals.Initialize(_ => new ManagedItem());
            int initialized = 0;
            references.OnInitialize += _ => initialized++;
            var reference = references.Get(identity);
            Assert.That(reference.IsDestroyed, Is.False);
            Assert.That(reference.id, Is.EqualTo(identity));
            Assert.That(references.Get(identity), Is.SameAs(reference));
            Assert.That(initialized, Is.EqualTo(1));
            Assert.That(references.PublishedCount, Is.EqualTo(1));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void InitializationFailureReturnsUnpublishedRental(bool controller)
        {
            IGameItem rental = null;
            rentals.Initialize(_ => rental = controller ? CreateController() : new ManagedItem());
            var failure = new InvalidOperationException("Reference initialization failed.");
            GameItemReferenceManager.InitializeHandler initialize = _ => throw failure;
            references.OnInitialize += initialize;
            Assert.That(Assert.Throws<InvalidOperationException>(() => references.Get(identity)),
                Is.SameAs(failure));
            Assert.That(references.PublishedCount, Is.Zero);
            Assert.That(rental.IsDestroyed, Is.True);
            if (controller) Assert.That(((ControllerItem)rental).gameObject.activeSelf, Is.False);

            references.OnInitialize -= initialize;
            var completed = references.Get(identity);
            Assert.That(completed, Is.SameAs(rental));
            Assert.That(completed.IsDestroyed, Is.False);
            Assert.That(references.PublishedCount, Is.EqualTo(1));
            if (controller) Assert.That(((ControllerItem)completed).gameObject.activeSelf, Is.False);
        }

        private ControllerItem CreateController()
        {
            var root = CreateObject("Reference Controller");
            root.transform.position = queryPosition;
            root.AddComponent<Rigidbody2D>().gravityScale = 0;
            root.AddComponent<BoxCollider2D>();
            return root.AddComponent<ControllerItem>();
        }

        private GameObject CreateObject(string name)
        {
            var result = new GameObject(name);
            objects.Add(result);
            return result;
        }
    }
}
