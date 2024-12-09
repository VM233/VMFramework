using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public partial class Math
    {
        #region Square

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Square(this int num)
        {
            return num * num;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Square(this float num)
        {
            return num * num;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Square(this long num)
        {
            return num * num;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Square(this double num)
        {
            return num * num;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Square(this Vector2 vector)
        {
            return vector * vector;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Square(this Vector3 vector)
        {
            return new Vector3(vector.x * vector.x, vector.y * vector.y, vector.z * vector.z);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Square(this Vector2Int vector)
        {
            return vector * vector;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int Square(this Vector3Int vector)
        {
            return vector * vector;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Square(this Vector4 vector)
        {
            return new Vector4(vector.x * vector.x, vector.y * vector.y, vector.z * vector.z,
                vector.w * vector.w);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color Square(this Color color)
        {
            return new Color(color.r * color.r, color.g * color.g, color.b * color.b,
                color.a * color.a);
        }

        #endregion

        #region Power

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Power(this int toPow, float power)
        {
            return Mathf.Pow(toPow, power).Round();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Power(this float toPow, float power)
        {
            return Mathf.Pow(toPow, power);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Power(this double toPow, double power)
        {
            return System.Math.Pow(toPow, power);
        }

        #endregion
        
        #region Sqrt

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sqrt(this int value)
        {
            return Mathf.Sqrt(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sqrt(this float value)
        {
            return Mathf.Sqrt(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Sqrt(this double value)
        {
            return System.Math.Sqrt(value);
        }

        #endregion

        #region Cbrt

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Cbrt(this double value)
        {
            return System.Math.Cbrt(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cbrt(this int value)
        {
            return MathF.Cbrt(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cbrt(this float value)
        {
            return MathF.Cbrt(value);
        }

        #endregion
    }
}