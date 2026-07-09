using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using VMFramework.Core;
using VMFramework.Core.Pools;

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
                if (countDictionary.TryAdd(item.id, item.Count.GetValue()) == false)
                {
                    countDictionary[item.id] += item.Count.GetValue();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetTotalItemCount<TItem>(this IEnumerable<TItem> collection)
            where TItem : IContainerItem
        {
            int count = 0;
            foreach (var item in collection)
            {
                if (item is null)
                {
                    continue;
                }

                count += item.Count.GetValue();
            }

            return count;
        }

        #region By ID

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetItemCountByID<TItem>(this IEnumerable<TItem> collection, string itemID)
            where TItem : IContainerItem
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
                    result += item.Count.GetValue();
                }
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasItemCountByID<TItem>(this IEnumerable<TItem> collection, string itemID, int count)
            where TItem : IContainerItem
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
                    existedCount += item.Count.GetValue();
                }

                if (existedCount >= count)
                {
                    return true;
                }
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasItemsCountByID<TCollection>(this IEnumerable<TCollection> collections, string itemID,
            int count)
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
                if (collections.HasItemsCountByID(itemID, count) == false)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region By Game Tag

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetItemCountByGameTag<TItem>(this IEnumerable<TItem> collection, string gameTag)
            where TItem : IContainerItem
        {
            int result = 0;

            if (gameTag.IsNullOrEmpty())
            {
                return result;
            }

            foreach (var item in collection)
            {
                if (item == null)
                {
                    continue;
                }

                if (item.GameTags.Contains(gameTag))
                {
                    result += item.Count.GetValue();
                }
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetItemCountByAnyGameTags<TItem>(this IEnumerable<TItem> collection,
            IEnumerable<string> gameTags)
            where TItem : IContainerItem
        {
            var result = 0;

            var gameTagsSet = gameTags.ToHashSetDefaultPooled();

            foreach (var item in collection)
            {
                if (item == null)
                {
                    continue;
                }

                foreach (var gameTag in item.GameTags)
                {
                    if (gameTagsSet.Contains(gameTag))
                    {
                        result += item.Count.GetValue();
                        break;
                    }
                }
            }
            
            gameTagsSet.ReturnToDefaultPool();

            return result;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetItemCountByAnyGameTags(this IEnumerable<IContainerItem> collection,
            IEnumerable<string> gameTags, IContainerItem excludeItem)
        {
            var result = 0;

            var gameTagsSet = gameTags.ToHashSetDefaultPooled();

            foreach (var item in collection)
            {
                if (item == null)
                {
                    continue;
                }
                
                if (item == excludeItem)
                {
                    continue;
                }

                foreach (var gameTag in item.GameTags)
                {
                    if (gameTagsSet.Contains(gameTag))
                    {
                        result += item.Count.GetValue();
                        break;
                    }
                }
            }
            
            gameTagsSet.ReturnToDefaultPool();

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasItemCountByGameTag<TItem>(this IEnumerable<TItem> collection, string gameTag, int count,
            out int existedCount)
            where TItem : IContainerItem
        {
            existedCount = 0;

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
                    existedCount += item.Count.GetValue();
                }

                if (existedCount >= count)
                {
                    return true;
                }
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasItemCountByGameTag<TItem>(this IEnumerable<IEnumerable<TItem>> collections,
            string gameTag, int count)
            where TItem : IContainerItem
        {
            if (gameTag.IsNullOrEmpty())
            {
                return false;
            }

            if (count <= 0)
            {
                return true;
            }

            int leftCount = count;

            foreach (var collection in collections)
            {
                if (collection.HasItemCountByGameTag(gameTag, count, out var localCount))
                {
                    return true;
                }

                leftCount -= localCount;
            }

            return leftCount <= 0;
        }

        #endregion
    }
}