using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System;

namespace VMFramework.Core
{
    public static partial class ListUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(this IList<T> list, T item)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list.Equals(item))
                {
                    return i;
                }
            }

            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetIndex<T>(this IList<T> list, T item, out int index)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Equals(item))
                {
                    index = i;
                    return true;
                }
            }
            index = -1;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Prepend<T>(this IList<T> list, T item)
        {
            list.Insert(0, item);
        }

        #region Remove

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveAllNull<T>(this IList<T> list) where T : class
        {
            var nullIndices = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                {
                    nullIndices.Add(i - nullIndices.Count);
                }
            }

            foreach (var nullIndex in nullIndices)
            {
                list.RemoveAt(nullIndex);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Remove<T>(this IList<T> list, Func<T, bool> condition)
        {
            bool hasRemoved = false;

            list.Examine(item =>
            {
                if (condition(item))
                {
                    list.Remove(item);
                    hasRemoved = true;
                }
            });

            return hasRemoved;
        }

        #endregion

        #region Remove Same

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveSame<T>(this IList<T> list)
        {
            list.RemoveSame(item => item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveSame<T, TSelector>(this IList<T> list, Func<T, TSelector> selector)
        {
            if (list.Count == 0)
            {
                return;
            }

            List<TSelector> items = new();

            foreach (var parent in list.ToArray())
            {
                var newItem = selector(parent);

                if (items.Contains(newItem))
                {
                    list.Remove(parent);
                }

                items.Add(newItem);
            }
        }

        #endregion

        #region Merge Duplicates

        public static IList<T> MergeDuplicates<T, TSelector>(this IList<T> list, Func<T, TSelector> selector,
            Func<T, T, T> merge)
        {
            var afterMerge = new List<T>();

            var completeIndices = new List<int>();

            for (int thisIndex = 0; thisIndex < list.Count; thisIndex++)
            {
                if (completeIndices.Contains(thisIndex))
                {
                    continue;
                }

                var sameItems = new List<T>();

                for (int otherIndex = thisIndex + 1; otherIndex < list.Count; otherIndex++)
                {
                    if (selector(list[thisIndex]).ClassOrStructEquals(selector(list[otherIndex])))
                    {
                        sameItems.Add(list[otherIndex]);
                        completeIndices.Add(otherIndex);
                    }
                }

                if (sameItems.Count > 0)
                {
                    sameItems.Add(list[thisIndex]);

                    afterMerge.Add(sameItems.Aggregate(merge));
                }
                else
                {
                    afterMerge.Add(list[thisIndex]);
                }
            }

            return afterMerge;
        }

        #endregion

        #region Examine

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Examine<T>(this IList<T> list, Func<T, T> func)
            where T : struct
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = func(list[i]);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Examine<T>(this IList<T> list, Action<T> action)
        {
            foreach (var item in list)
            {
                action(item);
            }
        }

        #endregion

        #region Rotate

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Rotate<T>(this IList<T> list, int offset)
        {
            if (list is not { Count: > 1 })
            {
                return;
            }

            offset = offset.Modulo(list.Count);

            var temp = new List<T>(list.GetRange(list.Count - offset, offset));
            temp.AddRange(list.GetRange(0, list.Count - offset));
            list.Clear();
            list.AddRange(temp);
        }

        #endregion
    }
}
