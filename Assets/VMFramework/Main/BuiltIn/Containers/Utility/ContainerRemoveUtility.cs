using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.Containers
{
    public static class ContainerRemoveUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveFromContainer<TItem>(this TItem item)
            where TItem : IContainerItem
        {
            item.SourceContainer?.SetItem(item.SlotIndex, null);
        }
        
        /// <summary>
        /// 尝试移除希望数量的物品，如果完全移除，则返回true，否则返回false，
        /// 如果item为null或item没找到，则changedSlotIndex为-1
        /// </summary>
        /// <param name="item"></param>
        /// <param name="preferredCount"></param>
        /// <param name="removedCount"></param>
        /// <returns></returns>
        public static bool TryRemovePreferredCount<TItem>(this TItem item, int preferredCount,
            out int removedCount)
            where TItem : IContainerItem
        {
            removedCount = 0;

            if (item == null)
            {
                return true;
            }

            removedCount = item.Count.Min(preferredCount);
            item.Count -= removedCount;

            return removedCount == preferredCount || item.Count == 0;
        }
        
        /// <summary>
        /// 在限定的槽位序号里，尝试移除希望数量的一些物品，如果所有物品完全移除，则返回true，否则返回false
        /// </summary>
        /// <param name="container"></param>
        /// <param name="consumptions"></param>
        /// <param name="startSlotIndex"></param>
        /// <param name="endSlotIndex"></param>
        /// <param name="removedCount"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRemoveItems<TContainer, TConsumptions>(this TContainer container, TConsumptions consumptions,
            int startSlotIndex, int endSlotIndex, IDictionary<string, int> removedCount = null)
            where TContainer : IContainer
            where TConsumptions : IReadOnlyDictionary<string, int>
        {
            var result = true;

            foreach (var (itemID, preferredCount) in consumptions)
            {
                result &= container.TryRemoveItems(itemID, preferredCount, startSlotIndex, endSlotIndex,
                    out var itemRemovedCount);

                if (removedCount != null)
                {
                    removedCount.TryAdd(itemID, 0);

                    removedCount[itemID] += itemRemovedCount;
                }
            }

            return result;
        }

        /// <summary>
        /// 在限定的槽位序号里，尝试移除希望数量的物品，如果完全移除，则返回true，否则返回false
        /// </summary>
        /// <param name="container"></param>
        /// <param name="itemID"></param>
        /// <param name="preferredCount"></param>
        /// <param name="startSlotIndex">限定槽位序号起始</param>
        /// <param name="endSlotIndex">限定槽位序号结束</param>
        /// <param name="removedCount">实际被移除的物品数量</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRemoveItems<TContainer>(this TContainer container, [NotNull] string itemID, int preferredCount,
            int startSlotIndex, int endSlotIndex, out int removedCount)
            where TContainer : IContainer
        {
            itemID.AssertIsNotNullOrEmpty(nameof(itemID));
            
            removedCount = 0;

            if (preferredCount <= 0)
            {
                return true;
            }

            (startSlotIndex, endSlotIndex) = container.ClampSlotRange(startSlotIndex, endSlotIndex);

            for (int slotIndex = startSlotIndex; slotIndex <= endSlotIndex; slotIndex++)
            {
                if (container.TryGetValidItem(slotIndex, out var item) == false)
                {
                    continue;
                }

                if (item.id != itemID)
                {
                    continue;
                }
                
                var maxRemovedCount = preferredCount - removedCount;
                if (item.Count >= maxRemovedCount)
                {
                    item.Count -= maxRemovedCount;
                    removedCount = preferredCount;
                    return true;
                }

                container.SetItem(slotIndex, null);
                removedCount += item.Count;
            }

            return false;
        }

        /// <summary>
        /// 尝试移除希望数量的物品，如果完全移除，则返回true，否则返回false
        /// </summary>
        /// <param name="container"></param>
        /// <param name="itemID"></param>
        /// <param name="preferredCount"></param>
        /// <param name="removedCount"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryRemoveItems<TContainer>(this TContainer container, string itemID, int preferredCount,
            out int removedCount)
            where TContainer : IContainer
        {
            return container.TryRemoveItems(itemID, preferredCount, int.MinValue,
                int.MaxValue, out removedCount);
        }
    }
}