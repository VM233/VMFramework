using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class GeometryVectorUtility
    {
        #region Range

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> GetCircularRangeOfPoints(this int start, int end, int count)
        {
            int result = start;
            for (int i = 0; i < count; i++)
            {
                yield return result;
                result++;
                if (result > end)
                {
                    result = start;
                }
            }
        }

        #endregion

        #region Rectangle

        #region Get Points On Boundary From Cluster

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Vector2Int> GetLeftmostPoints(this IEnumerable<Vector2Int> points)
        {
            return points.GroupBy(point => point.y).Select(group => group.SelectMin(pos => pos.x));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Vector2Int> GetRightmostPoints(this IEnumerable<Vector2Int> points)
        {
            return points.GroupBy(point => point.y).Select(group => group.SelectMax(pos => pos.x));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Vector2Int> GetTopmostPoints(this IEnumerable<Vector2Int> points)
        {
            return points.GroupBy(point => point.x).Select(group => group.SelectMax(pos => pos.y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Vector2Int> GetBottommostPoints(this IEnumerable<Vector2Int> points)
        {
            return points.GroupBy(point => point.x).Select(group => group.SelectMin(pos => pos.y));
        }

        #endregion

        #endregion

        #region Cube

        #region Face Points

        /// <summary>
        /// 获取立方体面上的点
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="faceType"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Vector3Int> GetFacePointsOfCube(this Vector3Int start, Vector3Int end,
            FaceType faceType)
        {
            // 获取立方体右面（x = end.x）的点
            if (faceType.HasFlag(FaceType.Right))
            {
                for (int y = start.y; y <= end.y; y++)
                {
                    for (int z = start.z; z <= end.z; z++)
                    {
                        yield return new(end.x, y, z);
                    }
                }
            }

            // 获取立方体左面（x = start.x）的点
            if (faceType.HasFlag(FaceType.Left))
            {
                for (int y = start.y; y <= end.y; y++)
                {
                    for (int z = start.z; z <= end.z; z++)
                    {
                        yield return new(start.x, y, z);
                    }
                }
            }

            // 获取立方体上面（y = end.y）的点
            if (faceType.HasFlag(FaceType.Up))
            {
                for (int x = start.x; x <= end.x; x++)
                {
                    for (int z = start.z; z <= end.z; z++)
                    {
                        yield return new(x, end.y, z);
                    }
                }
            }

            // 获取立方体下面（y = start.y）的点
            if (faceType.HasFlag(FaceType.Down))
            {
                for (int x = start.x; x <= end.x; x++)
                {
                    for (int z = start.z; z <= end.z; z++)
                    {
                        yield return new(x, start.y, z);
                    }
                }
            }

            // 获取立方体前面（z = end.z）的点
            if (faceType.HasFlag(FaceType.Forward))
            {
                for (int x = start.x; x <= end.x; x++)
                {
                    for (int y = start.y; y <= end.y; y++)
                    {
                        yield return new(x, y, end.z);
                    }
                }
            }

            // 获取立方体后面（z = start.z）的点
            if (faceType.HasFlag(FaceType.Back))
            {
                for (int x = start.x; x <= end.x; x++)
                {
                    for (int y = start.y; y <= end.y; y++)
                    {
                        yield return new(x, y, start.z);
                    }
                }
            }
        }

        /// <summary>
        /// 获取立方体面上的点
        /// </summary>
        /// <param name="size"></param>
        /// <param name="faceType"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Vector3Int> GetFacePointsOfCube(this Vector3Int size, FaceType faceType)
        {
            return GetFacePointsOfCube(Vector3Int.zero, size - Vector3Int.one, faceType);
        }

        #endregion

        #region Inner Face Points

        /// <summary>
        /// 获取立方体面上的点，但不包括边界
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="faceType"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Vector3Int> GetInnerFacePointsOfCube(this Vector3Int start, Vector3Int end,
            FaceType faceType)
        {
            // 获取立方体右面（x = end.x）的点
            if (faceType.HasFlag(FaceType.Right))
            {
                for (int y = start.y + 1; y < end.y; y++)
                {
                    for (int z = start.z + 1; z < end.z; z++)
                    {
                        yield return new(end.x, y, z);
                    }
                }
            }

            // 获取立方体左面（x = start.x）的点
            if (faceType.HasFlag(FaceType.Left))
            {
                for (int y = start.y + 1; y < end.y; y++)
                {
                    for (int z = start.z + 1; z < end.z; z++)
                    {
                        yield return new(start.x, y, z);
                    }
                }
            }

            // 获取立方体上面（y = end.y）的点
            if (faceType.HasFlag(FaceType.Up))
            {
                for (int x = start.x + 1; x < end.x; x++)
                {
                    for (int z = start.z + 1; z < end.z; z++)
                    {
                        yield return new(x, end.y, z);
                    }
                }
            }

            // 获取立方体下面（y = start.y）的点
            if (faceType.HasFlag(FaceType.Down))
            {
                for (int x = start.x + 1; x < end.x; x++)
                {
                    for (int z = start.z + 1; z < end.z; z++)
                    {
                        yield return new(x, start.y, z);
                    }
                }
            }

            // 获取立方体前面（z = end.z）的点
            if (faceType.HasFlag(FaceType.Forward))
            {
                for (int x = start.x + 1; x < end.x; x++)
                {
                    for (int y = start.y + 1; y < end.y; y++)
                    {
                        yield return new(x, y, end.z);
                    }
                }
            }

            // 获取立方体后面（z = start.z）的点
            if (faceType.HasFlag(FaceType.Back))
            {
                for (int x = start.x + 1; x < end.x; x++)
                {
                    for (int y = start.y + 1; y < end.y; y++)
                    {
                        yield return new(x, y, start.z);
                    }
                }
            }
        }

        /// <summary>
        /// 获取立方体面上的点，但不包括边界
        /// </summary>
        /// <param name="size"></param>
        /// <param name="faceType"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Vector3Int> GetInnerFacePointsOfCube(this Vector3Int size, FaceType faceType)
        {
            return GetInnerFacePointsOfCube(Vector3Int.zero, size - Vector3Int.one, faceType);
        }

        #endregion

        #region All Inner Face Points

        /// <summary>
        /// 获取所有立方体面上的点，且不包括边界
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Vector3Int> GetAllInnerFacePointsOfCube(this Vector3Int start, Vector3Int end)
        {
            for (int y = start.y + 1; y < end.y; y++)
            {
                for (int z = start.z + 1; z < end.z; z++)
                {
                    yield return new(end.x, y, z);
                }
            }

            for (int y = start.y + 1; y < end.y; y++)
            {
                for (int z = start.z + 1; z < end.z; z++)
                {
                    yield return new(start.x, y, z);
                }
            }

            for (int x = start.x + 1; x < end.x; x++)
            {
                for (int z = start.z + 1; z < end.z; z++)
                {
                    yield return new(x, end.y, z);
                }
            }

            for (int x = start.x + 1; x < end.x; x++)
            {
                for (int z = start.z + 1; z < end.z; z++)
                {
                    yield return new(x, start.y, z);
                }
            }

            for (int x = start.x + 1; x < end.x; x++)
            {
                for (int y = start.y + 1; y < end.y; y++)
                {
                    yield return new(x, y, end.z);
                }
            }

            for (int x = start.x + 1; x < end.x; x++)
            {
                for (int y = start.y + 1; y < end.y; y++)
                {
                    yield return new(x, y, start.z);
                }
            }

        }

        /// <summary>
        /// 获取所有立方体面上的点，且不包括边界
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Vector3Int> GetAllInnerFacePointsOfCube(this Vector3Int size)
        {
            return GetAllInnerFacePointsOfCube(Vector3Int.zero, size - Vector3Int.one);
        }

        #endregion

        #region All Face Points

        /// <summary>
        /// 获取立方体所有面上的点
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Vector3Int> GetAllFacePointsOfCube(this Vector3Int start, Vector3Int end)
        {
            return GetAllInnerFacePointsOfCube(start, end).Concat(GetAllEdgePointsOfCube(start, end));
        }

        /// <summary>
        /// 获取立方体所有面上的点
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Vector3Int> GetAllFacePointsOfCube(this Vector3Int size)
        {
            return GetAllInnerFacePointsOfCube(size).Concat(GetAllEdgePointsOfCube(size));
        }

        #endregion

        #region All Edge Points

        /// <summary>
        /// 获取立方体边界上的点
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Vector3Int> GetAllEdgePointsOfCube(this Vector3Int start, Vector3Int end)
        {
            if (start.x != end.x)
            {
                if (start.y != end.y)
                {
                    for (int z = start.z; z <= end.z; z++)
                    {
                        yield return new(start.x, start.y, z);
                        yield return new(start.x, end.y, z);
                        yield return new(end.x, start.y, z);
                        yield return new(end.x, end.y, z);
                    }

                    for (int x = start.x + 1; x < end.x; x++)
                    {
                        yield return new(x, start.y, start.z);
                        yield return new(x, start.y, end.z);
                        yield return new(x, end.y, start.z);
                        yield return new(x, end.y, end.z);
                    }

                    for (int y = start.y + 1; y < end.y; y++)
                    {
                        yield return new(start.x, y, start.z);
                        yield return new(start.x, y, end.z);
                        yield return new(end.x, y, start.z);
                        yield return new(end.x, y, end.z);
                    }
                }
                else
                {
                    for (int z = start.z; z <= end.z; z++)
                    {
                        yield return new(start.x, start.y, z);
                        yield return new(end.x, start.y, z);
                    }

                    for (int x = start.x + 1; x < end.x; x++)
                    {
                        yield return new(x, start.y, start.z);
                        yield return new(x, start.y, end.z);
                    }
                }
            }
            else
            {
                if (start.y != end.y)
                {
                    for (int z = start.z; z <= end.z; z++)
                    {
                        yield return new(start.x, start.y, z);
                        yield return new(start.x, end.y, z);
                    }

                    for (int y = start.y + 1; y < end.y; y++)
                    {
                        yield return new(start.x, y, start.z);
                        yield return new(start.x, y, end.z);
                    }
                }
                else
                {
                    for (int z = start.z; z <= end.z; z++)
                    {
                        yield return new(start.x, start.y, z);
                    }
                }
            }
        }

        /// <summary>
        /// 获取立方体边界上的点
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Vector3Int> GetAllEdgePointsOfCube(this Vector3Int size)
        {
            return GetAllEdgePointsOfCube(Vector3Int.zero, size - Vector3Int.one);
        }

        #endregion

        #region Edge Points

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public static IEnumerable<Vector3Int> GetEdgePointsOfCube(
        //    this Vector3Int start, Vector3Int end, EdgeType edgeType)
        //{
        //    if (edgeType.HasFlag(EdgeType.))
        //} 

        #endregion

        #endregion

        #region Circle

        #region Get Points

        /// <summary>
        /// 获取以pivot为中心，半径为radius的圆上的点
        /// </summary>
        /// <param name="pivot"></param>
        /// <param name="radius"></param>
        /// <param name="distanceType"></param>
        /// <param name="planeType"></param>
        /// <returns></returns>
        /// <exception cref="InvalidEnumArgumentException"></exception>
        public static IEnumerable<Vector3Int> GetPointsOfCircle(this Vector3Int pivot, float radius,
            DistanceType distanceType = DistanceType.Manhattan, PlaneType planeType = PlaneType.XY)
        {
            return distanceType switch
            {
                DistanceType.Manhattan => pivot.GetPointsOfManhattanCircle(radius, planeType),
                DistanceType.Euclidean => pivot.GetPointsOfEuclideanCircle(radius, planeType),
                _ => throw new InvalidEnumArgumentException(nameof(distanceType))
            };
        }

        /// <summary>
        /// 以pivot为中心，获取曼哈顿距离不超过radius的点
        /// </summary>
        /// <param name="pivot"></param>
        /// <param name="radius"></param>
        /// <param name="planeType"></param>
        /// <returns></returns>
        public static IEnumerable<Vector3Int> GetPointsOfManhattanCircle(this Vector3Int pivot, float radius,
            PlaneType planeType = PlaneType.XY)
        {
            switch (planeType)
            {
                case PlaneType.XY:
                    foreach (var pos in GetPointsOfManhattanCircle(new(pivot.x, pivot.y), radius))
                    {
                        yield return new(pos.x, pos.y, pivot.z);
                    }

                    break;
                case PlaneType.XZ:
                    foreach (var pos in GetPointsOfManhattanCircle(new(pivot.x, pivot.z), radius))
                    {
                        yield return new(pos.x, pivot.y, pos.y);
                    }

                    break;
                case PlaneType.YZ:
                    foreach (var pos in GetPointsOfManhattanCircle(new(pivot.y, pivot.z), radius))
                    {
                        yield return new(pivot.x, pos.x, pos.y);
                    }

                    break;
            }
        }

        /// <summary>
        /// 以pivot为中心，获取曼哈顿距离不超过radius的点
        /// </summary>
        /// <param name="pivot"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static IEnumerable<Vector2Int> GetPointsOfManhattanCircle(this Vector2Int pivot, float radius)
        {
            if (radius < 0)
            {
                yield break;
            }

            int r = Mathf.FloorToInt(radius);

            for (int l = 0; l < r; l++)
            {
                int py = pivot.y + (r - l);
                int ny = pivot.y - (r - l);
                for (int x = pivot.x - l; x <= pivot.x + l; x++)
                {
                    yield return new(x, py);
                    yield return new(x, ny);
                }
            }

            for (int x = pivot.x - r; x <= pivot.x + r; x++)
            {
                yield return new(x, pivot.y);
            }
        }

        /// <summary>
        /// 以pivot为中心，获取欧氏距离不超过radius的点
        /// </summary>
        /// <param name="pivot"></param>
        /// <param name="radius"></param>
        /// <param name="planeType"></param>
        /// <returns></returns>
        public static IEnumerable<Vector3Int> GetPointsOfEuclideanCircle(this Vector3Int pivot, float radius,
            PlaneType planeType = PlaneType.XY)
        {
            switch (planeType)
            {
                case PlaneType.XY:
                    foreach (var pos in GetPointsOfEuclideanCircle(new(pivot.x, pivot.y), radius))
                    {
                        yield return new(pos.x, pos.y, pivot.z);
                    }

                    break;
                case PlaneType.XZ:
                    foreach (var pos in GetPointsOfEuclideanCircle(new(pivot.x, pivot.z), radius))
                    {
                        yield return new(pos.x, pivot.y, pos.y);
                    }

                    break;
                case PlaneType.YZ:
                    foreach (var pos in GetPointsOfEuclideanCircle(new(pivot.y, pivot.z), radius))
                    {
                        yield return new(pivot.x, pos.x, pos.y);
                    }

                    break;
            }
        }

        /// <summary>
        /// 以pivot为中心，获取欧氏距离不超过radius的点
        /// </summary>
        /// <param name="pivot"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static IEnumerable<Vector2Int> GetPointsOfEuclideanCircle(this Vector2Int pivot, float radius)
        {
            if (radius < 0)
            {
                yield break;
            }

            foreach (var pos in GetPointsOfManhattanCircle(pivot, radius))
            {
                yield return pos;
            }

            int r = radius.Floor();

            List<Vector2Int> quarterCircle = new();

            for (int l = 0; l < r; l++)
            {
                int y = pivot.y + l + 1;
                int xMax = pivot.x + r;
                for (int x = pivot.x + r - l; x <= xMax; x++)
                {
                    Vector2Int newPos = new(x, y);
                    if (newPos.EuclideanDistance(pivot) <= radius)
                    {
                        quarterCircle.Add(newPos);
                    }
                }
            }

            foreach (var quarterPos in quarterCircle)
            {
                foreach (var pos in quarterPos.Symmetric(pivot))
                {
                    yield return pos;
                }
            }
        }

        #endregion

        #region Uniformly Spaced Angles & Directions

        /// <summary>
        /// 获取一个圆上均匀分布的角度
        /// </summary>
        /// <param name="stepCount"></param>
        /// <param name="startAngle"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniformlySpacedRangeFloat GetUniformlySpacedAnglesOfCircle(this int stepCount,
            float startAngle = 0f)
        {
            return new UniformlySpacedRangeFloat(startAngle, startAngle + 360f, stepCount);
        }

        /// <summary>
        /// 获取一个圆上均匀分布的方向, 0度为向右，顺时针旋转
        /// </summary>
        /// <param name="stepCount"></param>
        /// <param name="startAngle"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Vector2> GetUniformlySpacedDirectionsOfCircle(this int stepCount,
            float startAngle = 0f)
        {
            foreach (var angle in stepCount.GetUniformlySpacedAnglesOfCircle())
            {
                yield return angle.ClockwiseAngleToDirection();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Vector2> GetUniformlySpacedClockwiseAngleDirections(this float startAngle,
            float endAngle, int stepCount)
        {
            foreach (var angle in startAngle.UniformlySpaced(endAngle, stepCount))
            {
                yield return angle.ClockwiseAngleToDirection();
            }
        }

        #endregion

        #endregion

        #region Near Points

        #region Vector3 Int

        /// <summary>
        /// 获取一个三维坐标的直接相邻或者斜角相邻的二十六个方向的相邻点
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Vector3Int> GetTwentySixDirectionsNearPoints(this Vector3Int pos)
        {
            yield return new(pos.x + 1, pos.y, pos.z);
            yield return new(pos.x - 1, pos.y, pos.z);
            yield return new(pos.x, pos.y + 1, pos.z);
            yield return new(pos.x, pos.y - 1, pos.z);
            yield return new(pos.x, pos.y, pos.z + 1);
            yield return new(pos.x, pos.y, pos.z - 1);
            yield return new(pos.x + 1, pos.y + 1, pos.z);
            yield return new(pos.x + 1, pos.y - 1, pos.z);
            yield return new(pos.x - 1, pos.y + 1, pos.z);
            yield return new(pos.x - 1, pos.y - 1, pos.z);
            yield return new(pos.x + 1, pos.y, pos.z + 1);
            yield return new(pos.x + 1, pos.y, pos.z - 1);
            yield return new(pos.x - 1, pos.y, pos.z + 1);
            yield return new(pos.x - 1, pos.y, pos.z - 1);
            yield return new(pos.x, pos.y + 1, pos.z + 1);
            yield return new(pos.x, pos.y + 1, pos.z - 1);
            yield return new(pos.x, pos.y - 1, pos.z + 1);
            yield return new(pos.x, pos.y - 1, pos.z - 1);
            yield return new(pos.x + 1, pos.y + 1, pos.z + 1);
            yield return new(pos.x + 1, pos.y + 1, pos.z - 1);
            yield return new(pos.x + 1, pos.y - 1, pos.z + 1);
            yield return new(pos.x + 1, pos.y - 1, pos.z - 1);
            yield return new(pos.x - 1, pos.y + 1, pos.z + 1);
            yield return new(pos.x - 1, pos.y + 1, pos.z - 1);
            yield return new(pos.x - 1, pos.y - 1, pos.z + 1);
        }

        /// <summary>
        /// 获取一个三维坐标的直接相邻的八个方向的相邻点，需要提供平面类型（XY平面，XZ平面，YZ平面）
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="planeType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Vector3Int> GetEightDirectionsNearPoints(this Vector3Int pos, PlaneType planeType)
        {
            switch (planeType)
            {
                case PlaneType.XY:
                    yield return new(pos.x + 1, pos.y, pos.z);
                    yield return new(pos.x - 1, pos.y, pos.z);
                    yield return new(pos.x, pos.y + 1, pos.z);
                    yield return new(pos.x, pos.y - 1, pos.z);
                    yield return new(pos.x + 1, pos.y + 1, pos.z);
                    yield return new(pos.x + 1, pos.y - 1, pos.z);
                    yield return new(pos.x - 1, pos.y + 1, pos.z);
                    yield return new(pos.x - 1, pos.y - 1, pos.z);
                    break;
                case PlaneType.XZ:
                    yield return new(pos.x + 1, pos.y, pos.z);
                    yield return new(pos.x - 1, pos.y, pos.z);
                    yield return new(pos.x, pos.y, pos.z + 1);
                    yield return new(pos.x, pos.y, pos.z - 1);
                    yield return new(pos.x + 1, pos.y, pos.z + 1);
                    yield return new(pos.x + 1, pos.y, pos.z - 1);
                    yield return new(pos.x - 1, pos.y, pos.z + 1);
                    yield return new(pos.x - 1, pos.y, pos.z - 1);
                    break;
                case PlaneType.YZ:
                    yield return new(pos.x, pos.y + 1, pos.z);
                    yield return new(pos.x, pos.y - 1, pos.z);
                    yield return new(pos.x, pos.y, pos.z + 1);
                    yield return new(pos.x, pos.y, pos.z - 1);
                    yield return new(pos.x, pos.y + 1, pos.z + 1);
                    yield return new(pos.x, pos.y + 1, pos.z - 1);
                    yield return new(pos.x, pos.y - 1, pos.z + 1);
                    yield return new(pos.x, pos.y - 1, pos.z - 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(planeType), planeType, null);
            }
        }

        #endregion

        #region Cube

        /// <summary>
        /// 获取立方体的直接相邻外部点（不包括八个角）
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Vector3Int> GetCubeNearPoints(this Vector3Int start, Vector3Int end)
        {
            return GetAllInnerFacePointsOfCube(start - Vector3Int.one, end + Vector3Int.one);
        }

        #endregion

        #endregion
    }
}