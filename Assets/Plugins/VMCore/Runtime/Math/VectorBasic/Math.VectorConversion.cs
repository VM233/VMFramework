using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static partial class Math
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 To3D(this Color color)
        {
            return new(color.r, color.g, color.b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 To4D(this Color color)
        {
            return color;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 To3D(this Vector2 vector)
        {
            return vector;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 To4D(this Vector2 vector)
        {
            return new(vector.x, vector.y, 0, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int To3D(this Vector2Int vector)
        {
            return (Vector3Int)vector;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 To2D(this Vector3 vector)
        {
            return vector;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int To2D(this Vector3Int vector)
        {
            return (Vector2Int)vector;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 F(this Vector3Int vector)
        {
            return vector;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 F(this Vector2Int vector)
        {
            return vector;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 To2DF(this Vector3Int vector)
        {
            return vector.To2D();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 To3DF(this Vector2Int vector)
        {
            return vector.To3D();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 To4DF(this Vector2Int vector)
        {
            return new(vector.x, vector.y, 0, 0);
        }
    }
}
