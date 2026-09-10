using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Localization;
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

        private static UIPanelLanguageConfig Language(string localeCode, StyleSheet style)
        {
            var config = new UIPanelLanguageConfig { styleSheet = style };
            typeof(UIPanelLanguageConfig).GetField("localeCode", BindingFlags.Instance | BindingFlags.NonPublic)
                .SetValue(config, localeCode);
            return config;
        }
    }
}
