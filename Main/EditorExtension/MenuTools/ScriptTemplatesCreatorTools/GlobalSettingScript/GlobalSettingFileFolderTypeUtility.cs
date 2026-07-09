#if UNITY_EDITOR
using System;
using System.Runtime.CompilerServices;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Editor
{
    public static class GlobalSettingFileFolderTypeUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToFolderString(this GlobalSettingFileFolderType folderType)
        {
            return folderType switch
            {
                GlobalSettingFileFolderType.DefaultFolder => nameof(ConfigurationPath) + "." +
                                                             nameof(ConfigurationPath.DEFAULT_GLOBAL_SETTINGS_PATH),
                GlobalSettingFileFolderType.InternalFolder => nameof(ConfigurationPath) + "." +
                                                              nameof(ConfigurationPath.INTERNAL_GLOBAL_SETTINGS_PATH),
                GlobalSettingFileFolderType.Custom => "/*Enter Folder Path Here*/",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
#endif