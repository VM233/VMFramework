using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class TransformScaleUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FlipX(this Transform transform)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FlipY(this Transform transform)
        {
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FlipZ(this Transform transform)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, -transform.localScale.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetScaleXSign(this Transform transform, bool positive)
        {
            var scale = transform.localScale;
            scale.x = positive ? scale.x.Abs() : -scale.x.Abs();
            transform.localScale = scale;
        }
    }
}