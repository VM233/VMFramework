using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Maps
{
    public static class GridMapTileQueryUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsTile(this IGridMap map, Vector3Int tilePosition)
        {
            map.GetChunkPositionAndRelativePosition(tilePosition, out var chunkPosition, out var relativePosition);

            if (map.TryGetChunk(chunkPosition, out var chunk) == false)
            {
                return false;
            }
            
            return chunk.ContainsTile(relativePosition);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IGridTile GetTile(this IGridMap map, Vector3Int tilePosition)
        {
            map.GetChunkPositionAndRelativePosition(tilePosition, out var chunkPosition, out var relativePosition);
            
            if (map.TryGetChunk(chunkPosition, out var chunk) == false)
            {
                return null;
            }
            
            return chunk.GetTile(relativePosition);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetTile(this IGridMap map, Vector3Int tilePosition, out IGridTile tile)
        {
            map.GetChunkPositionAndRelativePosition(tilePosition, out var chunkPosition, out var relativePosition);
            if (map.TryGetChunk(chunkPosition, out var chunk) == false)
            {
                tile = null;
                return false;
            }
            
            return chunk.TryGetTile(relativePosition, out tile);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetTile<TGridTile>(this IGridMap map, Vector3Int tilePosition, out TGridTile tile)
        where TGridTile : IGridTile
        {
            map.GetChunkPositionAndRelativePosition(tilePosition, out var chunkPosition, out var relativePosition);
            if (map.TryGetChunk(chunkPosition, out var chunk) == false)
            {
                tile = default;
                return false;
            }

            if (chunk.TryGetTile(relativePosition, out var gridTile))
            {
                tile = (TGridTile)gridTile;
                return true;
            }
            
            tile = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IGridTile> GetAllTiles(this IGridMap map)
        {
            foreach (var chunk in map.GetAllChunks())
            {
                foreach (var tile in chunk.GetAllTiles())
                {
                    yield return tile;
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Vector3Int> GetAllTilePositions(this IGridMap map)
        {
            foreach (var tile in map.GetAllTiles())
            {
                yield return tile.Position;
            }
        }
    }
}