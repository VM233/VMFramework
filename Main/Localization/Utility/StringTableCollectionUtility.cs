#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Localization;
using UnityEngine.Localization.Tables;
using VMFramework.Core;
using VMFramework.Core.Editor;

namespace VMFramework.Localization
{
    public static class StringTableCollectionUtility
    {
        public static IEnumerable<string> GetKeys(this StringTableCollection collection)
        {
            if (collection == null)
            {
                yield break;
            }

            foreach (var row in collection.GetRowEnumerator())
            {
                var tableEntries = row.TableEntries;

                if (tableEntries.Length == 0)
                {
                    continue;
                }

                var entry = tableEntries[0];

                yield return entry.Key;
            }
        }

        public static bool TryGetStringTableCollection(TableReference reference, out StringTableCollection collection)
        {
            collection = LocalizationEditorSettings.GetStringTableCollection(reference);
            return collection != null;
        }

        public static IEnumerable<StringTableEntry> GetEntries(this StringTableCollection collection,
            TableEntryReference reference)
        {
            if (collection == null)
            {
                yield break;
            }

            if (reference.ReferenceType == TableEntryReference.Type.Empty)
            {
                yield break;
            }

            foreach (var stringTable in collection.StringTables)
            {
                var entry = stringTable.GetEntryFromReference(reference);

                if (entry != null)
                {
                    yield return entry;
                }
            }
        }

        public static bool ExitsKey(this StringTableCollection collection, string key)
        {
            if (collection == null)
            {
                return false;
            }

            if (key.IsNullOrWhiteSpace())
            {
                return false;
            }

            var table = collection.StringTables.FirstOrDefault();

            if (table == null)
            {
                return false;
            }

            return table.GetEntry(key) != null;
        }

        public static bool CreateKey(this StringTableCollection collection, string key, string defaultValue)
        {
            collection.AssertIsNotNull(nameof(collection));

            if (collection.ExitsKey(key))
            {
                UnityEngine.Debug.LogWarning($"Key : {key} already exists in table : {collection.name}");
                return false;
            }

            foreach (var stringTable in collection.StringTables)
            {
                stringTable.AddEntry(key, defaultValue);

                stringTable.SetEditorDirty();
            }

            collection.EnforceSave();

            return true;
        }
    }
}
#endif