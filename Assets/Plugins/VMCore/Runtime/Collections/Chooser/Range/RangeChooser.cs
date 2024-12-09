using System;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public readonly partial struct RangeChooser<TNumber, TRange> : IChooser<TNumber> where TNumber : struct, IEquatable<TNumber>
        where TRange : IMinMaxOwner<TNumber>, IRandomItemProvider<TNumber>
    {
        public readonly TRange range;

        public RangeChooser(TRange range)
        {
            this.range = range;
        }

        public TNumber GetRandomItem(Random random)
        {
            return range.GetRandomItem(random);
        }

        public void ResetChooser()
        {
            
        }

        public object GetObjectValue(Random random)
        {
            return GetRandomItem(random);
        }
    }
}