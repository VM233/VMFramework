using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class VectorInsertUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int InsertAsX(this Vector2Int vector, int x)
        {
            return new(x, vector.x, vector.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int InsertAsY(this Vector2Int vector, int y)
        {
            return new(vector.x, y, vector.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int InsertAsZ(this Vector2Int vector, int z)
        {
            return new(vector.x, vector.y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int InsertAs(this Vector2Int vector2, int extraNum,
            PlaneType planeType)
        {
            return planeType switch
            {
                PlaneType.XY => vector2.InsertAsZ(extraNum),
                PlaneType.XZ => vector2.InsertAsY(extraNum),
                PlaneType.YZ => vector2.InsertAsX(extraNum),
                _ => throw new ArgumentOutOfRangeException(nameof(planeType),
                    planeType, null)
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 InsertAsX(this Vector2 vector, float x)
        {
            return new(x, vector.x, vector.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 InsertAsY(this Vector2 vector, float y)
        {
            return new(vector.x, y, vector.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 InsertAsZ(this Vector2 vector, float z)
        {
            return new(vector.x, vector.y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 InsertAs(this Vector2 vector2, float extraNum,
            PlaneType planeType)
        {
            return planeType switch
            {
                PlaneType.XY => vector2.InsertAsZ(extraNum),
                PlaneType.XZ => vector2.InsertAsY(extraNum),
                PlaneType.YZ => vector2.InsertAsX(extraNum),
                _ => throw new ArgumentOutOfRangeException(nameof(planeType),
                    planeType, null)
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int As3DXY(this Vector2Int vector)
        {
            return vector.InsertAsZ(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int As3DXZ(this Vector2Int vector)
        {
            return vector.InsertAsY(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int As3DYZ(this Vector2Int vector)
        {
            return vector.InsertAsX(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 As3DXY(this Vector2 vector)
        {
            return vector.InsertAsZ(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 As3DXZ(this Vector2 vector)
        {
            return vector.InsertAsY(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 As3DYZ(this Vector2 vector)
        {
            return vector.InsertAsX(0);
        }
    }
}