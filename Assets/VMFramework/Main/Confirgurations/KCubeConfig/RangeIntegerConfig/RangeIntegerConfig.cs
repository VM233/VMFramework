using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    public partial class RangeIntegerConfig : KCubeIntegerConfig<int>
    {
        public override int Size => max - min + 1;

        public override int Count => Size;

        public override int Pivot => (min + max) / 2;

        #region Constructor

        public RangeIntegerConfig()
        {
            min = 0;
            max = 0;
        }

        public RangeIntegerConfig(int length)
        {
            min = 0;
            max = length - 1;
            max = max.ClampMin(-1);
        }

        public RangeIntegerConfig(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        #endregion

        #region KCube

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Contains(int pos) => pos >= min && pos <= max;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetRelativePos(int pos) => pos - min;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int ClampMin(int pos) => pos.ClampMin(min);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int ClampMax(int pos) => pos.ClampMax(max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetRandomItem(Random random) => random.Range(min, max);

        public override IEnumerator<int> GetEnumerator()
        {
            return new RangeInteger.Enumerator(new RangeInteger(min, max));
        }

        #endregion

        #region Cloneable

        public override object Clone()
        {
            return new RangeIntegerConfig(min, max);
        }

        #endregion
    }
}
