using System;
using System.Collections;
using System.Collections.Generic;

namespace VMFramework.Core
{
    public readonly struct EightDirectionsNeighbors<TItem> : IEnumerable<TItem>
    {
        public readonly TItem upLeft, up, upRight;
        public readonly TItem left, right;
        public readonly TItem downLeft, down, downRight;

        public EightDirectionsNeighbors(TItem left, TItem right, TItem up, TItem down, TItem upLeft, TItem upRight,
            TItem downLeft, TItem downRight)
        {
            this.left = left;
            this.right = right;
            this.up = up;
            this.down = down;
            this.upLeft = upLeft;
            this.upRight = upRight;
            this.downLeft = downLeft;
            this.downRight = downRight;
        }

        public EightDirectionsNeighbors(TItem item)
        {
            left = item;
            right = item;
            up = item;
            down = item;
            upLeft = item;
            upRight = item;
            downLeft = item;
            downRight = item;
        }

        #region Operators

        public TItem this[EightTypesDirection direction] => this.GetNeighbor(direction);
        
        public TItem this[FourTypesDirection direction] => this.GetNeighbor(direction);
        
        public TItem this[LeftRightDirection direction] => this.GetNeighbor(direction);

        public TItem this[int index] => this.GetNeighbor(index);
        
        public static implicit operator EightDirectionsNeighbors<TItem>(TItem item) => new(item);

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
            private readonly EightDirectionsNeighbors<TItem> neighbors;
            private int index;

            public Enumerator(EightDirectionsNeighbors<TItem> neighbors)
            {
                this.neighbors = neighbors;
                index = -1;
            }

            public TItem Current => neighbors.GetNeighbor(index);

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                index++;
                return index < 8;
            }

            public void Reset()
            {
                index = -1;
            }

            public void Dispose() { }
        }

        #endregion
    }
}