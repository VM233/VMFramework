using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class VectorAddUtility
    {
        #region Add 1

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 AddX(this Vector2 vector, float x)
        {
            return new(vector.x + x, vector.y);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 AddY(this Vector2 vector, float y)
        {
            return new(vector.x, vector.y + y);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int AddX(this Vector2Int vector, int x)
        {
            return new(vector.x + x, vector.y);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int AddY(this Vector2Int vector, int y)
        {
            return new(vector.x, vector.y + y);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 AddX(this Vector3 vector, float x)
        {
            return new(vector.x + x, vector.y, vector.z);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 AddY(this Vector3 vector, float y)
        {
            return new(vector.x, vector.y + y, vector.z);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 AddZ(this Vector3 vector, float z)
        {
            return new(vector.x, vector.y, vector.z + z);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int AddX(this Vector3Int vector, int x)
        {
            return new(vector.x + x, vector.y, vector.z);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int AddY(this Vector3Int vector, int y)
        {
            return new(vector.x, vector.y + y, vector.z);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int AddZ(this Vector3Int vector, int z)
        {
            return new(vector.x, vector.y, vector.z + z);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 AddX(this Vector4 vector, float x)
        {
            return new(vector.x + x, vector.y, vector.z, vector.w);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 AddY(this Vector4 vector, float y)
        {
            return new(vector.x, vector.y + y, vector.z, vector.w);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 AddZ(this Vector4 vector, float z)
        {
            return new(vector.x, vector.y, vector.z + z, vector.w);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 AddW(this Vector4 vector, float w)
        {
            return new(vector.x, vector.y, vector.z, vector.w + w);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color AddRed(this Color color, float red)
        {
            return new(color.r + red, color.g, color.b, color.a);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color AddGreen(this Color color, float green)
        {
            return new(color.r, color.g + green, color.b, color.a);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color AddBlue(this Color color, float blue)
        {
            return new(color.r, color.g, color.b + blue, color.a);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color AddAlpha(this Color color, float alpha)
        {
            return new(color.r, color.g, color.b, color.a + alpha);
        }

        #endregion

        #region Add 2

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 AddXY(this Vector3 vector, Vector2 xy)
        {
            return new(vector.x + xy.x, vector.y + xy.y, vector.z);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 AddXZ(this Vector3 vector, Vector2 xz)
        {
            return new(vector.x + xz.x, vector.y, vector.z + xz.y);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 AddYZ(this Vector3 vector, Vector2 yz)
        {
            return new(vector.x, vector.y + yz.x, vector.z + yz.y);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int AddXY(this Vector3Int vector, Vector2Int xy)
        {
            return new(vector.x + xy.x, vector.y + xy.y, vector.z);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int AddXZ(this Vector3Int vector, Vector2Int xz)
        {
            return new(vector.x + xz.x, vector.y, vector.z + xz.y);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int AddYZ(this Vector3Int vector, Vector2Int yz)
        {
            return new(vector.x, vector.y + yz.x, vector.z + yz.y);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 AddXY(this Vector4 vector, Vector2 xy)
        {
            return new(vector.x + xy.x, vector.y + xy.y, vector.z, vector.w);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 AddXZ(this Vector4 vector, Vector2 xz)
        {
            return new(vector.x + xz.x, vector.y, vector.z + xz.y, vector.w);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 AddYZ(this Vector4 vector, Vector2 yz)
        {
            return new(vector.x, vector.y + yz.x, vector.z + yz.y, vector.w);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 AddZW(this Vector4 vector, Vector2 zw)
        {
            return new(vector.x, vector.y, vector.z, vector.w + zw.x);
        }

        #endregion
    }
}