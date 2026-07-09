using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Maps
{
    public static class FillableMapUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FillRectangleTiles<TMap, TTileInfo>(this TMap map, RectangleInteger rectangle,
            TTileInfo tileInfo) where TMap : ITileFillableMap<Vector2Int, TTileInfo>
        {
            foreach (var position in rectangle)
            {
                map.FillTile(position, tileInfo);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FillCubeTiles<TMap, TTileInfo>(this TMap map, CubeInteger cube, TTileInfo tileInfo)
            where TMap : ITileFillableMap<Vector3Int, TTileInfo>
        {
            foreach (var position in cube)
            {
                map.FillTile(position, tileInfo);
            }
        }
    }
}