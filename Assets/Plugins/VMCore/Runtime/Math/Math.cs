using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Core.Linq;

namespace VMFramework.Core
{
    public static partial class Math
    {
        #region Number

        #region Basic

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float F(this int num)
        {
            return num;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float F(this double num)
        {
            return (float)num;
        }

        #endregion

        #region PingPong

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int PingPong(this int num, int length)
        {
            length--;
            return length - (num.Repeat(length + length) - length).Abs();
        }

        #endregion

        #region Caculate

        #region Average

        public static float Average(this IEnumerable<float> enumerable,
            int fromIndex, int endIndex)
        {
            return enumerable.Sum(fromIndex, endIndex) / (endIndex - fromIndex + 1);
        }

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any(params bool[] args)
        {
            return Enumerable.Any(args);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool All(params bool[] args)
        {
            return Enumerable.Any(args);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Percent(this float t, float min, float max)
        {
            float percent = t.Normalize01(min, max).Clamp(0, 1);

            return percent * 100;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Percent(this float t)
        {
            return t.Percent(0, 100);
        }

        #endregion

        #endregion

        #region Vector

        #region Scale To

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ScaleTo(this Vector2 vector, float length)
        {
            var magnitude = vector.magnitude;
            if (magnitude <= float.Epsilon)
            {
                return Vector2.zero;
            }
            
            var factor = length / magnitude;
            return vector * factor;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ScaleTo(this Vector3 vector, float length) {
            var magnitude = vector.magnitude;
            if (magnitude <= float.Epsilon)
            {
                return Vector3.zero;
            }
            
            var factor = length / magnitude;
            return vector * factor;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 ScaleTo(this Vector4 vector, float length) {
            var magnitude = vector.magnitude;
            if (magnitude <= float.Epsilon)
            {
                return Vector4.zero;
            }
            
            var factor = length / magnitude;
            return vector * factor;
        }

        #endregion

        #endregion
    }
}