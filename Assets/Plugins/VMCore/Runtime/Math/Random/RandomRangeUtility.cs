using System.Runtime.CompilerServices;
using UnityEngine;
using Random = System.Random;

namespace VMFramework.Core
{
    public static class RandomRangeUtility
    {
        #region KCube

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Range(this Random random, RangeInteger range) => random.Next(range.min, range.max + 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Range(this Random random, RangeFloat range) =>
            (float)random.NextDouble() * (range.max - range.min) + range.min;

        #endregion
        
        #region Min and Max

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Range(this Random random, int min, int max) => random.Next(min, max + 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Range(this Random random, float min, float max) =>
            (float)random.NextDouble() * (max - min) + min;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Range(this Random random, Vector2Int min, Vector2Int max) =>
            new(random.Range(min.x, max.x), random.Range(min.y, max.y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Range(this Random random, Vector2 min, Vector2 max) =>
            new(random.Range(min.x, max.x), random.Range(min.y, max.y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int Range(this Random random, Vector3Int min, Vector3Int max) =>
            new(random.Range(min.x, max.x), random.Range(min.y, max.y), random.Range(min.z, max.z));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Range(this Random random, Vector3 min, Vector3 max) =>
            new(random.Range(min.x, max.x), random.Range(min.y, max.y), random.Range(min.z, max.z));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Range(this Random random, Vector4 min, Vector4 max) =>
            new(random.Range(min.x, max.x), random.Range(min.y, max.y), random.Range(min.z, max.z),
                random.Range(min.w, max.w));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color Range(this Random random, Color min, Color max) =>
            new(random.Range(min.r, max.r), random.Range(min.g, max.g), random.Range(min.b, max.b),
                random.Range(min.a, max.a));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RandomRange(this int min, int max) => GlobalRandom.Default.Range(min, max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float RandomRange(this float min, float max) => GlobalRandom.Default.Range(min, max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int RandomRange(this Vector2Int min, Vector2Int max) =>
            GlobalRandom.Default.Range(min, max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 RandomRange(this Vector2 min, Vector2 max) => GlobalRandom.Default.Range(min, max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int RandomRange(this Vector3Int min, Vector3Int max) =>
            GlobalRandom.Default.Range(min, max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 RandomRange(this Vector3 min, Vector3 max) => GlobalRandom.Default.Range(min, max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 RandomRange(this Vector4 min, Vector4 max) => GlobalRandom.Default.Range(min, max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color RandomRange(this Color min, Color max) => GlobalRandom.Default.Range(min, max);

        #endregion

        #region Size And Length

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Range(this Random random, int length) => random.Next(length);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Range(this Random random, float length) => (float)random.NextDouble() * length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Range(this Random random, Vector2Int size) =>
            new(random.Range(size.x), random.Range(size.y));
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Range(this Random random, Vector2 size) =>
            new(random.Range(size.x), random.Range(size.y));
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int Range(this Random random, Vector3Int size) =>
            new(random.Range(size.x), random.Range(size.y), random.Range(size.z));
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Range(this Random random, Vector3 size) =>
            new(random.Range(size.x), random.Range(size.y), random.Range(size.z));
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Range(this Random random, Vector4 size) =>
            new(random.Range(size.x), random.Range(size.y), random.Range(size.z), random.Range(size.w));
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color Range(this Random random, Color size) =>
            new(random.Range(size.r), random.Range(size.g), random.Range(size.b), random.Range(size.a));
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RandomRange(this int length) => GlobalRandom.Default.Range(length);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float RandomRange(this float length) => GlobalRandom.Default.Range(length);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int RandomRange(this Vector2Int size) => GlobalRandom.Default.Range(size);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 RandomRange(this Vector2 size) => GlobalRandom.Default.Range(size);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int RandomRange(this Vector3Int size) => GlobalRandom.Default.Range(size);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 RandomRange(this Vector3 size) => GlobalRandom.Default.Range(size);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 RandomRange(this Vector4 size) => GlobalRandom.Default.Range(size);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color RandomRange(this Color size) => GlobalRandom.Default.Range(size);

        #endregion
    }
}