#if UNITY_EDITOR
using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;

namespace VMFramework.GameLogicArchitecture.Editor
{
    public static class GlobalSettingFileAddressableManager
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AutoAddressableGroup(this IGlobalSettingFile globalSettingFile)
        {
            var unityObject = (Object)globalSettingFile;

            if (unityObject.IsAddressableAsset())
            {
                return;
            }

            var entry = unityObject.CreateOrMoveEntryToDefaultGroup();

            if (globalSettingFile.TryGetGlobalSettingPath(out _, out var fileName))
            {
                entry.SetAddress(fileName.GetFileNameWithoutExtensionFromPath());
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AutoGroupAllGlobalSettings()
        {
            foreach (var globalSettingFile in GlobalSettingFileEditorManager.GetGlobalSettings())
            {
                globalSettingFile.AutoAddressableGroup();
            }
        }
    }
}
#endif