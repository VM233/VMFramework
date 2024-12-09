using System;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public partial struct RangeInteger : IKSphere<int, int>
    {
        public int Center
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Pivot;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            init
            {
                var radius = this.Radius;
                min = value - radius;
                max = value + radius;
            }
        }

        public int Radius
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (max + min) / 2;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            init
            {
                var pivot = this.Pivot;
                min = pivot - value;
                max = pivot + value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Clamp(int pos) => pos.Clamp(min, max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetRandomPointInside(Random random) => random.Range(min + 1, max - 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetRandomPointOnSurface(Random random)
        {
            if (random.NextBool())
            {
                return min;
            }
            
            return max;
        }
    }
}