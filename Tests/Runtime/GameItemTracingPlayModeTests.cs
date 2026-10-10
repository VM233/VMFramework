using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;
using Object = UnityEngine.Object;

namespace VMFramework.Tests
{
    public sealed class GameItemTracingPlayModeTests
    {
        private GameObject host;

        [SetUp]
        public void SetUp()
        {
            Assert.That(Application.isPlaying, Is.True);
            Assert.That(ReferenceEquals(GameItemTransformTracingManager.Instance, null), Is.True,
                "The standalone fixture requires no published transform tracing owner.");
            Assert.That(ReferenceEquals(GameItemTracingManager.Instance, null), Is.True,
                "The standalone fixture requires no published item tracing owner.");
            host = new GameObject("Tracing Lifetime Fixture");
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(host);
            Assert.That(ReferenceEquals(GameItemTransformTracingManager.Instance, null), Is.True);
            Assert.That(ReferenceEquals(GameItemTracingManager.Instance, null), Is.True);
        }

        [UnityTest]
        public IEnumerator TransformManagerDestructionAndRecreationRetireBindings()
        {
            var source = Item("Source");
            var owner = Item("Owner");
            var target = CreateObject("Target").transform;
            for (int generation = 0; generation < 2; generation++)
            {
                var manager = CreateObject("Transform Tracing Manager")
                    .AddComponent<GameItemTransformTracingManager>();
                manager.Add(source, owner, target);
                source.transform.position = new Vector3(3 + generation, 0, 0);
                yield return null;
                Assert.That(target.position.x, Is.EqualTo(3 + generation));
                Object.Destroy(manager.gameObject);
                yield return null;
                Assert.That(Callbacks(source), Is.Zero);
                Assert.That(Callbacks(owner), Is.Zero);
                Assert.That(GameItemTransformTracingManager.Instance, Is.Null);
                ((IPoolItem)owner).OnReturn();
                ((IPoolItem)owner).OnGet();
            }
        }

        [UnityTest]
        public IEnumerator WholeItemManagerDestructionAndRecreationRetireBindings()
        {
            var source = Item("Source");
            var target = Item("Target");
            for (int generation = 0; generation < 2; generation++)
            {
                var manager = CreateObject("Item Tracing Manager").AddComponent<GameItemTracingManager>();
                manager.Add(source, target, new Vector3(2, 0, 0));
                source.transform.position = new Vector3(5 + generation, 0, 0);
                yield return null;
                Assert.That(target.transform.position.x, Is.EqualTo(7 + generation));
                Object.Destroy(manager.gameObject);
                yield return null;
                Assert.That(Callbacks(source), Is.Zero);
                Assert.That(Callbacks(target), Is.Zero);
                Assert.That(GameItemTracingManager.Instance, Is.Null);
                ((IPoolItem)target).OnReturn();
                ((IPoolItem)target).OnGet();
            }
        }

        private GameObject CreateObject(string name)
        {
            var value = new GameObject(name);
            value.transform.SetParent(host.transform);
            return value;
        }

        private ControllerGameItem Item(string name) => CreateObject(name).AddComponent<ControllerGameItem>();

        private static int Callbacks(ControllerGameItem item) =>
            ((IReadOnlyCollection<IPoolEventProvider.ReturnHandler>)item.ReturnEvents).Count;
    }
}
