using System;
using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    public partial class RangeFloatConfig : KCubeFloatConfig<float>
    {
        public override float Size => max - min;

        public override float Pivot => (min + max) / 2f;

        public override float Extents => (max - min) / 2f;

        #region Constructor

        public RangeFloatConfig()
        {
            min = 0f;
            max = 0f;
        }

        public RangeFloatConfig(float length)
        {
            min = 0f;
            max = length;
            max = max.ClampMin(0);
        }

        public RangeFloatConfig(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        #endregion

        #region KCube

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Contains(float pos) =>
            pos >= min && pos <= max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override float GetRelativePos(float pos) => pos - min;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override float ClampMin(float pos) => pos.ClampMin(min);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override float ClampMax(float pos) => pos.ClampMax(max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override float GetRandomItem(Random random) => random.Range(min, max);

        #endregion

        #region Cloneable

        public override object Clone()
        {
            return new RangeFloatConfig(min, max);
        }

        #endregion
    }
}
