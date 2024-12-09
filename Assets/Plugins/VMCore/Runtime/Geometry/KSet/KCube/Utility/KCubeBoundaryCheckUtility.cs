using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class KCubeBoundaryCheckUtility
    {
        /// <summary>
        /// 判断一个点是否在K维立方体的边界上
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOnBoundary<TRange>(this TRange range, int pos)
            where TRange : IKCube<int>
        {
            return pos.IsOnBoundary(range.Min, range.Max);
        }

        /// <summary>
        /// 判断一个点是否在K维立方体的边界上
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOnBoundary<TRectangle>(this TRectangle rectangle, Vector2Int pos)
            where TRectangle : IKCube<Vector2Int>
        {
            return pos.IsOnBoundary(rectangle.Min, rectangle.Max);
        }

        /// <summary>
        /// 判断一个点是否在K维立方体的边界上
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOnBoundary<TCube>(this TCube cube, Vector3Int pos)
            where TCube : IKCube<Vector3Int>
        {
            return pos.IsOnBoundary(cube.Min, cube.Max);
        }
    }
}