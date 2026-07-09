using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMFramework.Core.Pools;

namespace VMFramework.Containers
{
    public static class ContainerItemRemoveUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool RemoveItemByCount<TItem>(this IEnumerable<TItem> items, string id, int count, out int removedCount)
            where TItem : IContainerItem
        {
            removedCount = 0;

            if (count <= 0)
            {
                return true;
            }

            var tempItems = items.ToListDefaultPooled();

            foreach (var item in tempItems)
            {
                if (item.id != id)
                {
                    continue;
                }
                
                var toRemove = count - removedCount;

                item.Remove(toRemove, out var thisRemovedCount);

                removedCount += thisRemovedCount;

                if (removedCount >= count)
                {
                    tempItems.ReturnToDefaultPool();
                    return true;
                }
            }

            tempItems.ReturnToDefaultPool();
            return false;
        }
        
        /// <summary>
        /// 尝试移除希望数量的物品，如果完全移除，则返回true，否则返回false
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool RemoveItems<TItem>(this IEnumerable<TItem> items, int targetCount, out int removedCount)
            where TItem : IContainerItem
        {
            removedCount = 0;

            if (targetCount <= 0)
            {
                return true;
            }

            var leftToRemove = targetCount;

            foreach (var item in items)
            {
                item.Remove(leftToRemove, out var thisRemovedCount);

                removedCount += thisRemovedCount;
                leftToRemove -= thisRemovedCount;

                if (leftToRemove <= 0)
                {
                    return true;
                }
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CheckRemovable<TItem>(this IEnumerable<TItem> items, int targetCount, out int removableCount)
            where TItem : IContainerItem
        {
            removableCount = 0;

            if (targetCount <= 0)
            {
                return true;
            }

            var leftToRemove = targetCount;

            foreach (var item in items)
            {
                item.CheckRemovable(leftToRemove, out var thisRemovableCount);

                removableCount += thisRemovableCount;
                leftToRemove -= thisRemovableCount;

                if (leftToRemove <= 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}