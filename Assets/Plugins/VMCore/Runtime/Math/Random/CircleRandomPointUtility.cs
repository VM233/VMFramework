using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = System.Random;

namespace VMFramework.Core
{
    public static class CircleRandomPointUtility
    {
        #region Point On Circle

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 PointOnUnitCircle(this Random random)
        {
            float angle = random.Range(Constants.TWO_PI);
            float x = MathF.Cos(angle);
            float y = MathF.Sin(angle);
            return new(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 PointOnCircle(this Random random, float radius)
        {
            return random.PointOnUnitCircle() * radius;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 PointOnCircle(this Random random, Vector2 center, float radius)
        {
            return random.PointOnUnitCircle() * radius + center;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 RandomPointOnCircle(this float radius) => GlobalRandom.Default.PointOnCircle(radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 RandomPointOnCircle(this Vector2 center, float radius) =>
            GlobalRandom.Default.PointOnCircle(center, radius);

        #endregion

        #region Point Inside Circle

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 PointInsideUnitCircle(this Random random)
        {
            float angle = random.Range(Constants.TWO_PI);
            float r = (float)random.NextDouble().Sqrt();

            float x = r * MathF.Cos(angle);
            float y = r * MathF.Sin(angle);
            return new(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 PointInsideCircle(this Random random, float radius)
        {
            float angle = random.Range(Constants.TWO_PI);
            float r = (float)random.NextDouble().Sqrt() * radius;

            float x = r * MathF.Cos(angle);
            float y = r * MathF.Sin(angle);
            return new(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 PointInsideCircle(this Random random, Vector2 center, float radius) =>
            random.PointInsideCircle(radius) + center;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 RandomPointInsideCircle(this float radius) =>
            GlobalRandom.Default.PointInsideCircle(radius);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 RandomPointInsideCircle(this Vector2 center, float radius) =>
            GlobalRandom.Default.PointInsideCircle(center, radius);

        #endregion
    }
}