using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Maps
{
    public static class ReplaceableMapUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceRectangleTiles<TMap, TTileInfo>(this TMap map, RectangleInteger rectangle,
            TTileInfo tileInfo) where TMap : ITileReplaceableMap<Vector2Int, TTileInfo>
        {
            foreach (var position in rectangle)
            {
                map.ReplaceTile(position, tileInfo);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceCubeTiles<TMap, TTileInfo>(this TMap map, CubeInteger cube, TTileInfo tileInfo)
            where TMap : ITileReplaceableMap<Vector3Int, TTileInfo>
        {
            foreach (var position in cube)
            {
                map.ReplaceTile(position, tileInfo);
            }
        }
    }
}