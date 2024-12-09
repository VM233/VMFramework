using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class RectangleUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangleInteger GetRectangle(this Vector2Int start, Vector2Int end)
        {
            return new RectangleInteger(start, end);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangleInteger GetRectangle(this Vector2Int size)
        {
            return new RectangleInteger(Vector2Int.zero, size - Vector2Int.one);
        }

        #region Inner

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangleInteger GetInnerRectangle(this RectangleInteger outer)
        {
            return new RectangleInteger(outer.min + Vector2Int.one, outer.max - Vector2Int.one);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangleInteger GetInnerRectangle(this Vector2Int start, Vector2Int end)
        {
            return new RectangleInteger(start + Vector2Int.one, end - Vector2Int.one);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangleInteger GetInnerRectangle(this Vector2Int size)
        {
            return new RectangleInteger(Vector2Int.one, size - CommonVector2Int.two);
        }

        #endregion

        #region Outer

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangleInteger GetOuterRectangle(this RectangleInteger inner)
        {
            return new RectangleInteger(inner.min - Vector2Int.one, inner.max + Vector2Int.one);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangleInteger GetOuterRectangle(this Vector2Int start, Vector2Int end)
        {
            return new RectangleInteger(start - Vector2Int.one, end + Vector2Int.one);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangleInteger GetOuterRectangle(this Vector2Int size)
        {
            return new RectangleInteger(Vector2Int.zero, size);
        }

        #endregion
        
        #region Get Boundary From Cluster

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectangleInteger GetMinRectangle(this IEnumerable<Vector2Int> points)
        {
            int xMax = int.MinValue;
            int xMin = int.MaxValue;
            int yMax = int.MinValue;
            int yMin = int.MaxValue;

            foreach (var point in points)
            {
                if (point.x > xMax)
                {
                    xMax = point.x;
                }

                if (point.x < xMin)
                {
                    xMin = point.x;
                }

                if (point.y > yMax)
                {
                    yMax = point.y;
                }

                if (point.y < yMin)
                {
                    yMin = point.y;
                }
            }

            return new RectangleInteger(xMin, yMin, xMax, yMax);
        }

        #endregion
    }
}