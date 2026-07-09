#if UNITY_EDITOR
using VMFramework.Procedure;

namespace VMFramework.GameLogicArchitecture.Editor
{
    [ManagerCreationProvider(ManagerType.SettingCore)]
    public sealed class EditorSetting : GlobalSetting<EditorSetting, EditorSettingFile>
    {
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
