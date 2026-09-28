using System;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;
using VMFramework.UI;
using Object = UnityEngine.Object;

namespace VMFramework.Editor.Tests
{
    public sealed class UIBindingLifetimeTests
    {
        private GameObject host;
        private BindVisualElementsManager bindings;
        private UIPanelManager previousPanelManager;

        [SetUp]
        public void SetUp()
        {
            host = new GameObject("UI Binding Lifetime Test");
            previousPanelManager = UIPanelManager.Instance;
            host.AddComponent<UIPanelManager>();
            bindings = host.AddComponent<BindVisualElementsManager>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(host);
            UIPanelManager.Instance = previousPanelManager;
        }

        [Test]
        public void Remove_RetiresBothPublicLookupsBeforeNotifyingConsumers()
        {
            var value = new object();
            var element = new VisualElement();
            bindings.Add("entries", value, element);
            var removals = 0;
            bindings.OnBindVisualElementChanged += (name, changed, visual, added) =>
            {
                if (added) return;
                Assert.That(bindings.TryGetVisualElement(name, changed, out _), Is.False);
                Assert.That(bindings.TryGetBindObject(name, visual, out _), Is.False);
                removals++;
            };

            Assert.That(bindings.Remove("entries", value, out var removed), Is.True);
            Assert.That(removed, Is.SameAs(element));
            Assert.That(bindings.GetBindInfos("entries"), Is.Empty);
            Assert.That(bindings.Remove("entries", value), Is.False);
            Assert.That(removals, Is.EqualTo(1));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ReplacingEitherKey_RetiresTheDisplacedPair(bool replaceObject)
        {
            var firstObject = new object();
            var secondObject = replaceObject ? new object() : firstObject;
            var firstElement = new VisualElement();
            var secondElement = replaceObject ? firstElement : new VisualElement();
            bindings.Add("entries", firstObject, firstElement);
            bindings.Add("entries", secondObject, secondElement);

            Assert.That(bindings.GetBindInfos("entries").Count, Is.EqualTo(1));
            Assert.That(bindings.TryGetVisualElement("entries", secondObject, out var currentElement), Is.True);
            Assert.That(currentElement, Is.SameAs(secondElement));
            Assert.That(bindings.TryGetBindObject("entries", secondElement, out var currentObject), Is.True);
            Assert.That(currentObject, Is.SameAs(secondObject));
            if (replaceObject)
                Assert.That(bindings.TryGetVisualElement("entries", firstObject, out _), Is.False);
            else
                Assert.That(bindings.TryGetBindObject("entries", firstElement, out _), Is.False);
        }

        [Test]
        public void ReplacingTwoOccupiedKeys_RetiresBothDisplacedPairs()
        {
            var firstObject = new object();
            var secondObject = new object();
            var firstElement = new VisualElement();
            var secondElement = new VisualElement();
            bindings.Add("entries", firstObject, firstElement);
            bindings.Add("entries", secondObject, secondElement);
            bindings.Add("entries", firstObject, secondElement);

            Assert.That(bindings.GetBindInfos("entries").Count, Is.EqualTo(1));
            Assert.That(bindings.TryGetBindObject("entries", firstElement, out _), Is.False);
            Assert.That(bindings.TryGetVisualElement("entries", secondObject, out _), Is.False);
            Assert.That(bindings.Remove("entries", firstObject), Is.True);
            Assert.That(bindings.TryGetBindObject("entries", secondElement, out _), Is.False);
        }

        [Test]
        public void RepeatedGeneration_RetainsOnlyTheCurrentEntriesAndIndependentNames()
        {
            var shared = new object();
            var persistentElement = new VisualElement();
            bindings.Add("independent", shared, persistentElement);
            for (var cycle = 0; cycle < 8; cycle++)
            {
                for (var entry = 0; entry < 16; entry++)
                {
                    var element = new VisualElement();
                    bindings.Add("entries", shared, element);
                    Assert.That(bindings.Remove("entries", shared), Is.True);
                    Assert.That(bindings.TryGetBindObject("entries", element, out _), Is.False);
                }
                Assert.That(bindings.GetBindInfos("entries"), Is.Empty);
                Assert.That(bindings.TryGetBindObject("independent", persistentElement, out var value), Is.True);
                Assert.That(value, Is.SameAs(shared));
            }
        }

        [Test]
        public void Deinitialize_ReleasesBindingsBeforeTheNextPanelLifetime()
        {
            var value = new object();
            var oldElement = new VisualElement();
            bindings.Initialize(null, null);
            bindings.Add("entries", value, oldElement);
            bindings.Deinitialize();
            Assert.That(bindings.TryGetBindObject("entries", oldElement, out _), Is.False);
            Assert.That(bindings.GetBindInfos("entries"), Is.Empty);
            bindings.Initialize(null, null);
            var currentElement = new VisualElement();
            bindings.Add("entries", value, currentElement);
            Assert.That(bindings.TryGetVisualElement("entries", value, out var current), Is.True);
            Assert.That(current, Is.SameAs(currentElement));
            bindings.Deinitialize();
        }

        [TestCase(false)]
        [TestCase(true)]
        public void NativePanelDestruction_RetiresExternalSubscriptionsExactlyOnce(bool clearFirst)
        {
            var source = new LifetimeEventSource();
            for (var generation = 0; generation < 2; generation++)
            {
                var panelHost = new GameObject("Panel Native Lifetime Test");
                var panel = panelHost.AddComponent<NativeLifetimePanel>();
                typeof(ControllerGameItem).GetProperty("GamePrefab", BindingFlags.Instance | BindingFlags.NonPublic)
                    .SetValue(panel, new UIPanelConfig { id = "native_lifetime_ui", isUnique = false });
                var modifier = panelHost.AddComponent<ExternalEventModifier>();
                modifier.Source = source;
                panel.InitializeModifier(modifier);
                source.Publish();
                Assert.That(modifier.Received, Is.EqualTo(1));
                if (clearFirst) ((IPoolItem)panel).OnClear();
                Object.DestroyImmediate(panelHost);
                source.Publish();
                Assert.That(modifier.Received, Is.EqualTo(1));
                Assert.That(modifier.Deinitializations, Is.EqualTo(1));
                Assert.That(modifier.IsInitialized, Is.False);
            }
        }
    }

    public sealed class LifetimeEventSource
    {
        public event Action Changed;
        public void Publish() => Changed?.Invoke();
    }

    public sealed class ExternalEventModifier : PanelModifier
    {
        public LifetimeEventSource Source { get; set; }
        public int Received { get; private set; }
        public int Deinitializations { get; private set; }
        protected override void OnInitialize() => Source.Changed += OnChanged;
        protected override void OnDeinitialize()
        {
            Source.Changed -= OnChanged;
            Deinitializations++;
        }
        private void OnChanged() => Received++;
    }

    public sealed class NativeLifetimePanel : UIPanel
    {
        public void InitializeModifier(PanelModifier modifier)
        {
            modifiers.Add(modifier);
            modifier.Initialize(this, null);
        }
    }
}
