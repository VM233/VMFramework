#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Configuration
{
    public partial class CommonPresetGeneralSetting
    {
        public bool EnsurePreset(string key, Type presetType, string[] initialKeys, object[] initialValues)
        {
            if (key.IsNullOrEmpty())
            {
                return false;
            }

            if (presetType == null || typeof(CommonPreset).IsAssignableFrom(presetType) == false)
            {
                Debug.LogError($"Common preset '{key}' requires a {nameof(CommonPreset)} asset type.");
                return false;
            }

            bool anyChange = false;

            if (presets.TryGetValue(key, out var preset))
            {
                if (preset != null)
                {
                    if (presetType.IsInstanceOfType(preset) == false)
                    {
                        Debug.LogError(
                            $"Common preset '{key}' has type {preset.GetType()}, but {presetType} is required.");
                        return false;
                    }
                }
            }

            if (preset == null)
            {
                var fileName = key.ToPascalCase(" ");
                var path = ConfigurationPath.DEFAULT_COMMON_PRIORITIES_PATH.PathCombine(fileName);
                var asset = presetType.FindOrCreateScriptableObjectAtPath(path);

                if (asset is not CommonPreset createdPreset)
                {
                    return false;
                }

                preset = createdPreset;
                presets[key] = preset;
                anyChange = true;
            }

            if (initialKeys.IsNullOrEmpty() == false)
            {
                for (int i = 0; i < initialKeys.Length; i++)
                {
                    var initialKey = initialKeys[i];

                    object initialValue = null;
                    if (initialValues.IsNullOrEmpty() == false)
                    {
                        if (i < initialValues.Length)
                        {
                            initialValue = initialValues[i];
                        }
                    }

                    if (preset.ContainsItem(initialKey))
                    {
                        continue;
                    }

                    preset.AddItem(initialKey, initialValue);
                    anyChange = true;
                }
            }

            if (anyChange)
            {
                preset.SetEditorDirty();
                if (EditorUtility.IsPersistent(preset))
                {
                    AssetDatabase.SaveAssetIfDirty(preset);
                }
            }

            return anyChange;
        }
    }
}
#endif
