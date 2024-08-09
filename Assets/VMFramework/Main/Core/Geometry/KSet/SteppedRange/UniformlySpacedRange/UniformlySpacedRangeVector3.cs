﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VMFramework.Core
{
    public readonly struct UniformlySpacedRangeVector3 : ISteppedRange<Vector3>
    {
        public readonly Vector3 min;
        public readonly Vector3 max;
        public readonly int count;
        
        public Vector3 step => count > 0 ? (max - min) / (count - 1) : Vector3.zero;
        
        public UniformlySpacedRangeVector3(Vector3 min, Vector3 max, int count)
        {
            this.min = min;
            this.max = max;
            this.count = count;
        }
        
        Vector3 IMinMaxOwner<Vector3>.Min
        {
            get => min;
            init => min = value;
        }

        Vector3 IMinMaxOwner<Vector3>.Max
        {
            get => max;
            init => max = value;
        }
        
        int IReadOnlyCollection<Vector3>.Count => count;

        public bool Contains(Vector3 pos)
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
            return offset.x % step.x == 0 && offset.y % step.y == 0 && offset.z % step.z == 0;
        }
        
        #region Enumerator

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Vector3> GetEnumerator()
        {
            return new Enumerator(this);
        }
        
        public struct Enumerator : IEnumerator<Vector3>
        {
            private readonly UniformlySpacedRangeVector3 range;
            private readonly Vector3 step;
            private Vector3 x;
            private int index;

            public Enumerator(UniformlySpacedRangeVector3 range)
            {
                this.range = range;
                step = range.step;
                x = range.min - step;
                index = -1;
            }

            public Vector3 Current => x;

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
                x = range.min - range.step;
            }

            public void Dispose() { }
        }

        #endregion
    }
}