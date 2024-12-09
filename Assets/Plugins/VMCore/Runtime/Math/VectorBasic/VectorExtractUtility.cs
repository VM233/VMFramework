using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class VectorExtractUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int XZ(this Vector3Int vector)
        {
            return new(vector.x, vector.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int XY(this Vector3Int vector)
        {
            return new(vector.x, vector.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int YZ(this Vector3Int vector)
        {
            return new(vector.y, vector.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 XZ(this Vector3 vector)
        {
            return new(vector.x, vector.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 XY(this Vector3 vector)
        {
            return new(vector.x, vector.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 YZ(this Vector3 vector)
        {
            return new(vector.y, vector.z);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ExtractAs(this Vector3 vector3, PlaneType planeType)
        {
            return planeType switch
            {
                PlaneType.XY => new(vector3.x, vector3.y),
                PlaneType.XZ => new(vector3.x, vector3.z),
                PlaneType.YZ => new(vector3.y, vector3.z),
                _ => throw new ArgumentOutOfRangeException(nameof(planeType),
                    planeType, null)
            };
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int ExtractAs(this Vector3Int vector3, PlaneType planeType)
        {
            return planeType switch
            {
                PlaneType.XY => new(vector3.x, vector3.y),
                PlaneType.XZ => new(vector3.x, vector3.z),
                PlaneType.YZ => new(vector3.y, vector3.z),
                _ => throw new ArgumentOutOfRangeException(nameof(planeType),
                    planeType, null)
            };
        }
    }
}