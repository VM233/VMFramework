using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class SwapUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapRed(this ref Color a, ref Color b)
        {
            (b.r, a.r) = (a.r, b.r);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapGreen(this ref Color a, ref Color b)
        {
            (b.g, a.g) = (a.g, b.g);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapBlue(this ref Color a, ref Color b)
        {
            (b.b, a.b) = (a.b, b.b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapAlpha(this ref Color a, ref Color b)
        {
            (b.a, a.a) = (a.a, b.a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapX(this ref Vector4 a, ref Vector4 b)
        {
            (b.x, a.x) = (a.x, b.x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapY(this ref Vector4 a, ref Vector4 b)
        {
            (b.y, a.y) = (a.y, b.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapZ(this ref Vector4 a, ref Vector4 b)
        {
            (b.z, a.z) = (a.z, b.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapW(this ref Vector4 a, ref Vector4 b)
        {
            (b.w, a.w) = (a.w, b.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapX(this ref Vector3Int a, ref Vector3Int b)
        {
            (b.x, a.x) = (a.x, b.x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapY(this ref Vector3Int a, ref Vector3Int b)
        {
            (b.y, a.y) = (a.y, b.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapZ(this ref Vector3Int a, ref Vector3Int b)
        {
            (b.z, a.z) = (a.z, b.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapX(this ref Vector3 a, ref Vector3 b)
        {
            (b.x, a.x) = (a.x, b.x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapY(this ref Vector3 a, ref Vector3 b)
        {
            (b.y, a.y) = (a.y, b.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapZ(this ref Vector3 a, ref Vector3 b)
        {
            (b.z, a.z) = (a.z, b.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapX(this ref Vector2 a, ref Vector2 b)
        {
            (b.x, a.x) = (a.x, b.x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapY(this ref Vector2 a, ref Vector2 b)
        {
            (b.y, a.y) = (a.y, b.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapX(this ref Vector2Int a, ref Vector2Int b)
        {
            (b.x, a.x) = (a.x, b.x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapY(this ref Vector2Int a, ref Vector2Int b)
        {
            (b.y, a.y) = (a.y, b.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 SwapXY(this Vector2 a)
        {
            return new(a.y, a.x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 SwapXY(this Vector3 a)
        {
            return new(a.y, a.x, a.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 SwapXZ(this Vector3 a)
        {
            return new(a.z, a.y, a.x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 SwapYZ(this Vector3 a)
        {
            return new(a.x, a.z, a.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int SwapXY(this Vector2Int a)
        {
            return new(a.y, a.x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int SwapXY(this Vector3Int a)
        {
            return new(a.y, a.x, a.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int SwapXZ(this Vector3Int a)
        {
            return new(a.z, a.y, a.x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int SwapYZ(this Vector3Int a)
        {
            return new(a.x, a.z, a.y);
        }
    }
}