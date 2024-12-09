using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public partial class Math
    {
        #region Log

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log(this int f, float p)
        {
            return Mathf.Log(f, p);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log(this float f, float p)
        {
            return Mathf.Log(f, p);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Log(this double f, double p)
        {
            return System.Math.Log(f, p);
        }

        #endregion

        #region Ln

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log(this int f)
        {
            return Mathf.Log(f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log(this float f)
        {
            return Mathf.Log(f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Log(this double f)
        {
            return System.Math.Log(f);
        }

        #endregion

        #region Log10

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log10(this int f)
        {
            return Mathf.Log10(f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log10(this float f)
        {
            return Mathf.Log10(f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Log10(this double f)
        {
            return System.Math.Log10(f);
        }

        #endregion

        #region Log2

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log2(this int f)
        {
            return Mathf.Log(f, 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log2(this float f)
        {
            return Mathf.Log(f, 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Log2(this double f)
        {
            return System.Math.Log(f, 2);
        }

        #endregion
    }
}