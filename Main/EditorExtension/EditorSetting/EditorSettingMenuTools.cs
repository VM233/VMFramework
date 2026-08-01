#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using VMFramework.Core.Editor;
using VMFramework.Editor;
using VMFramework.Procedure.Editor;

namespace VMFramework.GameLogicArchitecture.Editor
{
    internal static class EditorSettingMenuTools
    {
        [MenuItem(UnityMenuItemNames.GAME_PREFABS_TOOLS + "Collect All Game Prefab Providers")]
        private static void CollectAllGamePrefabProviders()
        {
            foreach (var generalSetting in GlobalSettingCollector.GetAllGeneralSettings())
            {
                if (generalSetting is GamePrefabGeneralSetting gamePrefabSetting)
                {
                    gamePrefabSetting.CollectAllGamePrefabProviders();
                }
            }
        }

        [MenuItem(UnityMenuItemNames.GAME_PREFABS_TOOLS + "Remove Empty Game Prefab Wrappers")]
        private static void RemoveEmptyGamePrefabWrappers()
        {
            GamePrefabWrapperRemover.RemoveEmptyWrappers();
        }

        [MenuItem(UnityMenuItemNames.GLOBAL_SETTINGS + "Move General Settings To Configured Folder")]
        private static void MoveGeneralSettingsToConfiguredFolder()
        {
            foreach (var generalSetting in GlobalSettingCollector.GetAllGeneralSettings())
            {
                if (generalSetting is Object obj)
                {
                    obj.MoveAssetToNewFolder(EditorSetting.GeneralSettingsAssetFolderPath);
                }
            }
        }

        [MenuItem(UnityMenuItemNames.GLOBAL_SETTINGS + "Auto Find All Settings")]
        private static void AutoFindAllSettings()
        {
            foreach (var globalSettingFile in GlobalSettingFileEditorManager.GetGlobalSettings())
            {
                globalSettingFile.AutoFindSettings();
            }
        }

        [MenuItem(UnityMenuItemNames.GLOBAL_SETTINGS + "Auto Find And Create All Settings")]
        private static void AutoFindAndCreateAllSettings()
        {
            foreach (var globalSettingFile in GlobalSettingFileEditorManager.GetGlobalSettings())
            {
                globalSettingFile.AutoFindAndCreateSettings();
            }

            EditorInitializer.ScheduleInitialize();
        }

        [MenuItem(UnityMenuItemNames.GAME_PREFABS_TOOLS + "Move Game Prefab Wrappers To Configured Folder")]
        private static void MoveGamePrefabWrappersToConfiguredFolder()
        {
            foreach (var wrapper in GamePrefabWrapperQueryTools.GetAllGamePrefabWrappers())
            {
                wrapper.MoveToDefaultFolder();
            }
        }

        [MenuItem(UnityMenuItemNames.GLOBAL_SETTINGS + "Make Settings Addressable")]
        private static void MakeSettingsAddressable()
        {
            GlobalSettingFileAddressableManager.AutoGroupAllGlobalSettings();

            if (EditorSetting.GeneralSettingsAssetFolderPath.TryGetFolderObject(out var generalSettingsFolder))
            {
                generalSettingsFolder.CreateOrMoveEntryToDefaultGroup();
            }

            if (EditorSetting.GamePrefabsAssetFolderPath.TryGetFolderObject(out var gamePrefabsFolder))
            {
                gamePrefabsFolder.CreateOrMoveEntryToDefaultGroup();
            }
        }
    }
}
#endif
