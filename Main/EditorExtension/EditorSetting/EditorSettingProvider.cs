#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture.Editor
{
    internal static class EditorSettingProvider
    {
        private const string SETTINGS_PROVIDER_PATH = "Project/" + FrameworkMeta.NAME;

        [SettingsProvider]
        private static SettingsProvider CreateSettingsProvider()
        {
            return new SettingsProvider(SETTINGS_PROVIDER_PATH, SettingsScope.Project)
            {
                label = FrameworkMeta.NAME,
                guiHandler = DrawSettings,
                keywords = new HashSet<string>
                {
                    "VMFramework",
                    "General Settings",
                    "Game Prefabs",
                    "Asset Folder"
                }
            };
        }

        private static void DrawSettings(string searchContext)
        {
            var settings = EditorSetting.Instance;

            EditorGUILayout.LabelField("Configuration Asset Folders", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                $"These project-wide editor settings are stored in {EditorSetting.PROJECT_SETTINGS_FILE_PATH}.",
                MessageType.Info);

            EditorGUI.BeginChangeCheck();

            var generalSettingsAssetFolderPath = EditorGUILayout.DelayedTextField(
                new GUIContent("General Settings Folder", "Project-relative folder for GeneralSetting assets."),
                settings.GeneralSettingsAssetFolderPathValue);

            var gamePrefabsAssetFolderPath = EditorGUILayout.DelayedTextField(
                new GUIContent("Game Prefabs Folder", "Project-relative folder for GamePrefab wrapper assets."),
                settings.GamePrefabsAssetFolderPathValue);

            if (EditorGUI.EndChangeCheck() == false)
            {
                return;
            }

            settings.GeneralSettingsAssetFolderPathValue = generalSettingsAssetFolderPath;
            settings.GamePrefabsAssetFolderPathValue = gamePrefabsAssetFolderPath;
            settings.SaveSettings();
        }
    }
}
#endif
