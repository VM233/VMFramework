#if UNITY_EDITOR
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace VMFramework.Core.Editor
{
    public static class AddressableEditorUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAddressableAsset(this Object obj)
        {
            var setting = AddressableAssetSettingsDefaultObject.GetSettings(false);

            if (setting == null)
            {
                return false;
            }
            
            return setting.FindAssetEntry(obj.GetAssetGUID()) != null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AddressableAssetEntry CreateOrMoveEntryToDefaultGroup(this Object obj)
        {
            var setting = AddressableAssetSettingsDefaultObject.GetSettings(true);
            
            var guid = obj.GetAssetGUID();
            
            return setting.CreateOrMoveEntry(guid, setting.DefaultGroup);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SelectEntryGroupInAddressableWindow(this Object obj)
        {
            var setting = AddressableAssetSettingsDefaultObject.GetSettings(false);
            
            if (setting == null)
            {
                return;
            }
            
            var entry = setting.FindAssetEntry(obj.GetAssetGUID());
            
            if (entry == null)
            {
                return;
            }
            
            Selection.activeObject = entry.parentGroup;
        }
    }
}
#endif