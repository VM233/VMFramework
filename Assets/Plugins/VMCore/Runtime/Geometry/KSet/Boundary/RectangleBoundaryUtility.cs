using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class RectangleBoundaryUtility
    {
        #region Get Boundary

        /// <summary>
        /// 获取矩形的所有边界点
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangleBoundary GetBoundary<TMinMaxOwner>(this TMinMaxOwner owner)
            where TMinMaxOwner : IMinMaxOwner<Vector2Int> => new(owner.Min, owner.Max);

        /// <summary>
        /// 获取矩形的所有边界点
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangleBoundary GetBoundary(this Vector2Int min, Vector2Int max) => new(min, max);

        /// <summary>
        /// 获取特定尺寸的矩形的所有边界点
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangleBoundary GetBoundary(this Vector2Int size) =>
            new(Vector2Int.zero, size - Vector2Int.one);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangleBoundary GetInnerBoundary(this RectangleBoundary outerBoundary) =>
            new(outerBoundary.min + Vector2Int.one, outerBoundary.max - Vector2Int.one);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangleBoundary GetInnerBoundary(this Vector2Int min, Vector2Int max) =>
            new(min + Vector2Int.one, max - Vector2Int.one);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangleBoundary GetInnerBoundary(this Vector2Int size) =>
            new(Vector2Int.one, size - CommonVector2Int.two);

        #endregion
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int MinDistanceToBoundary(this Vector2Int point, RectangleBoundary boundary)
        {
            return point.MinDistanceToBoundary(boundary.min, boundary.max);
        }
    }
}