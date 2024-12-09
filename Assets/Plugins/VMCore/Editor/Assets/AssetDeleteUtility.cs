using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace VMFramework.Core.Editor
{
    public static class AssetDeleteUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteAsset(this Object obj)
        {
            if (obj.IsAsset() == false)
            {
                Debug.LogWarning($"{obj} is not an asset, cannot delete it.");
                return;
            }
            
            AssetDatabase.DeleteAsset(obj.GetAssetPath());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteAssetWithDialog(this Object obj)
        {
            if (obj == null)
            {
                Debug.LogWarning($"The object is null, cannot delete the asset.");
                return;
            }

            if (obj.IsAsset() == false)
            {
                Debug.LogWarning($"{obj} is not an asset, cannot delete it.");
                return;
            }
            
            if ($"Are you sure you want to delete the asset : {obj.name} ?".DisplayWarningDialog())
            {
                AssetDatabase.DeleteAsset(obj.GetAssetPath());
            }
        }
    }
}