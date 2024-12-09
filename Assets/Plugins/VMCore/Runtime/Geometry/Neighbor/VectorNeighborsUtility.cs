using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class VectorNeighborsUtility
    {
        #region Left Right Neighbors

        /// <summary>
        /// Get the left and right neighbors of an integer.
        /// That is, the integer - 1 and the integer + 1.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LeftRightNeighbors<int> GetNeighbors(this int point)
        {
            return new(point - 1, point + 1);
        }

        /// <summary>
        /// Get the left and right neighbors of a float.
        /// That is, the float - 1 and the float + 1.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LeftRightNeighbors<float> GetNeighbors(this float point)
        {
            return new(point - 1, point + 1);
        }

        #endregion

        #region Four Directions Neighbors

        /// <summary>
        /// Get the left, right, up, and down neighbors of a Vector2Int,
        /// that is, the Vector2Int with x - 1, x + 1, y - 1, and y + 1.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FourDirectionsNeighbors<Vector2Int> GetFourDirectionsNeighbors(this Vector2Int point)
        {
            return new(point + Vector2Int.left, point + Vector2Int.right, point + Vector2Int.up,
                point + Vector2Int.down);
        }

        /// <summary>
        /// Get the left, right, up, and down neighbors of a Vector2,
        /// that is, the Vector2 with x - 1, x + 1, y - 1, and y + 1.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FourDirectionsNeighbors<Vector2> GetFourDirectionsNeighbors(this Vector2 point)
        {
            return new(point + Vector2.left, point + Vector2.right, point + Vector2.up, point + Vector2.down);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FourDirectionsNeighbors<Vector3Int> GetFourDirectionsNeighborsOnXYPlane(this Vector3Int point)
        {
            return new(point + Vector3Int.left, point + Vector3Int.right, point + Vector3Int.up,
                point + Vector3Int.down);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FourDirectionsNeighbors<Vector3Int> GetFourDirectionsNeighborsOnXZPlane(this Vector3Int point)
        {
            return new(point + Vector3Int.left, point + Vector3Int.right, point + Vector3Int.back,
                point + Vector3Int.forward);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FourDirectionsNeighbors<Vector3Int> GetFourDirectionsNeighborsOnYZPlane(this Vector3Int point)
        {
            return new(point + Vector3Int.forward, point + Vector3Int.back, point + Vector3Int.up,
                point + Vector3Int.down);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FourDirectionsNeighbors<Vector3Int> GetFourDirectionsNeighborsOnPlane(this Vector3Int point,
            PlaneType planeType)
        {
            return planeType switch
            {
                PlaneType.XY => GetFourDirectionsNeighborsOnXYPlane(point),
                PlaneType.XZ => GetFourDirectionsNeighborsOnXZPlane(point),
                PlaneType.YZ => GetFourDirectionsNeighborsOnYZPlane(point),
                _ => throw new ArgumentOutOfRangeException(nameof(planeType), planeType, null)
            };
        }

        #endregion

        #region Eight Directions Neighbors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EightDirectionsNeighbors<Vector2Int> GetEightDirectionsNeighbors(this Vector2Int point)
        {
            return new(point + Vector2Int.left, point + Vector2Int.right, point + Vector2Int.up,
                point + Vector2Int.down, point + CommonVector2Int.upLeft, point + CommonVector2Int.upRight,
                point + CommonVector2Int.downLeft, point + CommonVector2Int.downRight);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EightDirectionsNeighbors<Vector2> GetEightDirectionsNeighbors(this Vector2 point)
        {
            return new(point + Vector2.left, point + Vector2.right, point + Vector2.up, point + Vector2.down,
                point + CommonVector2.upLeft, point + CommonVector2.upRight, point + CommonVector2.downLeft,
                point + CommonVector2.downRight);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EightDirectionsNeighbors<Vector3Int> GetEightDirectionsNeighborsOnXYPlane(this Vector3Int point)
        {
            return new(point + Vector3Int.left, point + Vector3Int.right, point + Vector3Int.up,
                point + Vector3Int.down, point + CommonVector3Int.upLeft, point + CommonVector3Int.upRight,
                point + CommonVector3Int.downLeft, point + CommonVector3Int.downRight);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EightDirectionsNeighbors<Vector3Int> GetEightDirectionsNeighborsOnXZPlane(this Vector3Int point)
        {
            return new(point + Vector3Int.left, point + Vector3Int.right, point + Vector3Int.back,
                point + Vector3Int.forward, point + CommonVector3Int.backLeft,
                point + CommonVector3Int.backRight, point + CommonVector3Int.forwardLeft,
                point + CommonVector3Int.forwardRight);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EightDirectionsNeighbors<Vector3Int> GetEightDirectionsNeighborsOnYZPlane(this Vector3Int point)
        {
            return new(point + Vector3Int.forward, point + Vector3Int.back, point + Vector3Int.up,
                point + Vector3Int.down, point + CommonVector3Int.upForward, point + CommonVector3Int.upBack,
                point + CommonVector3Int.downForward, point + CommonVector3Int.downBack);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EightDirectionsNeighbors<Vector3Int> GetEightDirectionsNeighborsOnPlane(this Vector3Int point,
            PlaneType planeType)
        {
            return planeType switch
            {
                PlaneType.XY => GetEightDirectionsNeighborsOnXYPlane(point),
                PlaneType.XZ => GetEightDirectionsNeighborsOnXZPlane(point),
                PlaneType.YZ => GetEightDirectionsNeighborsOnYZPlane(point),
                _ => throw new ArgumentOutOfRangeException(nameof(planeType), planeType, null)
            };
        }

        #endregion

        #region Six Directions Neighbors

        /// <summary>
        /// Get the left, right, up, down, forward, and back neighbors of a Vector3Int,
        /// that is, the Vector3Int with x - 1, x + 1, y - 1, y + 1, z - 1, and z + 1.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SixDirectionsNeighbors<Vector3Int> GetSixDirectionsNeighbors(this Vector3Int point)
        {
            return new(point + Vector3Int.left, point + Vector3Int.right, point + Vector3Int.up,
                point + Vector3Int.down, point + Vector3Int.forward, point + Vector3Int.back);
        }

        /// <summary>
        /// Get the left, right, up, down, forward, and back neighbors of a Vector3,
        /// that is, the Vector3 with x - 1, x + 1, y - 1, y + 1, z - 1, and z + 1.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SixDirectionsNeighbors<Vector3> GetSixDirectionsNeighbors(this Vector3 point)
        {
            return new(point + Vector3.left, point + Vector3.right, point + Vector3.up, point + Vector3.down,
                point + Vector3.forward, point + Vector3.back);
        }

        #endregion
    }
}