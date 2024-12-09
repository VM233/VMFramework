using System;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public partial struct RangeFloat
    {
        float IMinMaxOwner<float>.Min
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => min;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            init => min = value;
        }

        float IMinMaxOwner<float>.Max
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => max;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            init => max = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(float pos) => pos >= min && pos <= max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetRelativePos(float pos) => pos - min;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ClampMin(float pos) => pos.ClampMin(min);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ClampMax(float pos) => pos.ClampMax(max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float GetRandomItem(Random random) => random.Range(min, max);
        
        object IRandomItemProvider.GetRandomObjectItem(Random random)
        {
            return GetRandomItem(random);
        }

        void IChooser.ResetChooser()
        {
            
        }
    }
}