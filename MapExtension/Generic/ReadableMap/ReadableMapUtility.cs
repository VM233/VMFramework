using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Maps
{
    public static class ReadableMapUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetTile<TPoint, TTile>(this IReadableMap<TPoint, TTile> map, TPoint point, out TTile tile)
        {
            return map.TryGetTile(point, out tile);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsTile<TPoint, TTile>(this IReadableMap<TPoint, TTile> map, TPoint point)
        {
            return map.ContainsTile(point);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TTile> GetRectangleTiles<TTile>(this IReadableMap<Vector2Int, TTile> map,
            RectangleInteger rectangle)
        {
            foreach (var point in rectangle)
            {
                yield return map.GetTile(point);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TTile> GetValidRectangleTiles<TTile>(this IReadableMap<Vector2Int, TTile> map,
            RectangleInteger rectangle)
        {
            foreach (var point in rectangle)
            {
                if (map.TryGetTile(point, out var tile))
                {
                    yield return tile;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TTile> GetCubeTiles<TTile>(this IReadableMap<Vector3Int, TTile> map,
            CubeInteger rectangle)
        {
            foreach (var point in rectangle)
            {
                yield return map.GetTile(point);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TTile> GetValidCubeTiles<TTile>(this IReadableMap<Vector3Int, TTile> map,
            CubeInteger rectangle)
        {
            foreach (var point in rectangle)
            {
                if (map.TryGetTile(point, out var tile))
                {
                    yield return tile;
                }
            }
        }
    }
}