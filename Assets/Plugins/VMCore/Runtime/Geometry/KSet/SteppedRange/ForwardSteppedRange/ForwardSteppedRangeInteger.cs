using System;
using System.Collections;
using System.Collections.Generic;

namespace VMFramework.Core
{
    public readonly struct ForwardSteppedRangeInteger : ISteppedRange<int>
    {
        public readonly int min;
        public readonly int max;
        public readonly int step;

        public int Count => (max - min) / step + 1;

        int IMinMaxOwner<int>.Min
        {
            get => min;
            init => min = value;
        }

        int IMinMaxOwner<int>.Max
        {
            get => max;
            init => max = value;
        }
        
        int ISteppedRange<int>.Step => step;

        public ForwardSteppedRangeInteger(int min, int max, int step)
        {
            this.min = min;
            this.max = max;
            this.step = step;
        }
        
        public bool Contains(int pos)
        {
            return (pos - min) % step == 0;
        }
        
        public int GetRandomItem(Random random)
        {
            var index = random.Next(Count);
            return min + index * step;
        }

        object IRandomItemProvider.GetRandomObjectItem(Random random)
        {
            return GetRandomItem(random);
        }

        #region Enumerator

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<int> GetEnumerator()
        {
            return new Enumerator(this);
        }
        
        public struct Enumerator : IEnumerator<int>
        {
            private readonly ForwardSteppedRangeInteger range;
            private int x;

            public Enumerator(ForwardSteppedRangeInteger range)
            {
                this.range = range;
                x = range.min - range.step;
            }

            public int Current => x;

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                x += range.step;
                
                return x <= range.max;
            }

            public void Reset()
            {
                x = range.min - range.step;
            }

            public void Dispose() { }
        }

        #endregion
    }
}