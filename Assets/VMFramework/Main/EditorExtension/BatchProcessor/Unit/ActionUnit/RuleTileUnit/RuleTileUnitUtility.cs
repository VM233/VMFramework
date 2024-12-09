#if UNITY_EDITOR
using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;

namespace VMFramework.Editor.BatchProcessor
{
    public static class RuleTileUnitUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetRuleTilePath(Sprite sprite, out string path)
        {
            var name = sprite.texture.name;
            
            if (SelectionUtility.TryGetSelectedFolderPath(out var selectedFolderPath) == false)
            {
                selectedFolderPath = sprite.texture.GetAssetFolderPath();
            }
            
            var names = new[] { sprite.texture.name, sprite.name };

            return selectedFolderPath.TryGetFirstValidAssetPath(names, out path, ".asset");
        }
    }
}
#endif