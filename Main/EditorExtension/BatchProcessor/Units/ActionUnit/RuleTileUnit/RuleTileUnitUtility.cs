#if UNITY_EDITOR && ENABLE_TILEMAP
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;

namespace VMFramework.Editor.BatchProcessor
{
    public static class RuleTileUnitUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetRuleTilePath(Sprite sprite)
        {
            if (SelectionUtility.TryGetSelectedFolderPath(out var selectedFolderPath) == false)
            {
                selectedFolderPath = sprite.texture.GetAssetFolderPath();
            }
            
            var name = sprite.name + ".asset";
            var path = selectedFolderPath.PathCombine(name);
            path = AssetDatabase.GenerateUniqueAssetPath(path);
            return path;
        }
    }
}
#endif