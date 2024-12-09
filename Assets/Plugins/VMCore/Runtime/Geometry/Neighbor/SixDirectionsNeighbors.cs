using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public readonly struct SixDirectionsNeighbors<TItem> : IEnumerable<TItem>
    {
        public readonly TItem left, right, up, down, forward, back;

        public SixDirectionsNeighbors(TItem left, TItem right, TItem up, TItem down, TItem forward, TItem back)
        {
            this.left = left;
            this.right = right;
            this.up = up;
            this.down = down;
            this.forward = forward;
            this.back = back;
        }

        public SixDirectionsNeighbors(TItem item)
        {
            up = item;
            down = item;
            left = item;
            right = item;
            forward = item;
            back = item;
        }

        #region Operators

        public TItem this[SixTypesDirection direction] => this.GetNeighbor(direction);

        public TItem this[FourTypesDirection direction] => this.GetNeighbor(direction);

        public TItem this[LeftRightDirection direction] => this.GetNeighbor(direction);
        
        public TItem this[int index] => this.GetNeighbor(index);

        public static implicit operator SixDirectionsNeighbors<TItem>(TItem item) => new(item);

        #endregion

        #region Enumerator

        public IEnumerator<TItem> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public struct Enumerator : IEnumerator<TItem>
        {
            private readonly SixDirectionsNeighbors<TItem> neighbors;
            private int index;

            public Enumerator(SixDirectionsNeighbors<TItem> neighbors)
            {
                this.neighbors = neighbors;
                index = -1;
            }

            public TItem Current => neighbors.GetNeighbor(index);

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                index++;
                return index < 6;
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