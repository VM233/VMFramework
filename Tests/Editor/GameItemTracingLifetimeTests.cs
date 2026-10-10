using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Tests
{
    public sealed class GameItemTracingLifetimeTests
    {
        private const BindingFlags CALLBACK = BindingFlags.Instance | BindingFlags.NonPublic;
        private GameItemTransformTracingManager previousTransforms;
        private GameItemTracingManager previousItems;
        private Scene scene;
        private GameItemTransformTracingManager transforms;
        private GameItemTracingManager items;

        [SetUp]
        public void SetUp()
        {
            previousTransforms = GameItemTransformTracingManager.Instance;
            previousItems = GameItemTracingManager.Instance;
            GameItemTransformTracingManager.Instance = null;
            GameItemTracingManager.Instance = null;
            scene = EditorSceneManager.NewPreviewScene();
            transforms = CreateObject("Transform Tracing Manager").AddComponent<GameItemTransformTracingManager>();
            items = CreateObject("Item Tracing Manager").AddComponent<GameItemTracingManager>();
            Invoke(transforms, "Awake");
            Invoke(items, "Awake");
        }

        [TearDown]
        public void TearDown()
        {
            if (scene.IsValid() && scene.isLoaded) EditorSceneManager.ClosePreviewScene(scene);
            GameItemTransformTracingManager.Instance = previousTransforms;
            GameItemTracingManager.Instance = previousItems;
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ReturningEitherRoleRetiresMotionAndBothSubscriptions(bool returnSource)
        {
            var source = Item("Source");
            var owner = Item("Owner");
            var target = CreateObject("Target").transform;
            transforms.Add(source, owner, target);
            MoveAndRender(source, 3);
            Assert.That(target.position.x, Is.EqualTo(3));

            Return(returnSource ? source : owner);
            Assert.That(Callbacks(source), Is.Zero);
            Assert.That(Callbacks(owner), Is.Zero);
            MoveAndRender(source, 7);
            Assert.That(target.position.x, Is.EqualTo(3));
            Return(returnSource ? owner : source);
        }

        [Test]
        public void ExplicitRemovalRetiresBothRolesAndIsIdempotent()
        {
            var source = Item("Source");
            var owner = Item("Owner");
            var target = CreateObject("Target").transform;
            transforms.Add(source, owner, target);
            transforms.RemoveTransform(target);
            transforms.RemoveTransform(target);
            Assert.That(Callbacks(source), Is.Zero);
            Assert.That(Callbacks(owner), Is.Zero);
            MoveAndRender(source, 9);
            Assert.That(target.position, Is.EqualTo(Vector3.zero));
        }

        [Test]
        public void RebindingReplacesTheSourceAndPreservesOnlyTheNewRelationship()
        {
            var first = Item("First Source");
            var second = Item("Second Source");
            var owner = Item("Owner");
            var target = CreateObject("Target").transform;
            transforms.Add(first, owner, target);
            transforms.Add(second, owner, target);
            Assert.That(Callbacks(first), Is.Zero);
            Assert.That(Callbacks(second), Is.EqualTo(1));
            Assert.That(Callbacks(owner), Is.EqualTo(1));
            Return(first);
            MoveAndRender(second, 6);
            Assert.That(target.position.x, Is.EqualTo(6));
            Return(owner);
            Assert.That(Callbacks(second), Is.Zero);
        }

        [Test]
        public void ReturningOneOwnerPreservesOtherBindingsOfTheSharedSource()
        {
            var source = Item("Source");
            var first = Item("First Owner");
            var second = Item("Second Owner");
            var a = CreateObject("First Target").transform;
            var b = CreateObject("Second Target").transform;
            transforms.Add(source, first, a);
            transforms.Add(source, second, b);
            Return(first);
            MoveAndRender(source, 8);
            Assert.That(a.position.x, Is.Zero);
            Assert.That(b.position.x, Is.EqualTo(8));
            Assert.That(Callbacks(source), Is.EqualTo(1));
            Return(second);
            Assert.That(Callbacks(source), Is.Zero);
        }

        [Test]
        public void ReturningOneSourcePreservesOtherBindingsOfTheSharedOwner()
        {
            var first = Item("First Source");
            var second = Item("Second Source");
            var owner = Item("Owner");
            var a = CreateObject("First Target").transform;
            var b = CreateObject("Second Target").transform;
            transforms.Add(first, owner, a);
            transforms.Add(second, owner, b);
            Return(first);
            MoveAndRender(second, 4);
            Assert.That(a.position.x, Is.Zero);
            Assert.That(b.position.x, Is.EqualTo(4));
            Assert.That(Callbacks(owner), Is.EqualTo(1));
            Return(second);
            Assert.That(Callbacks(owner), Is.Zero);
        }

        [Test]
        public void OneItemMayOwnAndSupplyItsBinding()
        {
            var item = Item("Both Roles");
            var target = CreateObject("Target").transform;
            transforms.Add(item, item, target);
            Assert.That(Callbacks(item), Is.EqualTo(2));
            MoveAndRender(item, 5);
            Assert.That(target.position.x, Is.EqualTo(5));
            Return(item);
            Assert.That(Callbacks(item), Is.Zero);
            ((IPoolItem)item).OnGet();
            MoveAndRender(item, 11);
            Assert.That(target.position.x, Is.EqualTo(5));
        }

        [Test]
        public void RepeatedRentalsDoNotRetainBindingsOrDuplicateSubscriptions()
        {
            var source = Item("Source");
            var owner = Item("Owner");
            var target = CreateObject("Reused Target").transform;
            for (int cycle = 0; cycle < 8; cycle++)
            {
                ((IPoolItem)owner).OnGet();
                transforms.Add(source, owner, target);
                Assert.That(Callbacks(source), Is.EqualTo(1));
                Assert.That(Callbacks(owner), Is.EqualTo(1));
                MoveAndRender(source, cycle);
                Assert.That(target.position.x, Is.EqualTo(cycle));
                Return(owner);
                Assert.That(Callbacks(source), Is.Zero);
                Assert.That(Callbacks(owner), Is.Zero);
            }
        }

        [Test]
        public void RemovingTheLastWholeItemTargetDetachesItsSource()
        {
            var source = Item("Source");
            var target = Item("Target");
            items.Add(source, target, new Vector3(2, 0, 0));
            source.transform.position = new Vector3(3, 0, 0);
            Invoke(items, "Update");
            Assert.That(target.transform.position.x, Is.EqualTo(5));
            items.RemoveTarget(target);
            Assert.That(Callbacks(source), Is.Zero);
            Assert.That(Callbacks(target), Is.Zero);
            source.transform.position = new Vector3(7, 0, 0);
            Invoke(items, "Update");
            Assert.That(target.transform.position.x, Is.EqualTo(5));
        }

        [Test]
        public void WholeItemRebindingRetiresItsPreviousSource()
        {
            var first = Item("First Source");
            var second = Item("Second Source");
            var target = Item("Target");
            items.Add(first, target, Vector3.zero);
            items.Add(second, target, Vector3.zero);
            Assert.That(Callbacks(first), Is.Zero);
            Return(first);
            second.transform.position = new Vector3(9, 0, 0);
            Invoke(items, "Update");
            Assert.That(target.transform.position.x, Is.EqualTo(9));
            Return(target);
            Assert.That(Callbacks(second), Is.Zero);
        }

        private GameObject CreateObject(string name)
        {
            var value = new GameObject(name);
            SceneManager.MoveGameObjectToScene(value, scene);
            return value;
        }

        private ControllerGameItem Item(string name) => CreateObject(name).AddComponent<ControllerGameItem>();

        private static void Return(ControllerGameItem item) => ((IPoolItem)item).OnReturn();

        private static int Callbacks(ControllerGameItem item) =>
            ((IReadOnlyCollection<IPoolEventProvider.ReturnHandler>)item.ReturnEvents).Count;

        private void MoveAndRender(ControllerGameItem source, int x)
        {
            source.transform.position = new Vector3(x, 0, 0);
            Invoke(transforms, "Update");
        }

        private static void Invoke(MonoBehaviour target, string method) =>
            target.GetType().GetMethod(method, CALLBACK).Invoke(target, null);
    }
}
