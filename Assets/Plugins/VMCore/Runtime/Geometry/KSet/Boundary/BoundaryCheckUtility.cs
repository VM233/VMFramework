using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class BoundaryCheckUtility
    {
        #region Int

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOnBoundary(this int pos, int start, int end)
        {
            return pos == start || pos == end;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOnBoundary(this int pos, int start, int end, out LeftRightDirection direction)
        {
            direction = LeftRightDirection.None;

            if (pos == start)
            {
                direction |= LeftRightDirection.Left;
            }

            if (pos == end)
            {
                direction |= LeftRightDirection.Right;
            }

            return direction != LeftRightDirection.None;
        }

        #endregion
        
        #region Vector2Int
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOnBoundary(this Vector2Int pos, Vector2Int start, Vector2Int end)
        {
            return pos.x == start.x || pos.x == end.x || pos.y == start.y || pos.y == end.y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOnBoundary(this Vector2Int point, Vector2Int start, Vector2Int end,
            out FourTypesDirection boundaryDirection)
        {
            boundaryDirection = FourTypesDirection.None;

            if (point.x == start.x)
            {
                boundaryDirection = FourTypesDirection.Left;
            }

            if (point.x == end.x)
            {
                boundaryDirection = FourTypesDirection.Right;
            }

            if (point.y == start.y)
            {
                boundaryDirection |= FourTypesDirection.Down;
            }

            if (point.y == end.y)
            {
                boundaryDirection |= FourTypesDirection.Up;
            }

            return boundaryDirection != FourTypesDirection.None;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOnBoundary(this Vector2Int pos, Vector2Int start, Vector2Int end,
            FourTypesDirection boundaryDirection)
        {
            return boundaryDirection switch
            {
                FourTypesDirection.Left => pos.x == start.x,
                FourTypesDirection.Right => pos.x == end.x,
                FourTypesDirection.Up => pos.y == end.y,
                FourTypesDirection.Down => pos.y == start.y,
                _ => throw new InvalidEnumArgumentException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOnLeftBoundary(this Vector2Int pos, Vector2Int start, Vector2Int end)
        {
            return pos.x == start.x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOnRightBoundary(this Vector2Int pos, Vector2Int start, Vector2Int end)
        {
            return pos.x == end.x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOnTopBoundary(this Vector2Int pos, Vector2Int start, Vector2Int end)
        {
            return pos.y == end.y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOnBottomBoundary(this Vector2Int pos, Vector2Int start, Vector2Int end)
        {
            return pos.y == start.y;
        }
        
        #endregion

        #region Vector3Int

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOnBoundary(this Vector3Int pos, Vector3Int start, Vector3Int end)
        {
            return pos.x == start.x || pos.x == end.x || pos.y == start.y || pos.y == end.y || pos.z == start.z ||
                   pos.z == end.z;
        }

        #endregion
    }
}