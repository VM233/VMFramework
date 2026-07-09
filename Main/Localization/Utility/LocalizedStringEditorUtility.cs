#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.Localization;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using VMFramework.Core;
using VMFramework.Core.Editor;
using VMFramework.Core.Pools;

namespace VMFramework.Localization
{
    public static class LocalizedStringEditorUtility
    {
        public static string GetLocalizedStringInEditor(this LocalizedString localizedString)
        {
            StringTable table;
            var tableReference = localizedString.TableReference;

            if (tableReference.ReferenceType == TableReference.Type.Empty)
            {
                return null;
            }

            if (LocalizationSettings.ProjectLocale == null)
            {
                var collection = LocalizationEditorSettings.GetStringTableCollection(tableReference);

                if (collection == null)
                {
                    table = null;
                }
                else
                {
                    table = collection.StringTables.First();
                }
            }
            else
            {
                table = LocalizationSettings.StringDatabase.GetTable(tableReference,
                    LocalizationSettings.ProjectLocale);
            }

            if (table == null)
            {
                return null;
            }

            StringTableEntry entry = null;
            var key = localizedString.TableEntryReference.Key;

            if (string.IsNullOrEmpty(key) == false)
            {
                entry = table.GetEntry(key);
            }

            if (entry == null)
            {
                var keyID = localizedString.TableEntryReference.KeyId;
                entry = table.GetEntry(keyID);
            }

            return entry?.GetLocalizedString();
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