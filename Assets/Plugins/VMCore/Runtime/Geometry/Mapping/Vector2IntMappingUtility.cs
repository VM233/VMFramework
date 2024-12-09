using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class Vector2IntMappingUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FourDirectionsNeighbors<TResult> GetFourDirectionsNeighbors<TResult>(
            this IMapping<Vector2Int, TResult> mapping, Vector2Int point)
        {
            return point.GetFourDirectionsNeighbors().Map(mapping.MapTo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EightDirectionsNeighbors<TResult> GetEightDirectionsNeighbors<TResult>(
            this IMapping<Vector2Int, TResult> mapping, Vector2Int point)
        {
            return point.GetEightDirectionsNeighbors().Map(mapping.MapTo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EightDirectionsNeighbors<TResult> GetEightDirectionsNeighborsOnPlane<TResult>(
            this IMapping<Vector3Int, TResult> mapping, Vector3Int point, PlaneType planeType)
        {
            return point.GetEightDirectionsNeighborsOnPlane(planeType).Map(mapping.MapTo);
        }
    }
}