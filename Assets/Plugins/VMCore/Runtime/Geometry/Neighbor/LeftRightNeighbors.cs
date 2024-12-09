using System;
using System.Collections;
using System.Collections.Generic;

namespace VMFramework.Core
{
    public readonly struct LeftRightNeighbors<TItem> : IEnumerable<TItem>
    {
        public readonly TItem left, right;

        public LeftRightNeighbors(TItem left, TItem right)
        {
            this.left = left;
            this.right = right;
        }

        public LeftRightNeighbors(TItem item)
        {
            this.left = item;
            this.right = item;
        }

        #region Operators

        public TItem this[LeftRightDirection direction] => this.GetNeighbor(direction);

        public TItem this[int direction] => this.GetNeighbor(direction);

        public void Deconstruct(out TItem left, out TItem right)
        {
            left = this.left;
            right = this.right;
        }

        public static implicit operator LeftRightNeighbors<TItem>(TItem item) => new(item);

        public static implicit operator LeftRightNeighbors<TItem>((TItem left, TItem right) tuple) =>
            new(tuple.left, tuple.right);

        public static implicit operator (TItem left, TItem right)(LeftRightNeighbors<TItem> neighbors) =>
            (neighbors.left, neighbors.right);

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
            private readonly LeftRightNeighbors<TItem> neighbors;
            private int index;

            public Enumerator(LeftRightNeighbors<TItem> neighbors)
            {
                this.neighbors = neighbors;
                index = -1;
            }

            public TItem Current
            {
                get
                {
                    return index switch
                    {
                        0 => neighbors.left,
                        1 => neighbors.right,
                        _ => throw new InvalidOperationException()
                    };
                }
            }

            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                index++;
                return index < 2;
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