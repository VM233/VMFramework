﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public sealed class SimpleMinHeap<T> : IMinHeap<T>
    {
        private readonly List<T> values = new() { default };
        private readonly IComparer<T> comparer;

        public SimpleMinHeap(IEnumerable<T> items, [NotNull] IComparer<T> comparer)
        {
            values.AddRange(items);
            this.comparer = comparer;

            for (int i = values.Count / 2; i >= 1; i--)
            {
                BubbleDown(i);
            }
        }

        public SimpleMinHeap(IEnumerable<T> items) : this(items, Comparer<T>.Default)
        {
        }

        public SimpleMinHeap(IComparer<T> comparer) : this(Array.Empty<T>(), comparer)
        {
        }

        public SimpleMinHeap() : this(Comparer<T>.Default)
        {
        }

        public int Count => values.Count - 1;

        public T MinItem => values[1];

        /// <summary>
        /// Extract the smallest element.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public T ExtractMin()
        {
            int count = Count;

            if (count == 0)
            {
                throw new InvalidOperationException("Heap is empty.");
            }

            var min = MinItem;
            values[1] = values[count];
            values.RemoveAt(count);

            if (values.Count > 1)
            {
                BubbleDown(1);
            }

            return min;
        }

        /// <summary>
        /// Insert the value.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void Add(T item)
        {
            values.Add(item);
            BubbleUp(Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T item)
        {
            var index = values.IndexOf(item);
            return index > 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            values.Clear();
            values.Add(default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void BubbleUp(int index)
        {
            int parent = index / 2;

            while (index > 1 && CompareResult(parent, index) > 0)
            {
                Exchange(index, parent);
                index = parent;
                parent /= 2;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void BubbleDown(int index)
        {
            int min;

            while (true)
            {
                int left = index * 2;
                int right = index * 2 + 1;

                if (left < values.Count && CompareResult(left, index) < 0)
                {
                    min = left;
                }
                else
                {
                    min = index;
                }

                if (right < values.Count && CompareResult(right, min) < 0)
                {
                    min = right;
                }

                if (min != index)
                {
                    Exchange(index, min);
                    index = min;
                }
                else
                {
                    return;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int CompareResult(int index1, int index2)
        {
            return comparer.Compare(values[index1], values[index2]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Exchange(int index1, int index2)
        {
            (values[index1], values[index2]) = (values[index2], values[index1]);
        }

        public IEnumerator<T> GetEnumerator() => values.Skip(1).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}