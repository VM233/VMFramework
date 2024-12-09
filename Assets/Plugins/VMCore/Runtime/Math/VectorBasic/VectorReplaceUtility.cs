using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class VectorReplaceUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 ReplaceX(this Vector4 vector, float x)
        {
            return new(x, vector.y, vector.z, vector.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 ReplaceY(this Vector4 vector, float y)
        {
            return new(vector.x, y, vector.z, vector.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 ReplaceZ(this Vector4 vector, float z)
        {
            return new(vector.x, vector.y, z, vector.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 ReplaceW(this Vector4 vector, float w)
        {
            return new(vector.x, vector.y, vector.z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color ReplaceRed(this Color color, float red)
        {
            return new(red, color.g, color.b, color.a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color ReplaceGreen(this Color color, float green)
        {
            return new(color.r, green, color.b, color.a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color ReplaceBlue(this Color color, float blue)
        {
            return new(color.r, color.g, blue, color.a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color ReplaceAlpha(this Color color, float alpha)
        {
            return new(color.r, color.g, color.b, alpha);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ReplaceX(this Vector3 vector, float x)
        {
            return new(x, vector.y, vector.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ReplaceY(this Vector3 vector, float y)
        {
            return new(vector.x, y, vector.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ReplaceZ(this Vector3 vector, float z)
        {
            return new(vector.x, vector.y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int ReplaceX(this Vector3Int vector, int x)
        {
            return new(x, vector.y, vector.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int ReplaceY(this Vector3Int vector, int y)
        {
            return new(vector.x, y, vector.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int ReplaceZ(this Vector3Int vector, int z)
        {
            return new(vector.x, vector.y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ReplaceXY(this Vector3 vector, float x, float y)
        {
            return new(x, y, vector.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ReplaceYZ(this Vector3 vector, float y, float z)
        {
            return new(vector.x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ReplaceXZ(this Vector3 vector, float x, float z)
        {
            return new(x, vector.y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int ReplaceXY(this Vector3Int vector, int x, int y)
        {
            return new(x, y, vector.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int ReplaceYZ(this Vector3Int vector, int y, int z)
        {
            return new(vector.x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int ReplaceXZ(this Vector3Int vector, int x, int z)
        {
            return new(x, vector.y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ReplaceXY(this Vector3 vector, Vector2 xy)
        {
            return new(xy.x, xy.y, vector.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ReplaceYZ(this Vector3 vector, Vector2 yz)
        {
            return new(vector.x, yz.x, yz.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ReplaceXZ(this Vector3 vector, Vector2 xz)
        {
            return new(xz.x, vector.y, xz.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int ReplaceXY(this Vector3Int vector, Vector2Int xy)
        {
            return new(xy.x, xy.y, vector.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int ReplaceYZ(this Vector3Int vector, Vector2Int yz)
        {
            return new(vector.x, yz.x, yz.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int ReplaceXZ(this Vector3Int vector, Vector2Int xz)
        {
            return new(xz.x, vector.y, xz.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ReplaceX(this Vector2 vector, float x)
        {
            return new(x, vector.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ReplaceY(this Vector2 vector, float y)
        {
            return new(vector.x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int ReplaceX(this Vector2Int vector, int x)
        {
            return new(x, vector.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int ReplaceY(this Vector2Int vector, int y)
        {
            return new(vector.x, y);
        }
    }
}