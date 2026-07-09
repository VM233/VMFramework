using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Pools;
using Random = System.Random;

namespace VMFramework.Maps
{
    public static class FillableMapRandomUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FillRandomRectangleTiles<TTileInfo>(this ITileFillableMap<Vector2Int, TTileInfo> map,
            RectangleInteger rectangle, TTileInfo tileInfo, int count, Random random)
        {
            var array = ArrayDefaultPool<Vector2Int>.GetByMinLength(count);

            rectangle.GetRandomItems(count, ref array, random);
            
            for (int i = 0; i < count; i++)
            {
                map.FillTile(array[i], tileInfo);
            }
            
            array.ReturnToDefaultPool();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FillRandomRectangleTiles<TTileInfo>(this ITileFillableMap<Vector2Int, TTileInfo> map,
            RectangleInteger rectangle, TTileInfo tileInfo, float density, Random random)
        {
            var count = (rectangle.Count * density).Round();
            FillRandomRectangleTiles(map, rectangle, tileInfo, count, random);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FillRandomCubeTiles<TTileInfo>(this ITileFillableMap<Vector3Int, TTileInfo> map,
            CubeInteger cube, TTileInfo tileInfo, int count, Random random)
        {
            var array = ArrayDefaultPool<Vector3Int>.GetByMinLength(count);

            cube.GetRandomItems(count, ref array, random);

            for (int i = 0; i < count; i++)
            {
                map.FillTile(array[i], tileInfo);
            }
            
            array.ReturnToDefaultPool();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FillRandomCubeTiles<TTileInfo>(this ITileFillableMap<Vector3Int, TTileInfo> map,
            CubeInteger cube, TTileInfo tileInfo, float density, Random random)
        {
            var count = (cube.Count * density).Round();
            FillRandomCubeTiles(map, cube, tileInfo, count, random);
        }
    }
}