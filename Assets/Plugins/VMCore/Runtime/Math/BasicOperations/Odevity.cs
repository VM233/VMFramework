using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class Odevity
    {
        #region Is Odd

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOdd(this int num) => num % 2 == 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOdd(this long num) => num % 2 == 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOdd(this short num) => num % 2 == 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAllOdd(this Vector2Int vector) => vector.x.IsOdd() && vector.y.IsOdd();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAllOdd(this Vector3Int vector) => vector.x.IsOdd() && vector.y.IsOdd() && vector.z.IsOdd();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAnyOdd(this Vector2Int vector) => vector.x.IsOdd() || vector.y.IsOdd();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAnyOdd(this Vector3Int vector) => vector.x.IsOdd() || vector.y.IsOdd() || vector.z.IsOdd();

        #endregion

        #region Is Even

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEven(this int num) => num % 2 == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEven(this long num) => num % 2 == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEven(this short num) => num % 2 == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAllEven(this Vector2Int vector) => vector.x.IsEven() && vector.y.IsEven();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAllEven(this Vector3Int vector) =>
            vector.x.IsEven() && vector.y.IsEven() && vector.z.IsEven();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAnyEven(this Vector2Int vector) => vector.x.IsEven() || vector.y.IsEven();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAnyEven(this Vector3Int vector) =>
            vector.x.IsEven() || vector.y.IsEven() || vector.z.IsEven();

        #endregion
    }
}