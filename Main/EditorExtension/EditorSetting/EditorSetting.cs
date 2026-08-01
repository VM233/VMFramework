#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace VMFramework.GameLogicArchitecture.Editor
{
    [FilePath(PROJECT_SETTINGS_FILE_PATH, FilePathAttribute.Location.ProjectFolder)]
    public sealed class EditorSetting : ScriptableSingleton<EditorSetting>
    {
        internal const string PROJECT_SETTINGS_FILE_PATH = "ProjectSettings/VMFrameworkEditorSettings.asset";

        [SerializeField]
        private string generalSettingsAssetFolderPath = ConfigurationPath.DEFAULT_GENERAL_SETTINGS_PATH;

        [SerializeField]
        private string gamePrefabsAssetFolderPath = ConfigurationPath.DEFAULT_GAME_PREFABS_PATH;

        public static string GeneralSettingsAssetFolderPath =>
            GetFolderPath(instance.generalSettingsAssetFolderPath, ConfigurationPath.DEFAULT_GENERAL_SETTINGS_PATH);

        public static string GamePrefabsAssetFolderPath =>
            GetFolderPath(instance.gamePrefabsAssetFolderPath, ConfigurationPath.DEFAULT_GAME_PREFABS_PATH);

        internal string GeneralSettingsAssetFolderPathValue
        {
            get => GeneralSettingsAssetFolderPath;
            set => generalSettingsAssetFolderPath = NormalizeFolderPath(value);
        }

        internal string GamePrefabsAssetFolderPathValue
        {
            get => GamePrefabsAssetFolderPath;
            set => gamePrefabsAssetFolderPath = NormalizeFolderPath(value);
        }

        internal static EditorSetting Instance => instance;

        internal void SaveSettings()
        {
            Save(true);
        }

        private static string GetFolderPath(string configuredPath, string defaultPath)
        {
            var normalizedPath = NormalizeFolderPath(configuredPath);
            return string.IsNullOrEmpty(normalizedPath) ? defaultPath : normalizedPath;
        }

        private static string NormalizeFolderPath(string path)
        {
            return path?.Trim().Replace('\\', '/').TrimEnd('/');
        }
    }
}
#endif
