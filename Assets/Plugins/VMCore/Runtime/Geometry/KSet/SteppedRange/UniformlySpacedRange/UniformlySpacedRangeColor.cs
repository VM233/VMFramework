using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace VMFramework.Core
{
    public readonly struct UniformlySpacedRangeColor : ISteppedRange<Color>
    {
        public readonly Color min;
        public readonly Color max;
        public readonly int count;
        
        public Color Step => count > 0 ? (max - min) / (count - 1) : ColorDefinitions.zero;
        
        public UniformlySpacedRangeColor(Color min, Color max, int count)
        {
            this.min = min;
            this.max = max;
            this.count = count;
        }
        
        Color IMinMaxOwner<Color>.Min
        {
            get => min;
            init => min = value;
        }

        Color IMinMaxOwner<Color>.Max
        {
            get => max;
            init => max = value;
        }

        int IReadOnlyCollection<Color>.Count => count;

        public bool Contains(Color pos)
        {
            if (count <= 0)
            {
                return false;
            }

            if (count == 1)
            {
                return pos == (max + min) / 2;
            }

            if (count == 2)
            {
                return pos == min || pos == max;
            }
            
            var offset = pos - min;
            return offset.r % Step.r == 0 && offset.g % Step.g == 0 && offset.b % Step.b == 0 && offset.a % Step.a == 0;
        }
        
        public Color GetRandomItem(Random random)
        {
            if (count <= 0)
            {
                throw new InvalidOperationException($"{nameof(UniformlySpacedRangeColor)} is empty.");
            }

            if (count == 1)
            {
                return (max + min) / 2;
            }

            if (count == 2)
            {
                return random.NextBool() ? min : max;
            }

            var index = random.Next(count);
            return min + index * Step;
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

        public IEnumerator<Color> GetEnumerator()
        {
            return new Enumerator(this);
        }
        
        public struct Enumerator : IEnumerator<Color>
        {
            private readonly UniformlySpacedRangeColor range;
            private readonly Color step;
            private Color x;
            private int index;

            public Enumerator(UniformlySpacedRangeColor range)
            {
                this.range = range;
                step = range.Step;
                x = range.min - step;
                index = -1;
            }

            public Color Current => x;

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (range.count <= 0)
                {
                    return false;
                }
                
                index++;

                if (range.count == 1)
                {
                    x = (range.max + range.min) / 2;
                    return index < 1;
                }
                
                x += step;
                return index < range.count;
            }

            public void Reset()
            {
                x = range.min - range.Step;
            }

            public void Dispose() { }
        }

        #endregion
    }
}