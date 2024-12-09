using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public partial class Math
    {
        #region Exp

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Exp(this int power)
        {
            return Mathf.Exp(power);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Exp(this float power)
        {
            return Mathf.Exp(power);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Exp(this double power)
        {
            return System.Math.Exp(power);
        }

        #endregion
    }
}