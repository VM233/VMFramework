#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using VMFramework.Core;
using Object = UnityEngine.Object;
using UnityEditor.Localization;
using UnityEngine.Localization.Tables;

namespace VMFramework.Editor.BatchProcessor
{
    public sealed class ReplaceSerializedFieldWordsUnit : SingleButtonBatchProcessorUnit
    {
        [HideDuplicateReferenceBox]
        [HideReferenceObjectPicker]
        [Serializable]
        public sealed class WordsReplacement
        {
            public string oldWords;

            public bool objectNameAsNewWords = true;

            [HideIf(nameof(objectNameAsNewWords))]
            public string newWords;

            public IReadOnlyList<string> OldWordsList => oldWords.GetWords();

            public bool IsValid => oldWords.IsNullOrWhiteSpace() == false;
        }

        protected override string ProcessButtonName => "Replace Serialized Words";

        [ListDrawerSettings(DefaultExpandedState = true, ShowPaging = false)]
        public List<WordsReplacement> replacements = new()
        {
            new()
        };

        public bool includeChildren = true;

        public bool replacePartialMatches = true;

        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            return selectedObjects.Any(obj => obj is Object);
        }

        protected override IEnumerable<object> OnProcess(IReadOnlyList<object> selectedObjects)
        {
            var validReplacements = replacements.Where(r => r != null && r.IsValid).ToList();
            if (validReplacements.Count == 0)
            {
                foreach (var selectedObject in selectedObjects)
                {
                    yield return selectedObject;
                }

                yield break;
            }

            var processedObjects = new HashSet<Object>();

            foreach (var selectedObject in selectedObjects)
            {
                foreach (var target in GetTargets(selectedObject))
                {
                    if (target == null || processedObjects.Add(target) == false)
                    {
                        continue;
                    }

                    ApplyReplacement(target, validReplacements);
                }

                yield return selectedObject;
            }
        }

        private IEnumerable<Object> GetTargets(object selectedObject)
        {
            switch (selectedObject)
            {
                case GameObject gameObject:
                {
                    yield return gameObject;

                    var components = includeChildren
                        ? gameObject.GetComponentsInChildren<Component>(true)
                        : gameObject.GetComponents<Component>();

                    foreach (var component in components)
                    {
                        if (component != null)
                        {
                            yield return component;
                        }
                    }

                    break;
                }
                case Component component:
                    yield return component;
                    break;
                case Object unityObject:
                    yield return unityObject;
                    break;
            }
        }

        private void ApplyReplacement(Object target, IReadOnlyList<WordsReplacement> validReplacements)
        {
            Undo.RegisterCompleteObjectUndo(target, ProcessButtonName);

            var serializedObject = new SerializedObject(target);
            serializedObject.UpdateIfRequiredOrScript();

            bool changed = ReplaceInSerializedObject(serializedObject, validReplacements);

            if (changed)
            {
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
                PrefabUtility.RecordPrefabInstancePropertyModifications(target);
            }
        }

        private bool ReplaceInSerializedObject(SerializedObject serializedObject,
            IReadOnlyList<WordsReplacement> validReplacements)
        {
            bool changed = false;

            var iterator = serializedObject.GetIterator();

            while (iterator.Next(true))
            {
                if (TryHandleLocalizedString(iterator, validReplacements, out var isLocalizedProperty))
                {
                    changed = true;
                    continue;
                }

                if (isLocalizedProperty)
                {
                    continue;
                }

                if (iterator.propertyType != SerializedPropertyType.String)
                {
                    continue;
                }

                if (iterator.propertyPath == "m_Name")
                {
                    continue;
                }

                var newValue = ReplaceWords(serializedObject, iterator.stringValue, validReplacements);

                if (string.Equals(iterator.stringValue, newValue, StringComparison.Ordinal))
                {
                    continue;
                }

                iterator.stringValue = newValue;
                changed = true;
            }

            return changed;
        }

        private string ReplaceWords(SerializedObject serializedObject, string input,
            IReadOnlyList<WordsReplacement> validReplacements)
        {
            if (input.IsNullOrEmpty())
            {
                return input;
            }

            string result = input;

            foreach (var replacement in validReplacements)
            {
                result = ReplaceWords(serializedObject, result, replacement);
            }

            return result;
        }

        private string ReplaceWords(SerializedObject serializedObject, string input, WordsReplacement replacement)
        {
            var sourceWords = replacement.OldWordsList;
            if (sourceWords.Count == 0)
            {
                return input;
            }

            var targetWords = replacement.objectNameAsNewWords
                ? serializedObject.targetObject.name.GetWords()
                : replacement.newWords.GetWords();

            var segments = input.GetWordSegments();
            if (segments.Count == 0)
            {
                return input;
            }

            var matchIndices = FindMatchIndices(segments, sourceWords);
            if (matchIndices.Count == 0)
            {
                return replacePartialMatches
                    ? ReplacePartial(input, replacement.oldWords, replacement.newWords)
                    : input;
            }

            var builder = new StringBuilder();

            int currentCharIndex = 0;

            foreach (var matchStart in matchIndices)
            {
                var startSegment = segments[matchStart];
                var endSegment = segments[matchStart + sourceWords.Count - 1];

                int replaceStartIndex = startSegment.StartIndex;
                int replaceEndIndex = endSegment.EndIndex;

                builder.Append(input, currentCharIndex, replaceStartIndex - currentCharIndex);

                builder.Append(BuildReplacement(segments, matchStart, sourceWords.Count, targetWords, input));

                currentCharIndex = replaceEndIndex;
            }

            builder.Append(input, currentCharIndex, input.Length - currentCharIndex);

            return builder.ToString();
        }

        private string ReplacePartial(string input, string oldValue, string newValue)
        {
            if (oldValue.IsNullOrEmpty())
            {
                return input;
            }

            int currentIndex = 0;
            var builder = new StringBuilder();

            while (true)
            {
                int foundIndex = input.IndexOf(oldValue, currentIndex, StringComparison.OrdinalIgnoreCase);
                if (foundIndex < 0)
                {
                    break;
                }

                builder.Append(input, currentIndex, foundIndex - currentIndex);
                builder.Append(newValue);
                currentIndex = foundIndex + oldValue.Length;
            }

            if (builder.Length == 0)
            {
                return input;
            }

            builder.Append(input, currentIndex, input.Length - currentIndex);

            return builder.ToString();
        }

        private static List<int> FindMatchIndices(IReadOnlyList<WordSegment> segments,
            IReadOnlyList<string> sourceWords)
        {
            var indices = new List<int>();

            for (int i = 0; i <= segments.Count - sourceWords.Count; i++)
            {
                bool matched = true;
                for (int j = 0; j < sourceWords.Count; j++)
                {
                    if (string.Equals(segments[i + j].Word, sourceWords[j], StringComparison.OrdinalIgnoreCase) ==
                        false)
                    {
                        matched = false;
                        break;
                    }
                }

                if (matched)
                {
                    indices.Add(i);
                }
            }

            return indices;
        }

        private static string BuildReplacement(IReadOnlyList<WordSegment> segments, int startIndex, int sourceCount,
            IReadOnlyList<string> targetWords, string sourceString)
        {
            if (targetWords.Count == 0)
            {
                return string.Empty;
            }

            var separators = new List<string>(Math.Max(0, sourceCount - 1));

            for (int i = 0; i < sourceCount - 1; i++)
            {
                int currentEndIndex = segments[startIndex + i].EndIndex;
                int nextStartIndex = segments[startIndex + i + 1].StartIndex;

                separators.Add(sourceString.Substring(currentEndIndex, nextStartIndex - currentEndIndex));
            }

            string fallbackSeparator = separators.LastOrDefault() ?? string.Empty;

            var builder = new StringBuilder();

            for (int i = 0; i < targetWords.Count; i++)
            {
                if (i > 0)
                {
                    string separator = i - 1 < separators.Count ? separators[i - 1] : fallbackSeparator;
                    builder.Append(separator);
                }

                builder.Append(targetWords[i]);
            }

            return builder.ToString();
        }

        private bool TryHandleLocalizedString(SerializedProperty property,
            IReadOnlyList<WordsReplacement> validReplacements, out bool isLocalizedProperty)
        {
            bool isKey = property.propertyPath.EndsWith("m_TableEntryReference.m_Key", StringComparison.Ordinal);
            bool isKeyId = property.propertyPath.EndsWith("m_TableEntryReference.m_KeyId", StringComparison.Ordinal);

            isLocalizedProperty = isKey || isKeyId;

            if (isLocalizedProperty == false)
            {
                return false;
            }

            var sharedData = GetSharedTableData(property, isKey);
            if (sharedData == null)
            {
                return false;
            }

            string originalKey = null;

            if (isKey)
            {
                originalKey = property.stringValue;
            }
            else if (property.propertyType == SerializedPropertyType.Integer)
            {
                var entryById = sharedData.GetEntry(property.longValue);
                originalKey = entryById?.Key;
            }

            if (originalKey.IsNullOrEmpty())
            {
                return false;
            }

            var replacedValue = ReplaceWords(property.serializedObject, originalKey, validReplacements);

            if (string.Equals(originalKey, replacedValue, StringComparison.Ordinal))
            {
                return false;
            }

            var targetEntry = sharedData.GetEntry(replacedValue);
            if (targetEntry == null)
            {
                return false;
            }

            long targetKeyId = targetEntry.Id;

            if (isKey)
            {
                property.stringValue = replacedValue;

                var keyIdProp = property.serializedObject.FindProperty(
                    property.propertyPath.Replace("m_Key", "m_KeyId"));
                if (keyIdProp != null && keyIdProp.propertyType == SerializedPropertyType.Integer)
                {
                    keyIdProp.longValue = targetKeyId;
                }
            }
            else if (isKeyId && property.propertyType == SerializedPropertyType.Integer)
            {
                property.longValue = targetKeyId;

                var keyProp = property.serializedObject.FindProperty(property.propertyPath.Replace("m_KeyId", "m_Key"));
                if (keyProp != null && keyProp.propertyType == SerializedPropertyType.String)
                {
                    keyProp.stringValue = replacedValue;
                }
            }

            return true;
        }

        private SharedTableData GetSharedTableData(SerializedProperty property, bool isKey)
        {
            string tableNamePath = isKey
                ? property.propertyPath.Replace("m_TableEntryReference.m_Key", "m_TableReference.m_TableCollectionName")
                : property.propertyPath.Replace("m_TableEntryReference.m_KeyId",
                    "m_TableReference.m_TableCollectionName");

            string tableGuidPath = isKey
                ? property.propertyPath.Replace("m_TableEntryReference.m_Key",
                    "m_TableReference.m_TableCollectionGuidString")
                : property.propertyPath.Replace("m_TableEntryReference.m_KeyId",
                    "m_TableReference.m_TableCollectionGuidString");

            var tableNameProp = property.serializedObject.FindProperty(tableNamePath);
            var tableGuidProp = property.serializedObject.FindProperty(tableGuidPath);

            string tableName = tableNameProp?.stringValue;
            string tableGuid = tableGuidProp?.stringValue;

            return GetSharedTableData(tableName, tableGuid);
        }

        private SharedTableData GetSharedTableData(string tableName, string tableGuid)
        {
            StringTableCollection collection = null;

            string guidString = tableGuid;

            if (guidString.IsNullOrWhiteSpace() && tableName.IsNullOrWhiteSpace() == false &&
                tableName.StartsWith("GUID:", StringComparison.OrdinalIgnoreCase))
            {
                guidString = tableName.Substring("GUID:".Length);
            }

            if (guidString.IsNullOrEmpty() == false && Guid.TryParse(guidString, out var guid))
            {
                collection = LocalizationEditorSettings.GetStringTableCollection(guid);
            }

            if (collection == null && tableName.IsNullOrEmpty() == false)
            {
                collection = LocalizationEditorSettings.GetStringTableCollection(tableName);
            }

            return collection?.SharedData;
        }
    }
}
#endif