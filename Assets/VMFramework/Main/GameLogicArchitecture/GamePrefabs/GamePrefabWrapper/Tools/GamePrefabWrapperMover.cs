#if UNITY_EDITOR
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMFramework.Core.Editor;

namespace VMFramework.GameLogicArchitecture.Editor
{
    public static class GamePrefabWrapperMover
    {
        private static readonly List<GamePrefabGeneralSetting> generalSettingsCache = new();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MoveToDefaultFolder(this GamePrefabWrapper gamePrefabWrapper)
        {
            generalSettingsCache.Clear();

            gamePrefabWrapper.GetGamePrefabGeneralSettings(generalSettingsCache);

            if (generalSettingsCache.Count == 0)
            {
                return;
            }

            var generalSetting = generalSettingsCache[0];
            var folderPath = generalSetting.GamePrefabFolderPath;
                
            gamePrefabWrapper.MoveAssetToNewFolder(folderPath);
        }
    }
}
#endif