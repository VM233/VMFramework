using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace VMFramework.Core
{
    public readonly struct UniformlySpacedRangeVector2 : ISteppedRange<Vector2>
    {
        public readonly Vector2 min;
        public readonly Vector2 max;
        public readonly int count;

        public Vector2 Step => count > 0 ? (max - min) / (count - 1) : Vector2.zero;
        
        public UniformlySpacedRangeVector2(Vector2 min, Vector2 max, int count)
        {
            this.min = min;
            this.max = max;
            this.count = count;
        }
        
        Vector2 IMinMaxOwner<Vector2>.Min
        {
            get => min;
            init => min = value;
        }

        Vector2 IMinMaxOwner<Vector2>.Max
        {
            get => max;
            init => max = value;
        }
        
        int IReadOnlyCollection<Vector2>.Count => count;

        public bool Contains(Vector2 pos)
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
            return offset.x % Step.x == 0 && offset.y % Step.y == 0;
        }
        
        public Vector2 GetRandomItem(Random random)
        {
            if (count <= 0)
            {
                throw new InvalidOperationException($"{nameof(UniformlySpacedRangeVector2)} is empty.");
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

        public IEnumerator<Vector2> GetEnumerator()
        {
            return new Enumerator(this);
        }
        
        public struct Enumerator : IEnumerator<Vector2>
        {
            private readonly UniformlySpacedRangeVector2 range;
            private readonly Vector2 step;
            private Vector2 x;
            private int index;

            public Enumerator(UniformlySpacedRangeVector2 range)
            {
                this.range = range;
                step = range.Step;
                x = range.min - step;
                index = -1;
            }

            public Vector2 Current => x;

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