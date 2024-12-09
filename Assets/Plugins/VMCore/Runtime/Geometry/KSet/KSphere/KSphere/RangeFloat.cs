using System.Runtime.CompilerServices;
using UnityEngine;
using System;
using Random = System.Random;

namespace VMFramework.Core
{
    public partial struct RangeFloat : IKSphere<float, float>
    {
        public float Radius
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Extents;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            init
            {
                var pivot = this.Pivot;
                min = pivot - value;
                max = pivot + value;
            }
        }

        public float Center
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Pivot;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            init
            {
                var radius = Extents;
                min = value - radius;
                max = value + radius;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Clamp(float pos) => pos.Clamp(min, max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetRandomPointInside(Random random) => GetRandomItem(random);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetRandomPointOnSurface(Random random)
        {
            if (random.NextBool())
            {
                return min;
            }
            
            return max;
        }
    }
}