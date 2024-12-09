using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class BoundaryMinDistanceUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int MinDistanceToBoundary(this int pos, int start, int end)
        {
            return pos.MinDistance(start, end);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int MinDistanceToBoundary(this Vector2Int pos, Vector2Int start, Vector2Int end)
        {
            int minXDistance = pos.x.MinDistance(start.x, end.x);

            if (minXDistance == 0) return 0;

            int minYDistance = pos.y.MinDistance(start.y, end.y);

            return minXDistance.Min(minYDistance);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int MinDistanceToBoundary(this Vector3Int pos, Vector3Int start, Vector3Int end)
        {
            int minXDistance = pos.x.MinDistance(start.x, end.x);

            if (minXDistance == 0) return 0;

            int minYDistance = pos.y.MinDistance(start.y, end.y);

            if (minYDistance == 0) return 0;

            int minZDistance = pos.z.MinDistance(start.z, end.z);

            return Mathf.Min(minXDistance, minYDistance, minZDistance);
        }
    }
}