using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = System.Random;

namespace VMFramework.Core
{
    public static class SphereRandomPointUtility
    {
        /// <summary>
        /// Generates a random point on the surface of a sphere.
        /// 在球面上生成随机点。
        /// Implementation Ref(参考链接): https://www.jasondavies.com/maps/random-points/
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 PointOnUnitSphere(this Random random)
        {
            var theta = random.Range(Constants.TWO_PI);

            var phi = MathF.Acos(random.Range(-1, 1));
            var sinPhi = MathF.Sin(phi);

            var x = sinPhi * MathF.Cos(theta);
            var y = sinPhi * MathF.Sin(theta);
            var z = MathF.Cos(phi);

            return new(x, y, z);
        }

        /// <inheritdoc cref="PointOnUnitSphere"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 PointOnSphere(this Random random, float radius) => random.PointOnUnitSphere() * radius;

        /// <inheritdoc cref="PointOnUnitSphere"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 PointOnSphere(this Random random, Vector3 center, float radius) =>
            center + random.PointOnUnitSphere() * radius;

        /// <inheritdoc cref="PointOnUnitSphere"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 RandomPointOnSphere(this float radius) => GlobalRandom.Default.PointOnSphere(radius);

        /// <inheritdoc cref="PointOnUnitSphere"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 RandomPointOnSphere(this Vector3 center, float radius) =>
            GlobalRandom.Default.PointOnSphere(center, radius);

        /// <summary>
        /// Generates a random point inside the unit sphere.
        /// 随机生成球体内的点。
        /// Implementation Ref(参考链接): https://karthikkaranth.me/blog/generating-random-points-in-a-sphere/
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 PointInsideUnitSphere(this Random random)
        {
            var pointOnSphere = random.PointOnUnitSphere();
            var r = (float)random.NextDouble().Cbrt();
            return pointOnSphere * r;
        }

        /// <inheritdoc cref="PointInsideUnitSphere"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 PointInsideSphere(this Random random, float radius)
        {
            var pointOnSphere = random.PointOnUnitSphere();
            var r = (float)random.NextDouble().Cbrt() * radius;
            return pointOnSphere * r;
        }

        /// <inheritdoc cref="PointInsideUnitSphere"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 PointInsideSphere(this Random random, Vector3 center, float radius) =>
            center + random.PointInsideSphere(radius);

        /// <inheritdoc cref="PointInsideUnitSphere"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 RandomPointInsideSphere(this float radius) =>
            GlobalRandom.Default.PointInsideSphere(radius);

        /// <inheritdoc cref="PointInsideUnitSphere"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 RandomPointInsideSphere(this Vector3 center, float radius) =>
            GlobalRandom.Default.PointInsideSphere(center, radius);
    }
}