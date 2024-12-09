using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Core.Pools;
using Random = System.Random;

namespace VMFramework.Core
{
    public static class UniqueRandomUtility
    {
        /// <summary>
        /// 生成一定范围内一组不重复的随机整数
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UniqueIntegers(this Random random, int count, int min, int max,
            [NotNull] ICollection<int> collection)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, $"{nameof(count)} must be greater than 0.");
            }

            int range = max - min + 1;

            if (range <= 0)
            {
                throw new ArgumentException(
                    $"{nameof(max)} : {max} must be greater than or equal to {nameof(min)} : {min}.");
            }

            if (count > range)
            {
                throw new ArgumentException(
                    $"{nameof(count)} : {count} is greater than the range of integers from {min} to {max}.");
            }

            if (count < range / 8)
            {
                var numbers = HashSetPool<int>.Shared.Get();
                numbers.Clear();

                while (numbers.Count < count)
                {
                    int number = random.Range(min, max);
                    numbers.Add(number);
                }

                collection.AddRange(numbers);
                numbers.ReturnToSharedPool();
                return;
            }

            var container = min.GetRange(max).ToListSharedPooled();

            container.Shuffle();

            for (int i = 0; i < count; i++)
            {
                collection.Add(container[i]);
            }

            container.ReturnToSharedPool();
        }

        /// <summary>
        /// 生成一定范围内一组不重复的随机Vector2Int
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UniqueVector2Ints(this Random random, int count, Vector2Int minPos,
            Vector2Int maxPos, [NotNull] ICollection<Vector2Int> collection)
        {
            Vector2Int range = maxPos - minPos + Vector2Int.one;

            int totalLength = range.Products();

            var source = ListPool<int>.Shared.Get();
            source.Clear();

            random.UniqueIntegers(count, 0, totalLength - 1, source);

            foreach (var index in source)
            {
                int x = index / range.y;
                int y = index - x * range.y;
                collection.Add(new(x + minPos.x, y + minPos.y));
            }

            source.ReturnToSharedPool();
        }

        /// <summary>
        /// 生成一定范围内一组不重复的随机Vector3Int
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UniqueVector3Ints(this Random random, int count, Vector3Int minPos,
            Vector3Int maxPos, [NotNull] ICollection<Vector3Int> collection)
        {
            Vector3Int range = maxPos - minPos + Vector3Int.one;

            int totalLength = range.Products();

            var source = ListPool<int>.Shared.Get();
            source.Clear();

            random.UniqueIntegers(count, 0, totalLength - 1, source);

            foreach (var index in source)
            {
                int rangeYZ = range.y * range.z;
                int x = index / rangeYZ;
                int rangeDivideByYZ = index - x * rangeYZ;
                int y = rangeDivideByYZ / range.z;
                int z = rangeDivideByYZ - y * range.z;
                collection.Add(new(x + minPos.x, y + minPos.y, z + minPos.z));
            }

            source.ReturnToSharedPool();
        }

        /// <summary>
        /// 生成一定范围内一组不重复的随机整数
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RandomUniqueIntegers(this int count, int min, int max,
            [NotNull] ICollection<int> collection) =>
            GlobalRandom.Default.UniqueIntegers(count, min, max, collection);

        /// <summary>
        /// 生成一定范围内一组不重复的随机Vector2Int
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RandomUniqueVector2Ints(this int count, Vector2Int minPos, Vector2Int maxPos,
            [NotNull] ICollection<Vector2Int> collection) =>
            GlobalRandom.Default.UniqueVector2Ints(count, minPos, maxPos, collection);

        /// <summary>
        /// 生成一定范围内一组不重复的随机Vector3Int
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RandomUniqueVector3Ints(this int count, Vector3Int minPos, Vector3Int maxPos,
            [NotNull] ICollection<Vector3Int> collection) =>
            GlobalRandom.Default.UniqueVector3Ints(count, minPos, maxPos, collection);
    }
}