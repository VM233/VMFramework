using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VMFramework.Core
{
    public readonly struct RangeBoundary : IEnumerableKSet<int>
    {
        public readonly int min;
        public readonly int max;

        public int Count
        {
            get
            {
                if (min > max)
                {
                    return 0;
                }

                if (min == max)
                {
                    return 1;
                }

                return 2;
            }
        }

        public RangeBoundary(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public RangeBoundary(RangeInteger range)
        {
            min = range.min;
            max = range.max;
        }

        public RangeBoundary(Vector2Int range)
        {
            min = range.x;
            max = range.y;
        }

        public bool Contains(int pos)
        {
            return pos == min || pos == max;
        }

        #region Operators

        public static implicit operator RangeBoundary(RangeInteger range) => new(range);

        public static implicit operator RangeBoundary(Vector2Int range) => new(range);

        public static implicit operator RangeBoundary((int min, int max) range) => new(range.min, range.max);

        public static implicit operator (int min, int max)(RangeBoundary boundary) => (boundary.min, boundary.max);

        #endregion

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
            private readonly RangeBoundary boundary;
            private readonly int count;
            private int index;

            public Enumerator(RangeBoundary boundary)
            {
                this.boundary = boundary;
                count = boundary.Count;
                index = -1;
            }

            public int Current => index switch
            {
                0 => boundary.min,
                1 => boundary.max,
                _ => throw new IndexOutOfRangeException()
            };

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                index++;
                return index < count;
            }

            public void Reset()
            {
                index = -1;
            }

            public void Dispose()
            {
            }
        }

        #endregion
    }
}