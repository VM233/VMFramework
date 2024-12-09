using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public sealed class GenericMinMap<TItem> : IMinHeap<TItem>
        where TItem : IHeapItem<TItem>
    {
        private readonly List<TItem> values = new() { default };

        public GenericMinMap(IEnumerable<TItem> items)
        {
            values.AddRange(items);

            for (int i = values.Count / 2; i >= 1; i--)
            {
                BubbleDown(i);
            }
        }

        public GenericMinMap() : this(Array.Empty<TItem>())
        {
        }

        public int Count => values.Count - 1;

        public TItem MinItem => values[1];

        /// <summary>
        /// Extract the smallest element.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TItem ExtractMin()
        {
            int count = Count;

            if (count == 0)
            {
                throw new InvalidOperationException("Heap is empty.");
            }

            var min = MinItem;
            min.HeapIndex = -1;
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(TItem item)
        {
            item.HeapIndex = Count;
            values.Add(item);
            BubbleUp(Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(TItem item)
        {
            return values[item.HeapIndex].Equals(item);
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
            while (true)
            {
                int left = index * 2;
                int right = index * 2 + 1;

                int min;
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
            return values[index1].CompareTo(values[index2]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Exchange(int index1, int index2)
        {
            var value1 = values[index1];
            var value2 = values[index2];
            value1.HeapIndex = index2;
            value2.HeapIndex = index1;
            values[index1] = value2;
            values[index2] = value1;
        }

        public IEnumerator<TItem> GetEnumerator() => values.Skip(1).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}