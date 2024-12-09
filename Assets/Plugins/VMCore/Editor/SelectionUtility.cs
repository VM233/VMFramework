using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace VMFramework.Core.Editor
{
    public static class SelectionUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SelectObject(this Object obj)
        {
            Selection.activeObject = obj;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetSelectedFolderPath(out string selectedAssetFolderPath)
        {
            if (Selection.activeObject == null)
            {
                selectedAssetFolderPath = null;
                return false;
            }

            selectedAssetFolderPath = Selection.activeObject.GetAssetPath();
            
            if (Selection.activeObject.IsFolder() == false)
            {
                selectedAssetFolderPath = selectedAssetFolderPath.GetDirectoryPath().MakeAssetPath();
            }
            
            return true;
        }
    }
}