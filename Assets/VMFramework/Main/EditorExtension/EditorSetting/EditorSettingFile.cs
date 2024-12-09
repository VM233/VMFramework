#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.Procedure.Editor;

namespace VMFramework.GameLogicArchitecture.Editor
{
    [GlobalSettingFileConfig(FileName = nameof(EditorSettingFile))]
    [GlobalSettingFileEditorConfig(FolderPath = ConfigurationPath.INTERNAL_GLOBAL_SETTINGS_PATH)]
    public sealed partial class EditorSettingFile : GlobalSettingFile
    {
        private const string EDITOR_EXTENSION_CATEGORY = "Editor Extension";
        
        private const string RESOURCES_PATH_CATEGORY = "Resources Path";
        
        [TabGroup(TAB_GROUP_NAME, EDITOR_EXTENSION_CATEGORY)]
        [Required]
        public GameEditorGeneralSetting gameEditorGeneralSetting;

        [TabGroup(TAB_GROUP_NAME, EDITOR_EXTENSION_CATEGORY)]
        [Required]
        public HierarchyGeneralSetting hierarchyGeneralSetting;
        
        [TabGroup(TAB_GROUP_NAME, EDITOR_EXTENSION_CATEGORY)]
        [Required]
        public TextureImporterGeneralSetting textureImporterGeneralSetting;
        
        [TabGroup(TAB_GROUP_NAME, EDITOR_EXTENSION_CATEGORY)]
        [Required]
        public GamePrefabWrapperEditorGeneralSetting gamePrefabWrapperEditorGeneralSetting;

        [TabGroup(TAB_GROUP_NAME, RESOURCES_PATH_CATEGORY)]
        [FolderPath]
        public string generalSettingsAssetFolderPath = ConfigurationPath.DEFAULT_GENERAL_SETTINGS_PATH;
        
        [TabGroup(TAB_GROUP_NAME, RESOURCES_PATH_CATEGORY)]
        [FolderPath]
        public string gamePrefabsAssetFolderPath = ConfigurationPath.DEFAULT_GAME_PREFABS_PATH;

        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();

            if (generalSettingsAssetFolderPath.IsNullOrEmpty())
            {
                generalSettingsAssetFolderPath = ConfigurationPath.DEFAULT_GENERAL_SETTINGS_PATH;
            }

            if (gamePrefabsAssetFolderPath.IsNullOrEmpty())
            {
                gamePrefabsAssetFolderPath = ConfigurationPath.DEFAULT_GAME_PREFABS_PATH;
            }
        }

        [Button(ButtonSizes.Medium), TabGroup(TAB_GROUP_NAME, RESOURCES_PATH_CATEGORY)]
        public static void RemoveEmptyGamePrefabWrappers()
        {
            GamePrefabWrapperRemover.RemoveEmptyGamePrefabWrappers();
        }
        
        [Button(ButtonSizes.Medium), TabGroup(TAB_GROUP_NAME, RESOURCES_PATH_CATEGORY)]
        public void MoveGeneralSettingsToNewFolder()
        {
            foreach (var generalSetting in GlobalSettingCollector.GetAllGeneralSettings())
            {
                if (generalSetting is Object obj)
                {
                    obj.MoveAssetToNewFolder(EditorSetting.GeneralSettingsAssetFolderPath);
                }
            }
        }
        
        [Button(ButtonSizes.Medium), TabGroup(TAB_GROUP_NAME, RESOURCES_PATH_CATEGORY)]
        public static void AutoFindAllSettings()
        {
            foreach (var globalSettingFile in GlobalSettingFileEditorManager.GetGlobalSettings())
            {
                globalSettingFile.AutoFindSettings();
            }
        }

        [Button(ButtonSizes.Medium), TabGroup(TAB_GROUP_NAME, RESOURCES_PATH_CATEGORY)]
        public static void AutoFindAndCreateAllSettings()
        {
            foreach (var globalSettingFile in GlobalSettingFileEditorManager.GetGlobalSettings())
            {
                globalSettingFile.AutoFindAndCreateSettings();
            }
            
            EditorInitializer.Initialize();
        }
        
        [Button(ButtonSizes.Medium), TabGroup(TAB_GROUP_NAME, RESOURCES_PATH_CATEGORY)]
        public void MoveGamePrefabWrappersToNewFolder()
        {
            foreach (var wrapper in GamePrefabWrapperQueryTools.GetAllGamePrefabWrappers())
            {
                wrapper.MoveToDefaultFolder();
            }
        }
        
        [Button(ButtonSizes.Medium), TabGroup(TAB_GROUP_NAME, RESOURCES_PATH_CATEGORY)]
        public void MakeSettingsAddressable()
        {
            GlobalSettingFileAddressableManager.AutoGroupAllGlobalSettings();

            if (generalSettingsAssetFolderPath.TryGetFolderObject(out var generalSettingsFolder))
            {
                generalSettingsFolder.CreateOrMoveEntryToDefaultGroup();
            }
            
            if (gamePrefabsAssetFolderPath.TryGetFolderObject(out var gamePrefabsFolder))
            {
                gamePrefabsFolder.CreateOrMoveEntryToDefaultGroup();
            }
        }
    }
}
#endif