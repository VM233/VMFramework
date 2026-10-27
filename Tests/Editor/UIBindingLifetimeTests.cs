using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.TestTools;
using VMFramework.Core.Pools;
using VMFramework.Timers;
using VMFramework.Containers;
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
        private UIPanelManager panels;
        private GameEventManager events;

        [SetUp]
        public void SetUp()
        {
            Assert.That(ReferenceEquals(UIPanelManager.Instance, null), Is.True,
                "The standalone fixture requires no published panel manager owner.");
            Assert.That(ReferenceEquals(GameEventManager.Instance, null), Is.True,
                "The standalone fixture requires no published event manager owner.");
            host = new GameObject("UI Binding Lifetime Test");
            panels = host.AddComponent<UIPanelManager>();
            typeof(UIPanelManager).GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(panels, null);
            events = host.AddComponent<GameEventManager>();
            typeof(GameEventManager).GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(events, null);
            bindings = host.AddComponent<BindVisualElementsManager>();
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                typeof(GameEventManager).GetMethod("OnDestroy", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(events, null);
            }
            finally
            {
                try
                {
                    typeof(UIPanelManager).GetMethod("OnDestroy", BindingFlags.Instance | BindingFlags.NonPublic)
                        .Invoke(panels, null);
                }
                finally { Object.DestroyImmediate(host); }
            }
            Assert.That(ReferenceEquals(UIPanelManager.Instance, null), Is.True);
            Assert.That(ReferenceEquals(GameEventManager.Instance, null), Is.True);
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
        public void TooltipPanelRetirement_RemovesRegistrationAfterModifiersAreCleared(bool retireManagerFirst)
        {
            var tooltipManager = host.AddComponent<TooltipManager>();
            typeof(TooltipManager).GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(tooltipManager, null);
            typeof(TooltipManager).GetMethod("OnBeforeInitStart", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(tooltipManager, null);
            var panelHost = new GameObject("Tooltip Retirement Test");
            var panel = panelHost.AddComponent<UIPanel>();
            typeof(ControllerGameItem).GetProperty("GamePrefab", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(panel, new UIPanelConfig { id = "retiring_tooltip_ui", isUnique = true });
            var tooltip = panelHost.AddComponent<Tooltip>();
            ((ICollection<IPanelModifier>)panel.Modifiers).Add(tooltip);
            tooltip.Initialize(panel, null);
            typeof(UIPanelManager).GetMethod("Register", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(panels, new object[] { panel });
            try
            {
                if (retireManagerFirst)
                    typeof(TooltipManager).GetMethod("OnDestroy", BindingFlags.Instance | BindingFlags.NonPublic)
                        .Invoke(tooltipManager, null);
                typeof(UIPanel).GetMethod("OnDestroy", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(panel, null);
                Assert.That(panel.Modifiers, Is.Empty);
                var target = new object();
                Assert.DoesNotThrow(() => tooltipManager.CloseAll(target));
                Assert.DoesNotThrow(() => tooltipManager.Close("retiring_tooltip_ui", target));
                Assert.That(tooltipManager.Open("retiring_tooltip_ui", target, null), Is.False);
            }
            finally
            {
                Object.DestroyImmediate(panelHost);
                typeof(TooltipManager).GetMethod("OnDestroy", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(tooltipManager, null);
                Object.DestroyImmediate(tooltipManager);
            }
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
                if (clearFirst)
                {
                    ((IPoolItem)panel).OnClear();
                    Assert.That(panelHost == null, Is.True,
                        "Edit Mode pool clear must destroy the native panel immediately.");
                }
                else
                {
                    // An unawakened Edit Mode component receives no native OnDestroy callback.
                    typeof(UIPanel).GetMethod("OnDestroy", BindingFlags.Instance | BindingFlags.NonPublic)
                        .Invoke(panel, null);
                    Object.DestroyImmediate(panelHost);
                }
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
        public void PresentedPanels_IncludeNonuniqueAndClosingRentalsButExcludeClosedRegistrations()
        {
            var rentals = new List<UIPanel>();
            for (var i = 0; i < 3; i++)
            {
                var owner = new GameObject("Presented Panel Test");
                owner.transform.SetParent(host.transform);
                var panel = owner.AddComponent<UIPanel>();
                typeof(ControllerGameItem).GetProperty("GamePrefab", BindingFlags.Instance | BindingFlags.NonPublic)
                    .SetValue(panel, new UIPanelConfig { id = i == 0 ? "closed_unique" : "shared_rental", isUnique = i == 0 });
                typeof(UIPanelManager).GetMethod("Register", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(panels, new object[] { panel });
                rentals.Add(panel);
            }
            panels.Open(rentals[1], null);
            panels.Open(rentals[2], null);
            panels.Close(rentals[1]);
            var presented = new List<IUIPanel>();
            panels.GetPresentedPanels(presented);
            Assert.That(presented, Is.EquivalentTo(new[] { rentals[1], rentals[2] }),
                "Native input must cover every presented rental until post-close, not only unique panels.");
            typeof(UIPanel).GetMethod("OnDestroy", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(rentals[1], null);
            Object.DestroyImmediate(rentals[1].gameObject);
            presented.Clear();
            panels.GetPresentedPanels(presented);
            Assert.That(presented, Is.EquivalentTo(new[] { rentals[2] }));
        }

        [Test]
        public void ToolkitRoot_ReconstructionRebindsTheLiveRootAndRetiresOldPointerCallbacks()
        {
            var panel = CreateToolkitPanel(UIToolkitPanelCloseMode.DisableDocument, out var settings, out var tree);
            int ready = 0, released = 0, opens = 0, enters = 0;
            VisualElement target = null;
            panel.OnRootVisualElementReady += (owner, root) =>
            {
                Assert.That(root.pickingMode, Is.EqualTo(PickingMode.Ignore),
                    "A document viewport must not turn empty desktop pixels into a hit area.");
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
                    panel.RootVisualElement.Add(oldTarget);
                    SendMouseEnter(oldTarget);
                    Assert.That(enters, Is.EqualTo(generation + 1));
                    oldTarget.RemoveFromHierarchy();
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

        [UnityTest]
        public IEnumerator ToolkitRoot_DisplayNoneRetainsItsDocumentFromCreationThroughReopening()
        {
            var panel = CreateToolkitPanel(UIToolkitPanelCloseMode.VisualElementDisplayNone,
                out var settings, out var tree);
            var documentRoot = panel.UIDocument.rootVisualElement;
            var nativePanel = documentRoot.panel;
            var target = new VisualElement();
            documentRoot.Add(target);
            int ready = 0, released = 0, enters = 0, layouts = 0;
            panel.OnRootVisualElementReady += (_, _) => ready++;
            panel.OnRootVisualElementReleased += (_, _) => released++;
            panel.OnLayoutChangeEvent += _ => layouts++;
            ((IUIPanelPointerEventProvider)panel).AddPointerEvent(_ => enters++, _ => { });
            try
            {
                Assert.That(panel.IsOpened, Is.False);
                Assert.That(panel.RootVisualElement, Is.Null);
                Assert.That(panel.UIDocument.enabled, Is.True);
                Assert.That(nativePanel, Is.Not.Null);
                Assert.That(documentRoot.style.display.value, Is.EqualTo(DisplayStyle.None));
                for (int cycle = 0; cycle < 3; cycle++)
                {
                    ((IUIPanel)panel).OnOpenInternal(null);
                    Assert.That(panel.RootVisualElement, Is.SameAs(documentRoot));
                    Assert.That(panel.RootVisualElement.panel, Is.SameAs(nativePanel));
                    SendMouseEnter(target);
                    Assert.That(enters, Is.EqualTo(cycle + 1));
                    CloseToolkitPanel(panel);
                    Assert.That(panel.RootVisualElement, Is.Null);
                    Assert.That(panel.UIDocument.enabled, Is.True);
                    Assert.That(panel.UIDocument.rootVisualElement, Is.SameAs(documentRoot));
                    Assert.That(documentRoot.panel, Is.SameAs(nativePanel));
                    Assert.That(documentRoot.style.display.value, Is.EqualTo(DisplayStyle.None));
                    SendMouseEnter(target);
                    Assert.That(enters, Is.EqualTo(cycle + 1));
                }
                yield return null;
                yield return null;
                Assert.That(layouts, Is.Zero);
                Assert.That(ready, Is.EqualTo(3));
                Assert.That(released, Is.EqualTo(3));
            }
            finally
            {
                InvokeToolkitLifecycle(panel, "OnDisable");
                Object.DestroyImmediate(settings);
                Object.DestroyImmediate(tree);
            }
        }

        [UnityTest]
        public IEnumerator ContainerSlots_DisabledRootRetiresBeforeBindingRemoval() =>
            ExerciseContainerRootRetirement("disable");

        [UnityTest]
        public IEnumerator ContainerSlots_CloseAndReopenOwnSeparateContainerSubscriptions() =>
            ExerciseContainerRootRetirement("close");

        [UnityTest]
        public IEnumerator ContainerSlots_LiveReloadAdoptsTheCurrentBindingOnce() =>
            ExerciseContainerRootRetirement("live-reload");

        [UnityTest]
        public IEnumerator ContainerSlots_DeinitializeReleasesTheLiveRootSubscriptions() =>
            ExerciseContainerRootRetirement("deinitialize");

        private IEnumerator ExerciseContainerRootRetirement(string boundary)
        {
            var panel = CreateToolkitPanel(UIToolkitPanelCloseMode.VisualElementDisplayNone,
                out var settings, out var tree, includeObjects: true);
            var objects = panel.BindObjectsManager;
            const string bindName = "fixture_slots";
            ((HashSet<string>)typeof(BindObjectsManager)
                .GetField("singleModeNames", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(objects)).Add(bindName);
            var modifier = panel.gameObject.AddComponent<UIToolkitContainerModifierBase>();
            modifier.bindObjectsName = bindName;
            modifier.slotDistributorConfigs.Add(new ContainerSlotDistributorConfig
                { parentName = "Fixture Slots", removeExtraSlots = false });
            modifier.Initialize(panel, panel.Config);
            panel.OnGenerateVisualElement += (_, root) =>
                root.Add(new VisualElement { name = "Fixture Slots" });
            var firstHost = new GameObject("First Slot Container");
            firstHost.transform.SetParent(host.transform);
            var first = firstHost.AddComponent<Container>();
            var replacementHost = new GameObject("Replacement Slot Container");
            replacementHost.transform.SetParent(host.transform);
            var replacement = replacementHost.AddComponent<Container>();
            try
            {
                for (int cycle = 0; cycle < 3; cycle++)
                {
                    if (!modifier.IsInitialized) modifier.Initialize(panel, panel.Config);
                    objects.AddObject(bindName, first);
                    Assert.That(ContainerVisualCallbacks(first), Is.Zero);
                    ((IUIPanel)panel).OnOpenInternal(null);
                    yield return null;
                    yield return null;
                    Assert.That(ContainerVisualCallbacks(first), Is.EqualTo(3));
                    objects.AddObject(bindName, replacement);
                    Assert.That(ContainerVisualCallbacks(first), Is.Zero);
                    Assert.That(ContainerVisualCallbacks(replacement), Is.EqualTo(3));
                    if (boundary == "deinitialize") modifier.Deinitialize();
                    else if (boundary == "close") CloseToolkitPanel(panel);
                    else InvokeToolkitLifecycle(panel, "OnDisable");
                    Assert.That(ContainerVisualCallbacks(replacement), Is.Zero);
                    if (boundary != "deinitialize") Assert.That(panel.RootVisualElement, Is.Null);
                    Assert.DoesNotThrow(() => objects.ClearAllObjects(),
                        "Logical binding retirement must not query a retired visual root.");
                    if (boundary == "live-reload")
                    {
                        objects.AddObject(bindName, first);
                        Assert.That(ContainerVisualCallbacks(first), Is.Zero);
                        panel.UIDocument.visualTreeAsset = tree;
                        InvokeToolkitLifecycle(panel, "OnEnable");
                        yield return null;
                        yield return null;
                        Assert.That(ContainerVisualCallbacks(first), Is.EqualTo(3));
                    }
                    CloseToolkitPanel(panel);
                    Assert.That(ContainerVisualCallbacks(first), Is.Zero);
                    Assert.That(ContainerVisualCallbacks(replacement), Is.Zero);
                }
            }
            finally
            {
                if (modifier.IsInitialized) modifier.Deinitialize();
                InvokeToolkitLifecycle(panel, "OnDisable");
                Object.DestroyImmediate(settings);
                Object.DestroyImmediate(tree);
            }
        }

        private static int ContainerVisualCallbacks(Container container)
        {
            int count = 0;
            foreach (var name in new[] { "OnAfterItemChangedEvent", "OnItemDirtyEvent", "OnSizeChangedEvent" })
                count += (typeof(Container).GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetValue(container) as System.Delegate)?.GetInvocationList().Length ?? 0;
            return count;
        }

        [UnityTest]
        public IEnumerator Tracing_DisabledRootRetiresBeforeLogicalClose() =>
            ExerciseTracingRootRetirement("disable");

        [UnityTest]
        public IEnumerator SlotFilters_RetiredManagerSupportsNativeRootRetirement() =>
            ExerciseSlotFilterRetirement("disable");

        [UnityTest]
        public IEnumerator SlotFilters_RetiredManagerSupportsLogicalClose() =>
            ExerciseSlotFilterRetirement("close");

        [UnityTest]
        public IEnumerator SlotFilters_DeinitializeRetiresFiltersAndPointerCallbacks() =>
            ExerciseSlotFilterRetirement("deinitialize");

        private IEnumerator ExerciseSlotFilterRetirement(string boundary)
        {
            var panel = CreateToolkitPanel(UIToolkitPanelCloseMode.VisualElementDisplayNone,
                out var settings, out var tree, includeObjects: true);
            const string bindName = "filter_fixture_slots";
            const string filterID = "filter_fixture";
            var slots = panel.gameObject.AddComponent<UIToolkitContainerModifierBase>();
            slots.bindObjectsName = bindName;
            slots.slotDistributorConfigs.Add(new ContainerSlotDistributorConfig
                { parentName = "Filter Fixture Slots", removeExtraSlots = false });
            slots.Initialize(panel, panel.Config);
            var adder = panel.gameObject.AddComponent<GeneralContainerItemSlotFilterAdderModifierBase>();
            adder.filterID = filterID;
            var display = panel.gameObject.AddComponent<SlotFilterDisplayModifier>();
            var container = host.AddComponent<Container>();
            typeof(ControllerGameItem).GetProperty("GamePrefab", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(container, new ContainerConfig
                    { id = "slot_filter_fixture_container", prefab = host, fixedSize = 1 });
            typeof(Container).GetMethod("OnCreate", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(container, null);
            ((HashSet<string>)typeof(BindObjectsManager)
                .GetField("singleModeNames", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(panel.BindObjectsManager)).Add(bindName);
            panel.OnGenerateVisualElement += (_, root) =>
            {
                var parent = new VisualElement { name = "Filter Fixture Slots" };
                parent.Add(new SlotVisualElement());
                root.Add(parent);
            };
            SlotGlobalFiltersManager filters = null;
            try
            {
                for (int cycle = 0; cycle < 3; cycle++)
                {
                    filters = host.AddComponent<SlotGlobalFiltersManager>();
                    typeof(SlotGlobalFiltersManager).GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic)
                        .Invoke(filters, null);
                    adder.Initialize(panel, panel.Config);
                    display.Initialize(panel, panel.Config);
                    panel.BindObjectsManager.AddObject(bindName, container);
                    ((IUIPanel)panel).OnOpenInternal(null);
                    yield return null;
                    yield return null;
                    Assert.That(slots.Slots.Count, Is.EqualTo(1));
                    var slot = slots.Slots.Single();
                    var preview = panel.GenerateVisualElement();
                    Assert.That(preview.panel, Is.Null);
                    CollectionAssert.Contains(slots.Slots, slot, "Input must target the published slot.");
                    Assert.That(slot.panel, Is.Not.Null, "The input target must belong to a native panel.");
                    Assert.That(slot.enabledInHierarchy, Is.True, "The input target must be enabled.");
                    Assert.That(slots.TryGetContainerAndIndex(slot, out var boundContainer, out var boundIndex), Is.True);
                    Assert.That(boundContainer, Is.SameAs(container));
                    Assert.That(boundIndex, Is.Zero);
                    int deliveries = 0;
                    EventCallback<PointerEnterEvent> acknowledge = _ => deliveries++;
                    slot.RegisterCallback(acknowledge);
                    try
                    {
                        using var enter = PointerEnterEvent.GetPooled();
                        enter.target = slot;
                        slot.SendEvent(enter);
                        Assert.That(deliveries, Is.EqualTo(1), "Native input dispatch must acknowledge the stimulus.");
                    }
                    finally { slot.UnregisterCallback(acknowledge); }
                    Assert.That(filters.RemoveFilter(filterID, slot), Is.True,
                        "The real slot pointer callback must acquire a filter.");
                    filters.AddFilter(filterID, slot, new GeneralContainerItemSlotFilter(container, 0));
                    typeof(SlotGlobalFiltersManager).GetMethod("OnDestroy", BindingFlags.Instance | BindingFlags.NonPublic)
                        .Invoke(filters, null);
                    Object.DestroyImmediate(filters);
                    Assert.That(ReferenceEquals(SlotGlobalFiltersManager.Instance, null), Is.True);
                    if (boundary == "deinitialize")
                    {
                        adder.Deinitialize();
                        display.Deinitialize();
                        using var leave = PointerLeaveEvent.GetPooled();
                        leave.target = slot;
                        Assert.DoesNotThrow(() => slot.SendEvent(leave));
                        Assert.That(filters.RemoveFilter(filterID, slot), Is.False);
                    }
                    if (boundary == "close") CloseToolkitPanel(panel);
                    else InvokeToolkitLifecycle(panel, "OnDisable");
                    Assert.That(filters.RemoveFilter(filterID, slot), Is.False,
                        "Retirement must release the original physical manager's filter.");
                    CloseToolkitPanel(panel);
                    if (adder.IsInitialized) adder.Deinitialize();
                    if (display.IsInitialized) display.Deinitialize();
                    var callbacks = typeof(SlotGlobalFiltersManager)
                        .GetField("OnFilterChanged", BindingFlags.Instance | BindingFlags.NonPublic)
                        .GetValue(filters) as System.Delegate;
                    Assert.That(callbacks, Is.Null);
                    Assert.That(typeof(UIToolkitContainerModifierBase)
                        .GetField("OnSlotChanged", BindingFlags.Instance | BindingFlags.NonPublic)
                        .GetValue(slots), Is.Null);
                    panel.BindObjectsManager.ClearAllObjects();
                    filters = null;
                }
            }
            finally
            {
                if (adder.IsInitialized) adder.Deinitialize();
                if (display.IsInitialized) display.Deinitialize();
                slots.Deinitialize();
                InvokeToolkitLifecycle(panel, "OnDisable");
                if (!ReferenceEquals(filters, null))
                {
                    typeof(SlotGlobalFiltersManager).GetMethod("OnDestroy", BindingFlags.Instance | BindingFlags.NonPublic)
                        .Invoke(filters, null);
                    Object.DestroyImmediate(filters);
                }
                Object.DestroyImmediate(settings);
                Object.DestroyImmediate(tree);
            }
        }

        [UnityTest]
        public IEnumerator AutoRefresh_RetiredTimerSourceSupportsLogicalClose() =>
            ExerciseAutoRefreshRetirement("retired-source");

        [UnityTest]
        public IEnumerator AutoRefresh_DeinitializeRetiresTimerAndPanelSubscriptions() =>
            ExerciseAutoRefreshRetirement("deinitialize");

        [UnityTest]
        public IEnumerator AutoRefresh_ReentrantCloseAndReopenOwnSeparateTimers() =>
            ExerciseAutoRefreshRetirement("reentrant");

        public sealed class RefreshProbe : MonoBehaviour, VMFramework.Core.IRefreshable
        {
            public int Count { get; private set; }
            public System.Action Action { get; set; }
            public void Refresh() { Count++; Action?.Invoke(); }
        }

        private IEnumerator ExerciseAutoRefreshRetirement(string boundary)
        {
            var panel = CreateToolkitPanel(UIToolkitPanelCloseMode.VisualElementDisplayNone,
                out var settings, out var tree);
            var probe = panel.gameObject.AddComponent<RefreshProbe>();
            var modifier = panel.gameObject.AddComponent<AutoRefreshModifier>();
            modifier.refreshInterval = 100f;
            TimerManager source = null;
            try
            {
                for (int cycle = 0; cycle < 3; cycle++)
                {
                    source = host.AddComponent<TimerManager>();
                    typeof(TimerManager).GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic)
                        .Invoke(source, null);
                    modifier.Initialize(panel, panel.Config);
                    ((IUIPanel)panel).OnOpenInternal(null);
                    yield return null;
                    yield return null;
                    var timer = RefreshTimer(modifier);
                    Assert.That(source.Contains(timer), Is.True);
                    int before = probe.Count;
                    RunRefreshDeadline(source, timer);
                    Assert.That(probe.Count, Is.EqualTo(before + 1));
                    Assert.That(source.Contains(timer), Is.True);
                    if (boundary == "reentrant")
                        probe.Action = () =>
                        {
                            probe.Action = null;
                            CloseToolkitPanel(panel);
                            ((IUIPanel)panel).OnOpenInternal(null);
                        };
                    RunRefreshDeadline(source, timer);
                    Assert.That(probe.Count, Is.EqualTo(before + 2));
                    if (boundary == "reentrant")
                    {
                        var replacement = RefreshTimer(modifier);
                        Assert.That(replacement, Is.Not.SameAs(timer));
                        Assert.That(source.Contains(timer), Is.False);
                        Assert.That(timer.OnTimed, Is.Null);
                        Assert.That(source.Contains(replacement), Is.True);
                        timer = replacement;
                        yield return null;
                        yield return null;
                    }
                    if (boundary == "retired-source")
                    {
                        RetireRefreshSource(source);
                        Assert.That(ReferenceEquals(TimerManager.Instance, null), Is.True);
                    }
                    var queuedCallback = timer.OnTimed;
                    if (boundary == "deinitialize") modifier.Deinitialize();
                    else CloseToolkitPanel(panel);
                    Assert.That(source.Contains(timer), Is.False);
                    Assert.That(timer.OnTimed, Is.Null);
                    queuedCallback(timer);
                    Assert.That(probe.Count, Is.EqualTo(before + 2));
                    CloseToolkitPanel(panel);
                    if (modifier.IsInitialized) modifier.Deinitialize();
                    Assert.That(typeof(UIPanel).GetField("OnOpen", BindingFlags.Instance | BindingFlags.NonPublic)
                        .GetValue(panel), Is.Null);
                    Assert.That(typeof(UIPanel).GetField("OnPostClose", BindingFlags.Instance | BindingFlags.NonPublic)
                        .GetValue(panel), Is.Null);
                    if (boundary != "retired-source") RetireRefreshSource(source);
                    source = null;
                }
            }
            finally
            {
                if (modifier.IsInitialized) modifier.Deinitialize();
                CloseToolkitPanel(panel);
                InvokeToolkitLifecycle(panel, "OnDisable");
                if (!ReferenceEquals(source, null) && source != null) RetireRefreshSource(source);
                Object.DestroyImmediate(settings);
                Object.DestroyImmediate(tree);
            }
        }

        private static Timer<double> RefreshTimer(AutoRefreshModifier modifier) =>
            (Timer<double>)typeof(AutoRefreshModifier)
                .GetField("refreshTimer", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(modifier);

        private static void RunRefreshDeadline(TimerManager source, Timer<double> timer)
        {
            typeof(TimerManager).GetField("currentTime", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(source, timer.ExpectedTime);
            typeof(TimerManager).GetMethod("Update", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(source, null);
        }

        private static void RetireRefreshSource(TimerManager source)
        {
            typeof(TimerManager).GetMethod("OnDestroy", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(source, null);
            Object.DestroyImmediate(source);
        }

        [UnityTest]
        public IEnumerator Tracing_CloseAndReopenOwnSeparateRootLeases() =>
            ExerciseTracingRootRetirement("close");

        [UnityTest]
        public IEnumerator Tracing_LiveReloadAdoptsTheNewRootOnce() =>
            ExerciseTracingRootRetirement("live-reload");

        [UnityTest]
        public IEnumerator Tracing_DeinitializeReleasesTheLiveRootLease() =>
            ExerciseTracingRootRetirement("deinitialize");

        private IEnumerator ExerciseTracingRootRetirement(string boundary)
        {
            var manager = host.AddComponent<TracingUIManager>();
            typeof(TracingUIManager).GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(manager, null);
            var panel = CreateToolkitPanel(UIToolkitPanelCloseMode.VisualElementDisplayNone,
                out var settings, out var tree);
            var modifier = panel.gameObject.AddComponent<UIToolkitTracingPanelModifier>();
            modifier.containerPath.names.Add("Fixture Trace");
            modifier.resetPositionOnClose = true;
            modifier.Initialize(panel, panel.Config);
            panel.OnGenerateVisualElement += (_, root) =>
            {
                root.Clear();
                root.style.width = 800;
                root.style.height = 600;
                var container = new VisualElement { name = "Fixture Trace" };
                container.style.width = 100;
                container.style.height = 40;
                root.Add(container);
            };
            try
            {
                for (int cycle = 0; cycle < 3; cycle++)
                {
                    if (!modifier.IsInitialized) modifier.Initialize(panel, panel.Config);
                    modifier.enableAutoMouseTracing = cycle != 1;
                    ((IUIPanel)panel).OnOpenInternal(null);
                    yield return null;
                    yield return null;
                    if (!modifier.enableAutoMouseTracing) manager.StartTracing(modifier, true);
                    Assert.That(manager.TryGetConfig(modifier, out _), Is.True);
                    Assert.DoesNotThrow(() => modifier.TryUpdateScreenPosition(new Vector2(200, 100)));
                    if (boundary == "deinitialize") modifier.Deinitialize();
                    else if (boundary == "close") CloseToolkitPanel(panel);
                    else InvokeToolkitLifecycle(panel, "OnDisable");
                    Assert.That(manager.TryGetConfig(modifier, out _), Is.False);
                    Assert.That(typeof(UIToolkitTracingPanelModifier)
                        .GetField("tracingRoot", BindingFlags.Instance | BindingFlags.NonPublic)
                        .GetValue(modifier), Is.Null);
                    Assert.DoesNotThrow(() => CloseToolkitPanel(panel),
                        "Logical close must not query or reset an already retired Toolkit root.");
                    if (boundary == "live-reload")
                    {
                        ((IUIPanel)panel).OnOpenInternal(null);
                        yield return null;
                        InvokeToolkitLifecycle(panel, "OnDisable");
                        panel.UIDocument.visualTreeAsset = tree;
                        InvokeToolkitLifecycle(panel, "OnEnable");
                        yield return null;
                        yield return null;
                        if (!modifier.enableAutoMouseTracing) manager.StartTracing(modifier, true);
                        Assert.That(manager.TryGetConfig(modifier, out _), Is.True);
                        CloseToolkitPanel(panel);
                        Assert.That(manager.TryGetConfig(modifier, out _), Is.False);
                    }
                }
            }
            finally
            {
                if (modifier.IsInitialized) modifier.Deinitialize();
                InvokeToolkitLifecycle(panel, "OnDisable");
                typeof(TracingUIManager).GetMethod("OnDestroy", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(manager, null);
                Object.DestroyImmediate(settings);
                Object.DestroyImmediate(tree);
            }
        }

        private UIToolkitPanel CreateToolkitPanel(UIToolkitPanelCloseMode mode,
            out PanelSettings settings, out VisualTreeAsset tree, bool includeObjects = false)
        {
            var panelHost = new GameObject("Toolkit Root Lifetime Test");
            panelHost.transform.SetParent(host.transform);
            var panel = panelHost.AddComponent<UIToolkitPanel>();
            if (includeObjects) panelHost.AddComponent<BindObjectsManager>();
            var document = panel.GetComponent<UIDocument>();
            document.enabled = false;
            settings = ScriptableObject.CreateInstance<PanelSettings>();
            tree = ScriptableObject.CreateInstance<VisualTreeAsset>();
            document.panelSettings = settings;
            document.visualTreeAsset = tree;
            typeof(ControllerGameItem).GetProperty("GamePrefab", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(panel, new UIToolkitPanelConfig
                {
                    id = "root_lifetime_ui", isUnique = false, closeMode = mode, prefab = panelHost,
                    useDefaultPanelSettings = false, customPanelSettings = settings
                });
            InvokeToolkitLifecycle(panel, "OnCreate");
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
            evt.target = target;
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
