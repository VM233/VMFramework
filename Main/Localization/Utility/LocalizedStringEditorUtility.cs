#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine.Localization;
using UnityEngine.Localization.Pseudo;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Core.Formatting;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.Localization.Tables;
using VMFramework.Core;
using VMFramework.Core.Editor;
using VMFramework.Core.Pools;

namespace VMFramework.Localization
{
    public static class LocalizedStringEditorUtility
    {
        public static string GetLocalizedStringInEditor(this LocalizedString localizedString,
            IList<object> arguments = null)
        {
            var tableReference = localizedString.TableReference;

            if (tableReference.ReferenceType == TableReference.Type.Empty)
            {
                return null;
            }

            var locale = localizedString.LocaleOverride;
            if (locale == null)
            {
                // ProjectLocale's runtime getter preloads Locales. Read the authoring field instead.
                using var settings = new SerializedObject(LocalizationEditorSettings.ActiveLocalizationSettings);
                var code = settings.FindProperty("m_ProjectLocaleIdentifier").FindPropertyRelative("m_Code").stringValue;
                locale = LocalizationEditorSettings.GetLocale(new LocaleIdentifier(code));
            }
            var collection = LocalizationEditorSettings.GetStringTableCollection(tableReference);
            var table = locale == null ? null : collection?.GetTable(locale.Identifier) as StringTable;
            return table?.GetEntryFromReference(localizedString.TableEntryReference)
                ?.GetLocalizedStringInEditor(locale, localizedString, arguments ?? localizedString.Arguments);
        }

        public static string GetLocalizedStringInEditor(this StringTableEntry entry, Locale locale,
            IVariableGroup localVariables = null, IList<object> arguments = null)
        {
            string text;
            if (entry.IsSmart && !string.IsNullOrEmpty(entry.Value))
            {
                var formatter = LocalizationSettings.StringDatabase.SmartFormatter;
                var cache = new FormatCache
                {
                    Format = formatter.Parser.ParseFormat(entry.Value, formatter.GetNotEmptyFormatterExtensionNames()),
                    Table = entry.Table,
                    LocalVariables = localVariables
                };
                try { text = formatter.FormatWithCache(ref cache, entry.Value, locale.Formatter, arguments); }
                finally { cache.Format.ReleaseToPool(); }
            }
            else
            {
                text = arguments == null || arguments.Count == 0 ? entry.Value :
                    string.Format(locale.Formatter, entry.Value, arguments as object[] ?? arguments.ToArray());
            }
            return locale is PseudoLocale pseudo && !string.IsNullOrEmpty(text) ? pseudo.GetPseudoString(text) : text;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetDefaultKey(ref LocalizedString localizedString, TableReference tableReference, string key,
            string content, bool replace, bool autoRenameKey = true)
        {
            if (tableReference.ReferenceType == TableReference.Type.Empty)
            {
                return;
            }

            localizedString ??= new LocalizedString()
            {
                WaitForCompletion = true
            };
            localizedString.TableReference = tableReference;
            localizedString.SetDefaultKey(key, content, replace, autoRenameKey);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetDefaultKey(this LocalizedString localizedString, string key, string content, bool replace,
            bool autoRenameKey)
        {
            localizedString.SetDefaultKey(localizedString.TableReference, key, content, replace, autoRenameKey);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetDefaultKey(this LocalizedString localizedString, TableReference reference, string key,
            string content, bool replace, bool autoRenameKey)
        {
            if (StringTableCollectionUtility.TryGetStringTableCollection(reference, out var stringTableCollection) ==
                false)
            {
                UnityEngine.Debug.LogWarning($"Could not find string table collection with reference: {reference}");
                return;
            }

            if (key.IsNullOrWhiteSpace())
            {
                UnityEngine.Debug.LogWarning("Key cannot be empty.");
                return;
            }

            var tableEntryReference = localizedString.TableEntryReference;

            if (tableEntryReference.ReferenceType == TableEntryReference.Type.Empty)
            {
                foreach (var stringTable in stringTableCollection.StringTables)
                {
                    var entry = stringTable.GetEntry(key);

                    if (entry != null)
                    {
                        if (entry.Value.IsNullOrWhiteSpace() || replace)
                        {
                            entry.Value = content;
                        }
                    }
                    else
                    {
                        stringTable.AddEntry(key, content);
                    }
                }

                localizedString.TableEntryReference = key;
            }
            else
            {
                foreach (var stringTable in stringTableCollection.StringTables)
                {
                    var entry = stringTable.GetEntryFromReference(tableEntryReference);

                    if (entry != null)
                    {
                        if (entry.Value.IsNullOrWhiteSpace() || replace)
                        {
                            entry.Value = content;
                        }
                    }
                    else
                    {
                        stringTable.AddEntry(key, content);
                    }
                }
                
                if (autoRenameKey)
                {
                    localizedString.RenameKey(key);
                }
            }

            stringTableCollection.StringTables.SetEditorDirty();
            stringTableCollection.SetEditorDirty();
            stringTableCollection.SharedData.SetEditorDirty();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RenameKey(this LocalizedString localizedString, string newKey)
        {
            var tableReference = localizedString.TableReference;

            if (StringTableCollectionUtility.TryGetStringTableCollection(tableReference,
                    out var stringTableCollection) == false)
            {
                return;
            }

            foreach (var stringTable in stringTableCollection.StringTables)
            {
                var entry = stringTable.GetEntryFromReference(localizedString.TableEntryReference);
                if (entry != null)
                {
                    entry.Key = newKey;
                }
            }

            stringTableCollection.StringTables.SetEditorDirty();
            stringTableCollection.SetEditorDirty();
            stringTableCollection.SharedData.SetEditorDirty();

            if (localizedString.TableEntryReference.ReferenceType is TableEntryReference.Type.Empty
                or TableEntryReference.Type.Name)
            {
                localizedString.TableEntryReference = newKey;
            }
        }
    }
}
#endif
