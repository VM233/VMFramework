using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class ArithmeticUtility
    {
        #region Multiply

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Multiply(this Vector2Int vector, float a) => new(vector.x * a, vector.y * a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Multiply(this Vector2Int a, Vector2Int b) => a * b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Multiply(this Vector3Int vector, float a) =>
            new(vector.x * a, vector.y * a, vector.z * a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int Multiply(this Vector3Int a, Vector3Int b) => a * b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Multiply(this Vector2 a, Vector2 b) => a * b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Multiply(this Vector3 a, Vector3 b) => Vector3.Scale(a, b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Multiply(this Vector4 a, Vector4 b) => Vector4.Scale(a, b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color Multiply(this Color a, Color b) => a * b;

        #endregion

        #region Circular Divide

        /// <summary>
        /// 7 / 3 = 2
        /// -5 / 3 = -2 rather than -1
        /// -6 / 3 = -2
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CircularDivide(this int dividend, int divisor)
        {
            if (dividend < 0)
            {
                return (dividend + 1) / divisor - 1;
            }

            return dividend / divisor;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int CircularDivide(this Vector2Int dividend, Vector2Int divisor) =>
            new(dividend.x.CircularDivide(divisor.x), dividend.y.CircularDivide(divisor.y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int CircularDivide(this Vector3Int dividend, Vector3Int divisor) => new(
            dividend.x.CircularDivide(divisor.x), dividend.y.CircularDivide(divisor.y),
            dividend.z.CircularDivide(divisor.z));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int CircularDivide(this Vector2Int dividend, int divisor) =>
            new(dividend.x.CircularDivide(divisor), dividend.y.CircularDivide(divisor));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int CircularDivide(this Vector3Int dividend, int divisor) => new(
            dividend.x.CircularDivide(divisor), dividend.y.CircularDivide(divisor), dividend.z.CircularDivide(divisor));

        #endregion

        #region Divide

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Divide(this Vector2Int dividend, Vector2Int divisor) => new(dividend.x / divisor.x,
            dividend.y / divisor.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Divide(this Vector2 dividend, Vector2 divisor) => new(dividend.x / divisor.x,
            dividend.y / divisor.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int Divide(this Vector3Int dividend, Vector3Int divisor) => new(dividend.x / divisor.x,
            dividend.y / divisor.y, dividend.z / divisor.z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Divide(this Vector3 dividend, Vector3 divisor) => new(dividend.x / divisor.x,
            dividend.y / divisor.y, dividend.z / divisor.z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Divide(this Vector4 dividend, Vector4 divisor) => new(dividend.x / divisor.x,
            dividend.y / divisor.y, dividend.z / divisor.z, dividend.w / divisor.w);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color Divide(this Color dividend, Color divisor) => new(dividend.r / divisor.r,
            dividend.g / divisor.g, dividend.b / divisor.b, dividend.a / divisor.a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Divide(this Vector2Int dividend, int divisor) => new(dividend.x / divisor,
            dividend.y / divisor);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int Divide(this Vector3Int dividend, int divisor) => new(dividend.x / divisor,
            dividend.y / divisor, dividend.z / divisor);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Divide(this Vector2 dividend, float divisor) => new(dividend.x / divisor,
            dividend.y / divisor);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Divide(this Vector3 dividend, float divisor) => new(dividend.x / divisor,
            dividend.y / divisor, dividend.z / divisor);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Divide(this Vector4 dividend, float divisor) => new(dividend.x / divisor,
            dividend.y / divisor, dividend.z / divisor, dividend.w / divisor);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color Divide(this Color dividend, float divisor) => new(dividend.r / divisor,
            dividend.g / divisor, dividend.b / divisor, dividend.a / divisor);

        #endregion

        #region Modulo

        /// <summary>
        /// 7 Modulo 3 = 1
        /// -5 Modulo 3 = 2 rather than -1
        /// -6 Modulo 3 = 0
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Modulo(this int dividend, int divisor)
        {
            int temp = dividend % divisor;
            if (dividend < 0)
            {
                if (temp == 0)
                {
                    return 0;
                }

                return divisor - Mathf.Abs(temp);
            }

            return temp;
        }

        /// <summary>
        /// 5.9 Modulo 3.1 = 2.8 Same as 5.9 % 3.1
        /// -5.9 Modulo 3.1 = 0.3 rather than -2.8
        /// -5.9 Modulo -3.1 = 0.3 rather than -2.8
        /// 5.9 Modulo -3.1 = 2.8 Same as 5.9 % -3.1
        /// -6 Modulo 3 = 0 Same as -6 % 3
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Modulo(this float dividend, float divisor)
        {
            float temp = dividend % divisor;
            if (dividend < 0)
            {
                if (temp == 0)
                {
                    return 0;
                }

                return divisor - Mathf.Abs(temp);
            }

            return temp;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Modulo(this Vector2Int dividend, Vector2Int divisor) =>
            new(dividend.x.Modulo(divisor.x), dividend.y.Modulo(divisor.y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int Modulo(this Vector3Int dividend, Vector3Int divisor) =>
            new(dividend.x.Modulo(divisor.x), dividend.y.Modulo(divisor.y), dividend.z.Modulo(divisor.z));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Modulo(this Vector2 dividend, Vector2 divisor) => new(dividend.x.Modulo(divisor.x),
            dividend.y.Modulo(divisor.y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Modulo(this Vector3 dividend, Vector3 divisor) => new(dividend.x.Modulo(divisor.x),
            dividend.y.Modulo(divisor.y), dividend.z.Modulo(divisor.z));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Modulo(this Vector4 dividend, Vector4 divisor) => new(dividend.x.Modulo(divisor.x),
            dividend.y.Modulo(divisor.y), dividend.z.Modulo(divisor.z), dividend.w.Modulo(divisor.w));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color Modulo(this Color dividend, Color divisor) => new(dividend.r.Modulo(divisor.r),
            dividend.g.Modulo(divisor.g), dividend.b.Modulo(divisor.b), dividend.a.Modulo(divisor.a));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Modulo(this Vector2Int dividend, int divisor) => new(dividend.x.Modulo(divisor),
            dividend.y.Modulo(divisor));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int Modulo(this Vector3Int dividend, int divisor) => new(dividend.x.Modulo(divisor),
            dividend.y.Modulo(divisor), dividend.z.Modulo(divisor));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Modulo(this Vector2 dividend, float divisor) => new(dividend.x.Modulo(divisor),
            dividend.y.Modulo(divisor));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Modulo(this Vector3 dividend, float divisor) => new(dividend.x.Modulo(divisor),
            dividend.y.Modulo(divisor), dividend.z.Modulo(divisor));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Modulo(this Vector4 dividend, float divisor) => new(dividend.x.Modulo(divisor),
            dividend.y.Modulo(divisor), dividend.z.Modulo(divisor), dividend.w.Modulo(divisor));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color Modulo(this Color dividend, float divisor) => new(dividend.r.Modulo(divisor),
            dividend.g.Modulo(divisor), dividend.b.Modulo(divisor), dividend.a.Modulo(divisor));

        #region For Enumerable

        public static IEnumerable<int> ModuloRange(int start, int end, int divisor, bool includingBound = false)
        {
            int startIndex = start.CircularDivide(divisor);
            int endIndex = end.CircularDivide(divisor);

            if (includingBound == false)
            {
                startIndex++;
                endIndex--;
            }

            for (int i = startIndex; i <= endIndex; i++)
            {
                yield return i;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Vector3Int> ModuloRange(Vector3Int start, Vector3Int end, Vector3Int divisor,
            bool includingBound = false)
        {
            foreach (var x in ModuloRange(start.x, end.x, divisor.x, includingBound))
            {
                foreach (var y in ModuloRange(start.y, end.y, divisor.y, includingBound))
                {
                    foreach (var z in ModuloRange(start.z, end.z, divisor.z, includingBound))
                    {
                        yield return new(x, y, z);
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}