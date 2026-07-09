using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Maps
{
    public static class GridMapChunkUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int GetChunkPosition<TMap>(this TMap map, Vector3Int tilePosition) where TMap : IGridMap
        {
            return tilePosition.CircularDivide(map.ChunkSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetChunkPositionAndRelativePosition<TMap>(this TMap map, Vector3Int tilePosition,
            out Vector3Int chunkPosition, out Vector3Int relativePosition) where TMap : IGridMap
        {
            chunkPosition = map.GetChunkPosition(tilePosition);
            relativePosition = tilePosition - chunkPosition * map.ChunkSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IGridChunk GetChunkByTilePosition<TMap>(this TMap map, Vector3Int tilePosition)
            where TMap : IGridMap
        {
            return map.GetChunk(map.GetChunkPosition(tilePosition));
        }

        [return: NotNull]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IGridChunk GetOrCreateChunk<TMap>(this TMap map, Vector3Int chunkPosition)
            where TMap : IGridMap
        {
            if (map.TryGetChunk(chunkPosition, out var chunk))
            {
                return chunk;
            }

            return map.CreateChunk(chunkPosition);
        }

        [return: NotNull]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IGridChunk GetOrCreateChunk<TMap>(this TMap map, Vector3Int chunkPosition, out bool created)
            where TMap : IGridMap
        {
            if (map.TryGetChunk(chunkPosition, out var chunk))
            {
                created = false;
                return chunk;
            }

            created = true;
            return map.CreateChunk(chunkPosition);
        }

        [return: NotNull]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IGridChunk CreateChunk<TMap>(this TMap map, Vector3Int chunkPosition)
            where TMap : IGridMap
        {
            if (map.TryCreateChunk(chunkPosition, out var chunk))
            {
                return chunk;
            }

            throw new InvalidOperationException("Failed to create chunk at position: " + chunkPosition);
        }
    }
}