using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Maps
{
    public static class GridMapPositionUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int GetMinTilePosition<TGridMap>(this TGridMap gridMap, Vector3Int chunkPosition)
            where TGridMap : IGridMap
        {
            return gridMap.ChunkSize * chunkPosition;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int GetMinTilePositionXY<TGridMap>(this TGridMap gridMap, Vector2Int chunkPosition)
            where TGridMap : IGridMap
        {
            return new(chunkPosition.x * gridMap.ChunkSize.x, chunkPosition.y * gridMap.ChunkSize.y);
        }
    }
}