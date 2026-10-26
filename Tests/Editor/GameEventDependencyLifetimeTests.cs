using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using VMFramework.GameEvents;
using VMFramework.GameLogicArchitecture;
using VMFramework.UI;
using Object = UnityEngine.Object;

namespace VMFramework.Editor.Tests
{
    public sealed class GameEventDependencyLifetimeTests
    {
        private const BindingFlags InstancePrivate = BindingFlags.Instance | BindingFlags.NonPublic;

        [TestCase(false, false)]
        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(true, true)]
        public void PanelTokenRelease_AfterSettingsRetirement_UsesTheAdoptedDependencyProduct(
            bool retireManagerFirst, bool deinitialize)
        {
            Assert.That(ReferenceEquals(GameEventManager.Instance, null), Is.True);
            var globalProperty = typeof(GlobalSetting<CoreSetting, CoreSettingFile>)
                .GetProperty(nameof(CoreSetting.GlobalSettingFile));
            var previousGlobal = CoreSetting.GlobalSettingFile;
            var global = ScriptableObject.CreateInstance<CoreSettingFile>();
            var setting = ScriptableObject.CreateInstance<GameEventGeneralSetting>();
            var host = new GameObject("Event Dependency Lifetime Test");
            host.SetActive(false);
            var manager = host.AddComponent<GameEventManager>();
            var panel = host.AddComponent<UIPanel>();
            var modifier = host.AddComponent<EventsDisabledOnOpenModifier>();
            var managerRetired = false;
            try
            {
                var parent = CreateEvent("dependency_parent");
                var child = CreateEvent("dependency_child");
                setting.directDependencies = new Dictionary<string, HashSet<string>>
                {
                    [parent.id] = new() { child.id }
                };
                global.gameEventGeneralSetting = setting;
                globalProperty.SetValue(null, global);
                InvokeManager(manager, "Awake");
                typeof(GameEventManager).GetMethod("AdoptDependencies", InstancePrivate)
                    .Invoke(manager, new object[] { CoreSetting.GameEventGeneralSetting.directDependencies });
                manager.Register(parent);
                manager.Register(child);
                typeof(ControllerGameItem).GetProperty("GamePrefab", InstancePrivate)
                    .SetValue(panel, new UIPanelConfig { id = "dependency_lifetime_ui" });
                modifier.gameEventDisabledOnOpen.Add(parent.id);
                modifier.Initialize(panel, null);
                ((IUIPanel)panel).OnOpenInternal(null);
                Assert.That(parent.IsEnabled.GetValue(), Is.False);
                Assert.That(child.IsEnabled.GetValue(), Is.False,
                    "The actual parent token must propagate through the authored dependency.");

                globalProperty.SetValue(null, null);
                Object.DestroyImmediate(global);
                Object.DestroyImmediate(setting);
                Assert.That(CoreSetting.GameEventGeneralSetting, Is.Null);
                if (retireManagerFirst)
                {
                    InvokeManager(manager, "OnDestroy");
                    managerRetired = true;
                }

                if (deinitialize) modifier.Deinitialize();
                else Close(panel);

                Assert.That(parent.IsEnabled.GetValue(), Is.True,
                    "The panel must release its original token after the global provider retires.");
                if (!retireManagerFirst)
                    Assert.That(child.IsEnabled.GetValue(), Is.True,
                        "A still-live event manager must propagate the release using its adopted product.");
                if (modifier.IsInitialized) modifier.Deinitialize();
                Close(panel);
            }
            finally
            {
                try
                {
                    if (modifier.IsInitialized) modifier.Deinitialize();
                    if (!managerRetired) InvokeManager(manager, "OnDestroy");
                }
                finally
                {
                    globalProperty.SetValue(null, previousGlobal);
                    Object.DestroyImmediate(host);
                    if (global != null) Object.DestroyImmediate(global);
                    if (setting != null) Object.DestroyImmediate(setting);
                }
            }
            Assert.That(ReferenceEquals(GameEventManager.Instance, null), Is.True);
        }

        private static void Close(UIPanel panel)
        {
            ((IUIPanel)panel).OnPreCloseInternal();
            ((IUIPanel)panel).OnPostCloseInternal();
        }

        private static void InvokeManager(GameEventManager manager, string method) =>
            typeof(GameEventManager).GetMethod(method, InstancePrivate).Invoke(manager, null);

        private static ParameterlessGameEvent CreateEvent(string id)
        {
            var gameEvent = new ParameterlessGameEvent();
            typeof(GameItem).GetProperty("GamePrefab", InstancePrivate)
                .SetValue(gameEvent, new GameEventConfig { id = id });
            typeof(ParameterlessGameEvent).GetMethod("OnCreate", InstancePrivate).Invoke(gameEvent, null);
            return gameEvent;
        }
    }
}
