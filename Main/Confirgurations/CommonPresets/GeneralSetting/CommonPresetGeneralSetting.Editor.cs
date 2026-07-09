#if UNITY_EDITOR
using System;
using VMFramework.Core;
using VMFramework.Core.Editor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Configuration
{
    public partial class CommonPresetGeneralSetting
    {
        public bool AddPresetIfNotExists(string key, Type presetType, string[] initialKeys, object[] initialValues)
        {
            if (key.IsNullOrEmpty())
            {
                return false;
            }

            if (presets.TryGetValue(key, out var preset))
            {
                if (preset != null)
                {
                    return false;
                }
            }

            var fileName = key.ToPascalCase(" ");
            var path = ConfigurationPath.DEFAULT_COMMON_PRIORITIES_PATH.PathCombine(fileName);
            var asset = presetType.CreateScriptableObjectAsset(path);

            if (asset == null)
            {
                return false;
            }

            preset = (CommonPreset)asset;
            presets[key] = preset;

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

                    preset.AddItem(initialKey, initialValue);
                }
            }

            return true;
        }
    }
}
#endif