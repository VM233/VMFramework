using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public partial class ListUtility
    {
        #region Get

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> Get<T>(this IList<T> list, RangeInteger range)
        {
            return range.Select(index => list[index]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> Get<T>(this IList<T> list, IEnumerable<int> indices)
        {
            return indices.Select(index => list[index]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> Get<T>(this IList<T> list, params int[] indices)
        {
            return list.Get(indices.AsEnumerable());
        }

        #endregion

        #region Get Or Bound

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetOrBound<T>(this IList<T> list, int index)
        {
            return list[index.Clamp(0, list.Count - 1)];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> GetOrBound<T>(this IList<T> list, IEnumerable<int> indices)
        {
            return indices.Select(list.GetOrBound);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> GetOrBound<T>(this IList<T> list, params int[] indices)
        {
            return list.GetOrBound(indices.AsEnumerable());
        }

        #endregion

        #region Get Range

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> GetRange<T>(this IList<T> list, int index, int count)
        {
            if (index < 0 || index >= list.Count)
            {
                throw new ArgumentOutOfRangeException($"{nameof(index)}:{index}");
            }

            if (count < 0 || index + count > list.Count)
            {
                throw new ArgumentOutOfRangeException($"{nameof(count)}:{count}");
            }

            for (int i = index; i < index + count; i++)
            {
                yield return list[i];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> GetRange<T>(this IList<T> list, RangeInteger range)
        {
            if (range.min < 0 || range.max >= list.Count)
            {
                throw new ArgumentOutOfRangeException($"{nameof(range)}:{range}");
            }

            for (int i = range.min; i <= range.max; i++)
            {
                yield return list[i];
            }
        }

        #endregion

        #region Get Range Or Bound

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> GetRangeOrBound<T>(this IList<T> list, RangeInteger range)
        {
            return list.GetRange(range.ClampBy(0, list.Count - 1));
        }

        #endregion

        #region Get Size Range

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeInteger GetSizeRange<T>(this IList<T> list)
        {
            return new RangeInteger(0, list.Count - 1);
        }

        #endregion

        #region Get All Indices

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<int> GetAllIndices<T>(this IList<T> list)
        {
            return list.Count.GetRange();
        }

        #endregion
    }
}