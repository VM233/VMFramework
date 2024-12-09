using JetBrains.Annotations;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMFramework.Core.Linq;

namespace VMFramework.Core
{
    public static class CollectionUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty<T>(this IReadOnlyCollection<T> collection) => collection.Count == 0;
        
        #region Add

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddIfNotNull<TCollection, TItem>(this TCollection collection, TItem item)
            where TCollection : ICollection<TItem>
            where TItem : class
        {
            if (item == null)
            {
                return;
            }
            
            collection.Add(item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddIfNotContains<TCollection, TItem>(this TCollection collection, TItem item)
            where TCollection : ICollection<TItem>
        {
            if (collection.Contains(item) == false)
            {
                collection.Add(item);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddIfNotContains<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                if (collection.Contains(item) == false)
                {
                    collection.Add(item);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddRange<TCollection, TItem>(this TCollection collection, IEnumerable<TItem> items)
            where TCollection : ICollection<TItem>
        {
            if (items == null)
            {
                return;
            }

            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        #endregion

        #region Remove

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Remove(item);
            }
        }

        #endregion
    }
}
