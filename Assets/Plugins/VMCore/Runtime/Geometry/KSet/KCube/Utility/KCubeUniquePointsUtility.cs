using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = System.Random;

namespace VMFramework.Core
{
    public static class KCubeUniquePointsUtility
    {
        /// <inheritdoc cref="UniqueRandomUtility.UniqueIntegers"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetRandomUniquePoints<TKCube>(this TKCube cube, int count, ICollection<int> collection,
            Random random)
            where TKCube : IKCube<int> =>
            random.UniqueIntegers(count, cube.Min, cube.Max, collection);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetRandomUniquePoints<TKCube>(this TKCube cube, int count, ICollection<int> collection)
            where TKCube : IKCube<int> =>
            cube.GetRandomUniquePoints(count, collection, GlobalRandom.Default);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetRandomUniquePoints<TKCube>(this TKCube cube, int count,
            ICollection<Vector2Int> collection, Random random)
            where TKCube : IKCube<Vector2Int> =>
            random.UniqueVector2Ints(count, cube.Min, cube.Max, collection);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetRandomUniquePoints<TKCube>(this TKCube cube, int count,
            ICollection<Vector2Int> collection)
            where TKCube : IKCube<Vector2Int> =>
            cube.GetRandomUniquePoints(count, collection, GlobalRandom.Default);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetRandomUniquePoints<TKCube>(this TKCube cube, int count,
            ICollection<Vector3Int> collection, Random random)
            where TKCube : IKCube<Vector3Int> =>
            random.UniqueVector3Ints(count, cube.Min, cube.Max, collection);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetRandomUniquePoints<TKCube>(this TKCube cube, int count,
            ICollection<Vector3Int> collection)
            where TKCube : IKCube<Vector3Int> =>
            cube.GetRandomUniquePoints(count, collection, GlobalRandom.Default);
    }
}