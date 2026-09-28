using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UIElements;
using VMFramework.GameLogicArchitecture;
using VMFramework.UI;

namespace VMFramework.Editor.Tests
{
    public sealed class UIPanelLanguageListTests
    {
        [Test]
        public void LanguageSwitch_ReplacesOnlyThePreviousLocaleStyle()
        {
            var globalProperty = typeof(GlobalSetting<UISetting, UISettingFile>)
                .GetProperty(nameof(UISetting.GlobalSettingFile));
            var previousGlobal = UISetting.GlobalSettingFile;
            var global = ScriptableObject.CreateInstance<UISettingFile>();
            var setting = ScriptableObject.CreateInstance<UIPanelGeneralSetting>();
            var firstStyle = ScriptableObject.CreateInstance<StyleSheet>();
            var secondStyle = ScriptableObject.CreateInstance<StyleSheet>();
            var sharedStyle = ScriptableObject.CreateInstance<StyleSheet>();
            var firstLocale = Locale.CreateLocale("en-US");
            var secondLocale = Locale.CreateLocale("zh-CN");
            var unconfiguredLocale = Locale.CreateLocale("ja-JP");
            var host = new GameObject("Language List Test");
            host.SetActive(false);
            try
            {
                global.uiPanelGeneralSetting = setting;
                globalProperty.SetValue(null, global);
                setting.languageConfigs.Add(Language("en-US", firstStyle));
                setting.languageConfigs.Add(Language("zh-CN", secondStyle));

                var panel = host.AddComponent<UIToolkitPanel>();
                var root = new VisualElement();
                root.styleSheets.Add(sharedStyle);
                typeof(UIToolkitPanel).GetProperty(nameof(UIToolkitPanel.RootVisualElement))
                    .SetValue(panel, root);
                var modifier = (ILocalizedPanelModifier)panel;

                modifier.OnCurrentLanguageChanged(firstLocale);
                Assert.That(root.styleSheets.Contains(firstStyle), Is.True);
                modifier.OnCurrentLanguageChanged(secondLocale);
                Assert.That(root.styleSheets.Contains(firstStyle), Is.False);
                Assert.That(root.styleSheets.Contains(secondStyle), Is.True);
                modifier.OnCurrentLanguageChanged(firstLocale);
                Assert.That(root.styleSheets.Contains(secondStyle), Is.False);
                Assert.That(root.styleSheets.Contains(firstStyle), Is.True);
                modifier.OnCurrentLanguageChanged(unconfiguredLocale);
                Assert.That(root.styleSheets.Contains(firstStyle), Is.False);
                Assert.That(root.styleSheets.Contains(sharedStyle), Is.True);
                Assert.That(root.styleSheets.count, Is.EqualTo(1));
            }
            finally
            {
                globalProperty.SetValue(null, previousGlobal);
                Object.DestroyImmediate(host);
                Object.DestroyImmediate(global);
                Object.DestroyImmediate(setting);
                Object.DestroyImmediate(firstStyle);
                Object.DestroyImmediate(secondStyle);
                Object.DestroyImmediate(sharedStyle);
                Object.DestroyImmediate(firstLocale);
                Object.DestroyImmediate(secondLocale);
                Object.DestroyImmediate(unconfiguredLocale);
            }
        }

        [TestCase("OnUIPanelClose")]
        [TestCase("OnUIPanelDestruct")]
        [TestCase("OnDestroy")]
        public void LanguageManager_RefreshesPanelControllerForItsOpenLifetime(string endMethod)
        {
            var globalProperty = typeof(GlobalSetting<UISetting, UISettingFile>)
                .GetProperty(nameof(UISetting.GlobalSettingFile));
            var previousGlobal = UISetting.GlobalSettingFile;
            var previousLocalization = LocalizationSettings.Instance;
            var localization = ScriptableObject.CreateInstance<LocalizationSettings>();
            var global = ScriptableObject.CreateInstance<UISettingFile>();
            var setting = ScriptableObject.CreateInstance<UIPanelGeneralSetting>();
            var firstStyle = ScriptableObject.CreateInstance<StyleSheet>();
            var secondStyle = ScriptableObject.CreateInstance<StyleSheet>();
            var firstLocale = Locale.CreateLocale("en-US");
            var secondLocale = Locale.CreateLocale("ja-JP");
            var host = new GameObject("Localized Controller Test");
            host.SetActive(false);
            LocalizedUIPanelManager manager = null;
            try
            {
                global.uiPanelGeneralSetting = setting;
                globalProperty.SetValue(null, global);
                setting.enableLanguageConfigs = true;
                setting.languageConfigs.Add(Language("en-US", firstStyle));
                setting.languageConfigs.Add(Language("ja-JP", secondStyle));
                LocalizationSettings.Instance = localization;
                var locales = new LocalesProvider();
                locales.AddLocale(firstLocale);
                locales.AddLocale(secondLocale);
                LocalizationSettings.AvailableLocales = locales;
                LocalizationSettings.SelectedLocale = firstLocale;
                LocalizationSettings.InitializationOperation.WaitForCompletion();

                var panel = host.AddComponent<UIToolkitPanel>();
                var root = new VisualElement();
                typeof(UIToolkitPanel).GetProperty(nameof(UIToolkitPanel.RootVisualElement))
                    .SetValue(panel, root);
                manager = host.AddComponent<LocalizedUIPanelManager>();

                InvokeLanguageManager(manager, "OnUIPanelOpen", panel);
                Assert.That(root.styleSheets.Contains(firstStyle), Is.True,
                    "Panel controllers must receive the opening locale without being panel modifiers.");
                LocalizationSettings.SelectedLocale = secondLocale;
                Assert.That(root.styleSheets.Contains(firstStyle), Is.False);
                Assert.That(root.styleSheets.Contains(secondStyle), Is.True);

                if (endMethod == "OnDestroy")
                    InvokeLanguageManager(manager, endMethod);
                else
                    InvokeLanguageManager(manager, endMethod, panel);
                LocalizationSettings.SelectedLocale = firstLocale;
                Assert.That(root.styleSheets.Contains(secondStyle), Is.True,
                    "Retired panel controllers must stop receiving locale changes.");

                if (endMethod == "OnUIPanelClose")
                {
                    InvokeLanguageManager(manager, "OnUIPanelOpen", panel);
                    Assert.That(root.styleSheets.Contains(firstStyle), Is.True);
                    Assert.That(root.styleSheets.count, Is.EqualTo(1));
                    LocalizationSettings.SelectedLocale = secondLocale;
                    Assert.That(root.styleSheets.Contains(secondStyle), Is.True);
                    Assert.That(root.styleSheets.count, Is.EqualTo(1));
                }
            }
            finally
            {
                if (manager != null)
                    InvokeLanguageManager(manager, "OnDestroy");
                LocalizationSettings.Instance = previousLocalization;
                globalProperty.SetValue(null, previousGlobal);
                Object.DestroyImmediate(host);
                Object.DestroyImmediate(localization);
                Object.DestroyImmediate(global);
                Object.DestroyImmediate(setting);
                Object.DestroyImmediate(firstStyle);
                Object.DestroyImmediate(secondStyle);
                Object.DestroyImmediate(firstLocale);
                Object.DestroyImmediate(secondLocale);
            }
        }

        private static void InvokeLanguageManager(LocalizedUIPanelManager manager, string method,
            params object[] arguments)
        {
            typeof(LocalizedUIPanelManager).GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic)
                .Invoke(manager, arguments);
        }

        private static UIPanelLanguageConfig Language(string localeCode, StyleSheet style)
        {
            var config = new UIPanelLanguageConfig { styleSheet = style };
            typeof(UIPanelLanguageConfig).GetField("localeCode", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(config, localeCode);
            return config;
        }
    }
}
