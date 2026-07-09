using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Maps
{
    public static class DestructibleMapUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool DestructTile<TPoint, TTile>(this ITileDestructibleMap<TPoint, TTile> map, TPoint position)
        {
            return map.DestructTile(position, out _);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DestructRectangleTiles<TTile>(this ITileDestructibleMap<Vector2Int, TTile> map,
            RectangleInteger rectangle)
        {
            foreach (var position in rectangle)
            {
                map.DestructTile(position, out _);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DestructCubeTiles<TTile>(this ITileDestructibleMap<Vector3Int, TTile> map, CubeInteger cube)
        {
            foreach (var position in cube)
            {
                map.DestructTile(position, out _);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool DestructTile<TPoint, TInfo, TTile>(this ITileDestructibleMap<TPoint, TInfo, TTile> map,
            TPoint position, TInfo info)
        {
            return map.DestructTile(position, info, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DestructRectangleTiles<TInfo, TTile>(this ITileDestructibleMap<Vector2Int, TInfo, TTile> map,
            RectangleInteger rectangle, TInfo info)
        {
            foreach (var position in rectangle)
            {
                map.DestructTile(position, info, out _);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DestructCubeTiles<TInfo, TTile>(this ITileDestructibleMap<Vector3Int, TInfo, TTile> map,
            CubeInteger cube, TInfo info)
        {
            foreach (var position in cube)
            {
                map.DestructTile(position, info, out _);
            }
        }
    }
}