using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.TestTools;
using VMFramework.Core.Pools;
using VMFramework.GameEvents;
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
        private GameEventManager previousEventManager;
        private GameEventManager events;

        [SetUp]
        public void SetUp()
        {
            host = new GameObject("UI Binding Lifetime Test");
            previousPanelManager = UIPanelManager.Instance;
            UIPanelManager.Instance = host.AddComponent<UIPanelManager>();
            previousEventManager = GameEventManager.Instance;
            events = host.AddComponent<GameEventManager>();
            GameEventManager.Instance = events;
            bindings = host.AddComponent<BindVisualElementsManager>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(host);
            UIPanelManager.Instance = previousPanelManager;
            GameEventManager.Instance = previousEventManager;
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
        public void PanelRetirement_AfterEventRecycling_ReleasesExternalSubscriptionsExactlyOnce(bool clearFirst)
        {
            var source = new LifetimeEventSource();
            for (var generation = 0; generation < 2; generation++)
            {
                var panelHost = new GameObject("Panel Native Lifetime Test");
                var panel = panelHost.AddComponent<UIPanel>();
                typeof(ControllerGameItem).GetProperty("GamePrefab", BindingFlags.Instance | BindingFlags.NonPublic)
                    .SetValue(panel, new UIPanelConfig { id = "native_lifetime_ui", isUnique = false });
                var modifier = new ExternalEventModifier(panelHost, source);
                ((ICollection<IPanelModifier>)panel.Modifiers).Add(modifier);
                var gameEvent = CreateEvent("native_retirement_event");
                events.Register(gameEvent);
                var close = panelHost.AddComponent<UICloseOnEventTriggeredModifier>();
                close.uiCloseGameEventIDs.Add(gameEvent.id);
                ((ICollection<IPanelModifier>)panel.Modifiers).Add(close);
                close.Initialize(panel, null);
                modifier.Initialize(panel, null);
                source.Publish();
                Assert.That(modifier.Received, Is.EqualTo(1));
                events.Unregister(gameEvent);
                gameEvent.Reset();
                if (clearFirst) ((IPoolItem)panel).OnClear();
                typeof(UIPanel).GetMethod("OnDestroy", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(panel, null);
                Object.DestroyImmediate(panelHost);
                source.Publish();
                Assert.That(modifier.Received, Is.EqualTo(1));
                Assert.That(modifier.Deinitializations, Is.EqualTo(1));
                Assert.That(modifier.IsInitialized, Is.False);
            }
        }

        [TestCase(typeof(UICloseOnEventTriggeredModifier), "uiCloseGameEventIDs", false)]
        [TestCase(typeof(UICloseOnEventTriggeredModifier), "uiCloseGameEventIDs", true)]
        [TestCase(typeof(UIToggleOnEventTriggeredModifier), "uiToggleGameEventIDs", false)]
        [TestCase(typeof(UIToggleOnEventTriggeredModifier), "uiToggleGameEventIDs", true)]
        public void EventRetirement_RetiresTheOriginalEventAfterRegistryRemovalOrReplacement(
            System.Type modifierType, string configurationField, bool replace)
        {
            var panel = host.AddComponent<UIPanel>();
            var original = CreateEvent("retirement_identity_event");
            events.Register(original);
            var modifier = (PanelModifier)host.AddComponent(modifierType);
            ((List<string>)modifierType.GetField(configurationField).GetValue(modifier)).Add(original.id);
            modifier.Initialize(panel, null);
            Assert.That(CallbackCount(original), Is.EqualTo(1));
            events.Unregister(original);
            var replacement = CreateEvent(original.id);
            System.Action independentCallback = () => { };
            replacement.AddCallback(independentCallback, 0);
            if (replace) events.Register(replacement);
            modifier.Deinitialize();
            Assert.That(CallbackCount(original), Is.Zero,
                "Retirement must release the acquired event even after its ID leaves the registry.");
            Assert.That(CallbackCount(replacement), Is.EqualTo(1),
                "The current registry entry belongs to a different subscription lifetime.");
            if (replace) events.Unregister(replacement);
        }

        [Test]
        public void ToolkitRoot_ReconstructionRebindsTheLiveRootAndRetiresOldPointerCallbacks()
        {
            var panel = CreateToolkitPanel(UIToolkitPanelCloseMode.DisableDocument, out var settings, out var tree);
            int ready = 0, released = 0, opens = 0, enters = 0;
            VisualElement target = null;
            panel.OnRootVisualElementReady += (owner, root) =>
            {
                Assert.That(root, Is.SameAs(owner.UIDocument.rootVisualElement));
                Assert.That(owner.RootVisualElement, Is.SameAs(root));
                target = new VisualElement();
                root.Add(target);
                ready++;
            };
            panel.OnRootVisualElementReleased += (owner, root) =>
            {
                Assert.That(owner.RootVisualElement, Is.Null);
                released++;
            };
            panel.OnOpen += _ => opens++;
            ((IUIPanelPointerEventProvider)panel).AddPointerEvent(_ => enters++, _ => { });
            try
            {
                ((IUIPanel)panel).OnOpenInternal(null);
                SendMouseEnter(target);
                Assert.That(enters, Is.EqualTo(1));
                for (int generation = 0; generation < 3; generation++)
                {
                    var oldRoot = panel.RootVisualElement;
                    var oldTarget = target;
                    InvokeToolkitLifecycle(panel, "OnDisable");
                    panel.UIDocument.visualTreeAsset = tree;
                    Assert.That(oldRoot.panel, Is.Null);
                    Assert.That(panel.UIDocument.rootVisualElement, Is.Not.SameAs(oldRoot));
                    InvokeToolkitLifecycle(panel, "OnEnable");
                    SendMouseEnter(oldTarget);
                    Assert.That(enters, Is.EqualTo(generation + 1));
                    SendMouseEnter(target);
                    Assert.That(enters, Is.EqualTo(generation + 2));
                    Assert.That(panel.IsOpened, Is.True);
                }
                Assert.That(ready, Is.EqualTo(4));
                Assert.That(released, Is.EqualTo(3));
                Assert.That(opens, Is.EqualTo(1));
                CloseToolkitPanel(panel);
                Assert.That(released, Is.EqualTo(4));
            }
            finally
            {
                InvokeToolkitLifecycle(panel, "OnDisable");
                Object.DestroyImmediate(settings);
                Object.DestroyImmediate(tree);
            }
        }

        [Test]
        public void ToolkitRoot_DetachedAuthoringPreviewDoesNotReplaceOrRepublishTheRuntimeRoot()
        {
            var panel = CreateToolkitPanel(UIToolkitPanelCloseMode.DisableDocument, out var settings, out var tree);
            int ready = 0, released = 0;
            panel.OnRootVisualElementReady += (_, _) => ready++;
            panel.OnRootVisualElementReleased += (_, _) => released++;
            try
            {
                ((IUIPanel)panel).OnOpenInternal(null);
                var root = panel.RootVisualElement;
                var preview = panel.GenerateVisualElement();
                Assert.That(preview.panel, Is.Null);
                Assert.That(preview, Is.Not.SameAs(root));
                Assert.That(panel.RootVisualElement, Is.SameAs(root));
                Assert.That(ready, Is.EqualTo(1));
                Assert.That(released, Is.Zero);
                CloseToolkitPanel(panel);
                ((IUIPanel)panel).OnOpenInternal(null);
                Assert.That(ready, Is.EqualTo(2));
                Assert.That(released, Is.EqualTo(1));
                CloseToolkitPanel(panel);
            }
            finally
            {
                InvokeToolkitLifecycle(panel, "OnDisable");
                Object.DestroyImmediate(settings);
                Object.DestroyImmediate(tree);
            }
        }

        [TestCase(UIToolkitPanelCloseMode.DisableDocument)]
        [TestCase(UIToolkitPanelCloseMode.VisualElementDisplayNone)]
        public void ToolkitRoot_ClosedDocumentKeepsItsConfiguredVisibilityAfterReconstruction(UIToolkitPanelCloseMode mode)
        {
            var panel = CreateToolkitPanel(mode, out var settings, out var tree);
            int ready = 0, released = 0;
            panel.OnRootVisualElementReady += (_, _) => ready++;
            panel.OnRootVisualElementReleased += (_, _) => released++;
            try
            {
                ((IUIPanel)panel).OnOpenInternal(null);
                CloseToolkitPanel(panel);
                InvokeToolkitLifecycle(panel, "OnDisable");
                panel.UIDocument.visualTreeAsset = tree;
                InvokeToolkitLifecycle(panel, "OnEnable");
                Assert.That(panel.RootVisualElement, Is.Null);
                Assert.That(panel.IsOpened, Is.False);
                Assert.That(ready, Is.EqualTo(1));
                Assert.That(released, Is.EqualTo(1));
                if (mode == UIToolkitPanelCloseMode.DisableDocument)
                    Assert.That(panel.UIDocument.enabled, Is.False);
                else
                    Assert.That(panel.UIDocument.rootVisualElement.style.display.value, Is.EqualTo(DisplayStyle.None));
            }
            finally
            {
                InvokeToolkitLifecycle(panel, "OnDisable");
                Object.DestroyImmediate(settings);
                Object.DestroyImmediate(tree);
            }
        }

        [UnityTest]
        public IEnumerator ToolkitRoot_CloseCancelsThePendingLayoutPublication()
        {
            var panel = CreateToolkitPanel(UIToolkitPanelCloseMode.DisableDocument, out var settings, out var tree);
            int layouts = 0;
            panel.OnLayoutChangeEvent += _ => layouts++;
            try
            {
                ((IUIPanel)panel).OnOpenInternal(null);
                CloseToolkitPanel(panel);
                yield return null;
                yield return null;
                Assert.That(layouts, Is.Zero);
                Assert.That(panel.RootVisualElement, Is.Null);
            }
            finally
            {
                InvokeToolkitLifecycle(panel, "OnDisable");
                Object.DestroyImmediate(settings);
                Object.DestroyImmediate(tree);
            }
        }

        private UIToolkitPanel CreateToolkitPanel(UIToolkitPanelCloseMode mode,
            out PanelSettings settings, out VisualTreeAsset tree)
        {
            var panelHost = new GameObject("Toolkit Root Lifetime Test");
            panelHost.transform.SetParent(host.transform);
            var panel = panelHost.AddComponent<UIToolkitPanel>();
            var document = panel.GetComponent<UIDocument>();
            document.enabled = false;
            settings = ScriptableObject.CreateInstance<PanelSettings>();
            tree = ScriptableObject.CreateInstance<VisualTreeAsset>();
            document.panelSettings = settings;
            document.visualTreeAsset = tree;
            typeof(ControllerGameItem).GetProperty("GamePrefab", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(panel, new UIToolkitPanelConfig
                {
                    id = "root_lifetime_ui", isUnique = false, closeMode = mode,
                    useDefaultPanelSettings = false, customPanelSettings = settings
                });
            typeof(UIToolkitPanel).GetProperty(nameof(UIToolkitPanel.UIDocument)).SetValue(panel, document);
            return panel;
        }

        private static void CloseToolkitPanel(UIToolkitPanel panel)
        {
            ((IUIPanel)panel).OnPreCloseInternal();
            ((IUIPanel)panel).OnPostCloseInternal();
        }

        private static void InvokeToolkitLifecycle(UIToolkitPanel panel, string method) =>
            typeof(UIToolkitPanel).GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(panel, null);

        private static void SendMouseEnter(VisualElement target)
        {
            using var evt = MouseEnterEvent.GetPooled();
            target.SendEvent(evt);
        }

        private static ParameterlessGameEvent CreateEvent(string id)
        {
            var gameEvent = new ParameterlessGameEvent();
            typeof(GameItem).GetProperty("GamePrefab", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(gameEvent, new GameEventConfig { id = id });
            typeof(ParameterlessGameEvent).GetMethod("OnCreate", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(gameEvent, null);
            return gameEvent;
        }

        private static int CallbackCount(ParameterlessGameEvent gameEvent)
        {
            return ((PriorityEvents<System.Action>)typeof(ParameterlessGameEvent)
                .GetField("callbacks", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(gameEvent)).Count;
        }
    }

}
