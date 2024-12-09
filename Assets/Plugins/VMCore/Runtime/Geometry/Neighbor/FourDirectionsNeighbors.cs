using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public readonly struct FourDirectionsNeighbors<TItem> : IEnumerable<TItem>
    {
        public readonly TItem left, right, up, down;

        public FourDirectionsNeighbors(TItem left, TItem right, TItem up, TItem down)
        {
            this.left = left;
            this.right = right;
            this.up = up;
            this.down = down;
        }

        public FourDirectionsNeighbors(TItem item)
        {
            left = item;
            right = item;
            up = item;
            down = item;
        }

        #region Operators

        public TItem this[FourTypesDirection direction] => this.GetNeighbor(direction);

        public TItem this[LeftRightDirection direction] => this.GetNeighbor(direction);

        public TItem this[int index] => this.GetNeighbor(index);

        public static implicit operator FourDirectionsNeighbors<TItem>(TItem item) => new(item);

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
            private readonly FourDirectionsNeighbors<TItem> neighbors;
            private int index;

            public Enumerator(FourDirectionsNeighbors<TItem> neighbors)
            {
                this.neighbors = neighbors;
                index = -1;
            }

            public TItem Current => neighbors.GetNeighbor(index);

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                index++;
                return index < 4;
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