using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.Containers
{
    public static class ItemsCountUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BuildCountDictionary<TCollection, TDictionary>(this TCollection items,
            [NotNull] TDictionary countDictionary)
            where TCollection : IEnumerable<IContainerItem>
            where TDictionary : IDictionary<string, int>
        {
            foreach (var item in items)
            {
                if (countDictionary.TryAdd(item.id, item.Count) == false)
                {
                    countDictionary[item.id] += item.Count;
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetTotalItemCount<TCollection>(this TCollection collection)
            where TCollection : IEnumerable<IContainerItem>
        {
            int count = 0;
            foreach (var item in collection)
            {
                if (item is null)
                {
                    continue;
                }
                
                count += item.Count;
            }
            
            return count;
        }

        #region By ID

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetItemCountByID<TCollection>(this TCollection collection, string itemID)
            where TCollection : IEnumerable<IContainerItem>
        {
            int result = 0;

            if (itemID.IsNullOrEmpty())
            {
                return result;
            }

            foreach (var item in collection)
            {
                if (item == null)
                {
                    continue;
                }
                
                if (item.id == itemID)
                {
                    result += item.Count;
                }
            }

            return result;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasItemCountByID<TCollection>(this TCollection collection, string itemID, int count)
            where TCollection : IEnumerable<IContainerItem>
        {
            int existedCount = 0;

            if (itemID.IsNullOrEmpty())
            {
                return false;
            }

            foreach (var item in collection)
            {
                if (item == null)
                {
                    continue;
                }
                
                if (item.id == itemID)
                {
                    existedCount += item.Count;
                }

                if (existedCount >= count)
                {
                    return true;
                }
            }

            return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasItemCountByID<TCollection>(this IEnumerable<TCollection> collections, string itemID, int count)
            where TCollection : IEnumerable<IContainerItem>
        {
            var existedCount = 0;

            foreach (var collection in collections)
            {
                existedCount += collection.GetItemCountByID(itemID);

                if (existedCount >= count)
                {
                    return true;
                }
            }

            return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasItemsCountByID<TCollection>(this IEnumerable<TCollection> collections,
            IReadOnlyDictionary<string, int> itemDictionary)
            where TCollection : IEnumerable<IContainerItem>
        {
            foreach (var (itemID, count) in itemDictionary)
            {
                if (collections.HasItemCountByID(itemID, count) == false)
                {
                    return false;
                }
            }
            
            return true;
        }

        #endregion

        #region By Game Tag

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasItemCountByGameTag<TCollection>(this TCollection collection, string gameTag, int count)
            where TCollection : IEnumerable<IContainerItem>
        {
            int existedCount = 0;

            if (gameTag.IsNullOrEmpty())
            {
                return false;
            }

            foreach (var item in collection)
            {
                if (item == null)
                {
                    continue;
                }

                if (item.GameTags.Contains(gameTag))
                {
                    existedCount += item.Count;
                }

                if (existedCount >= count)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}