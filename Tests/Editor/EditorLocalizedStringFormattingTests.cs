using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Core.Formatting;
using UnityEngine.Localization.SmartFormat.Core.Settings;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.Localization.Tables;
using VMFramework.Localization;

namespace VMFramework.Editor.Tests
{
    public sealed class EditorLocalizedStringFormattingTests
    {
        private LocalizationSettings previousSettings;
        private EditorFormattingLocalizationSettings settings;
        private SharedTableData shared;
        private StringTable table;
        private Locale locale;

        [SetUp]
        public void SetUp()
        {
            previousSettings = LocalizationSettings.Instance;
            settings = ScriptableObject.CreateInstance<EditorFormattingLocalizationSettings>();
            LocalizationSettings.Instance = settings;
            shared = ScriptableObject.CreateInstance<SharedTableData>();
            shared.TableCollectionName = "Editor Formatting Test";
            table = ScriptableObject.CreateInstance<StringTable>();
            table.SharedData = shared;
        }

        [TearDown]
        public void TearDown()
        {
            LocalizationSettings.Instance = previousSettings;
            UnityEngine.Object.DestroyImmediate(table);
            UnityEngine.Object.DestroyImmediate(shared);
            if (locale != null) UnityEngine.Object.DestroyImmediate(locale);
            UnityEngine.Object.DestroyImmediate(settings);
        }

        [TestCase("en-US")]
        [TestCase("zh-CN")]
        public void TypedAndPositionalArgumentsFormatWithoutRuntimeInitializationOrSharedCacheMutation(string code)
        {
            SetLocale(code);
            var entry = table.AddEntry("Typed", "{amount:0.0} / {0}");
            entry.IsSmart = true;
            var original = new FormatCache();
            entry.FormatCache = original;
            var amount = new FloatVariable { Value = 12.5f };
            var variables = new LocalizedString { ["amount"] = amount };
            Assert.That(entry.GetLocalizedStringInEditor(locale, variables, new object[] { 7 }), Is.EqualTo("12.5 / 7"));
            amount.Value = 3.25f;
            Assert.That(entry.GetLocalizedStringInEditor(locale, variables, new object[] { 9 }), Is.EqualTo("3.3 / 9"));
            Assert.That(entry.FormatCache, Is.SameAs(original));
            Assert.That(original.LocalVariables, Is.Null);
            Assert.That(settings.InitializationRequests, Is.Zero);
            Assert.That(settings.SelectedLocaleRequests, Is.Zero);
        }

        [TestCase("en-US")]
        [TestCase("zh-CN")]
        public void LiteralEntriesUseTheExplicitLocaleAndRetainUnformattedTextWithoutArguments(string code)
        {
            SetLocale(code);
            var entry = table.AddEntry("Literal", "{0:0.00}");
            Assert.That(entry.GetLocalizedStringInEditor(locale, arguments: new object[] { 12.5f }), Is.EqualTo("12.50"));
            Assert.That(entry.GetLocalizedStringInEditor(locale), Is.EqualTo("{0:0.00}"));
            Assert.That(settings.SelectedLocaleRequests, Is.Zero);
            Assert.That(settings.InitializationRequests, Is.Zero);
        }

        [Test]
        public void MissingTypedSelectorPropagatesTheNativeFormattingFailure()
        {
            SetLocale("en-US");
            var entry = table.AddEntry("Missing", "{missing}");
            entry.IsSmart = true;
            settings.GetStringDatabase().SmartFormatter.Settings.FormatErrorAction = ErrorAction.ThrowError;
            Assert.Throws<FormattingException>(() => entry.GetLocalizedStringInEditor(locale, new LocalizedString()));
            Assert.That(entry.FormatCache, Is.Null);
            Assert.That(settings.InitializationRequests, Is.Zero);
            Assert.That(settings.SelectedLocaleRequests, Is.Zero);
        }

        [Test]
        public void OriginalImplicitSelectedLocaleEntryPointIsRejectedByTheSameProbe()
        {
            SetLocale("en-US");
            var entry = table.AddEntry("Legacy", "Literal");
            Assert.Throws<InvalidOperationException>(() => entry.GetLocalizedString());
            Assert.That(settings.SelectedLocaleRequests, Is.EqualTo(1));
            Assert.That(entry.GetLocalizedStringInEditor(locale), Is.EqualTo("Literal"));
            Assert.That(settings.SelectedLocaleRequests, Is.EqualTo(1));
            Assert.That(settings.InitializationRequests, Is.Zero);
        }

        private void SetLocale(string code)
        {
            locale = Locale.CreateLocale(code);
            table.LocaleIdentifier = locale.Identifier;
        }
    }
}
