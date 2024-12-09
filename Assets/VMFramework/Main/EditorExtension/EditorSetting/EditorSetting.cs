#if UNITY_EDITOR
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.Procedure;

namespace VMFramework.GameLogicArchitecture.Editor
{
    [ManagerCreationProvider(ManagerType.SettingCore)]
    public sealed class EditorSetting : GlobalSetting<EditorSetting, EditorSettingFile>
    {
        public static GameEditorGeneralSetting GameEditorGeneralSetting => 
            GlobalSettingFile == null? null : GlobalSettingFile.gameEditorGeneralSetting;

        public static HierarchyGeneralSetting HierarchyGeneralSetting => 
            GlobalSettingFile == null? null : GlobalSettingFile.hierarchyGeneralSetting;

        public static TextureImporterGeneralSetting TextureImporterGeneralSetting =>
            GlobalSettingFile == null? null : GlobalSettingFile.textureImporterGeneralSetting;
        
        public static GamePrefabWrapperEditorGeneralSetting GamePrefabWrapperEditorGeneralSetting =>
            GlobalSettingFile == null? null : GlobalSettingFile.gamePrefabWrapperEditorGeneralSetting;

        public static string GeneralSettingsAssetFolderPath =>
            GlobalSettingFile == null
                ? ConfigurationPath.DEFAULT_GENERAL_SETTINGS_PATH
                : GlobalSettingFile.generalSettingsAssetFolderPath;
        
        public static string GamePrefabsAssetFolderPath =>
            GlobalSettingFile == null
                ? ConfigurationPath.DEFAULT_GAME_PREFABS_PATH
                : GlobalSettingFile.gamePrefabsAssetFolderPath;
    }
}
#endif