using System;
using System.Collections;
using System.Collections.Generic;

namespace VMFramework.Core
{
    public readonly struct ForwardSteppedRangeFloat : ISteppedRange<float>
    {
        public readonly float min;
        public readonly float max;
        public readonly float step;

        public int Count => ((max - min) / step).Floor() + 1;

        float IMinMaxOwner<float>.Min
        {
            get => min;
            init => min = value;
        }

        float IMinMaxOwner<float>.Max
        {
            get => max;
            init => max = value;
        }
        
        float ISteppedRange<float>.Step => step;

        public ForwardSteppedRangeFloat(float min, float max, float step)
        {
            this.min = min;
            this.max = max;
            this.step = step;
        }
        
        public bool Contains(float pos)
        {
            return (pos - min) % step == 0;
        }

        public float GetRandomItem(Random random)
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

        public IEnumerator<float> GetEnumerator()
        {
            return new Enumerator(this);
        }
        
        public struct Enumerator : IEnumerator<float>
        {
            private readonly ForwardSteppedRangeFloat range;
            private float x;

            public Enumerator(ForwardSteppedRangeFloat range)
            {
                this.range = range;
                x = range.min - range.step;
            }

            public float Current => x;

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