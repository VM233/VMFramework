using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using VMFramework.Core;
using VMFramework.Core.Linq;

namespace VMFramework.Containers
{
    public static class ContainerCountUtility
    {
        #region Container Item Count

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetItemCount(this IContainer container, int minIndex, int maxIndex, string itemID)
        {
            int result = 0;

            if (itemID.IsNullOrEmpty())
            {
                return result;
            }

            maxIndex = maxIndex.Min(container.Count - 1);

            for (int slotIndex = minIndex; slotIndex <= maxIndex; slotIndex++)
            {
                var item = container.GetItem(slotIndex);
                
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
        public static bool HasItemCount(this IContainer container, int minIndex, int maxIndex, string itemID,
            int count)
        {
            int existedCount = 0;

            if (itemID.IsNullOrEmpty())
            {
                return false;
            }
            
            maxIndex = maxIndex.Min(container.Count - 1);

            for (int slotIndex = minIndex; slotIndex <= maxIndex; slotIndex++)
            {
                var item = container.GetItem(slotIndex);
                
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
        public static bool HasItemsCount(this IContainer container, IReadOnlyDictionary<string, int> itemCountDictionary)
        {
            if (itemCountDictionary.IsNullOrEmpty())
            {
                return true;
            }

            foreach (var (itemID, count) in itemCountDictionary)
            {
                if (container.HasItemCountByID(itemID, count) == false)
                {
                    return false;
                }
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasItemsCount(this IContainer container, int minIndex, int maxIndex,
            IReadOnlyDictionary<string, int> itemCountDictionary)
        {
            if (itemCountDictionary.IsNullOrEmpty())
            {
                return true;
            }

            foreach (var (itemID, count) in itemCountDictionary)
            {
                if (container.HasItemCount(minIndex, maxIndex, itemID, count) == false)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
        
        #region Get Item Count

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetItemCount(this IReadOnlyCollection<IContainer> containers, string itemID)
        {
            var count = 0;

            foreach (var container in containers)
            {
                count += container.GetItemCountByID(itemID);
            }

            return count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, int> GetItemsCount(this IReadOnlyCollection<IContainer> containers,
            IEnumerable<string> itemsID)
        {
            var itemsCount = new Dictionary<string, int>();

            foreach (var itemID in itemsID)
            {
                itemsCount[itemID] = GetItemCount(containers, itemID);
            }

            return itemsCount;
        }

        #endregion

        #region Query Item Count

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IContainer> QueryContainersWithEnoughItems(
            this IEnumerable<IContainer> containers, string itemID, int count)
        {
            return containers.Where(container => container.HasItemCountByID(itemID, count));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IContainer> QueryContainersWithEnoughItems(
            this IEnumerable<IContainer> containers, IReadOnlyDictionary<string, int> itemDictionary)
        {
            return containers.Where(container => container.HasItemsCount(itemDictionary));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IContainer QueryContainerWithEnoughItems(this IEnumerable<IContainer> containers,
            string itemID, int count)
        {
            return containers.FirstOrDefault(container => container.HasItemCountByID(itemID, count));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IContainer QueryContainerWithEnoughItems(this IEnumerable<IContainer> containers,
            IReadOnlyDictionary<string, int> itemDictionary)
        {
            return containers.FirstOrDefault(container => container.HasItemsCount(itemDictionary));
        }

        #endregion
    }
}