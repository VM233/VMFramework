using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.Containers
{
    public static class ContainerAddUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryAddItem<TContainer, TItem>(this TContainer container, TItem item)
            where TContainer : IContainer
            where TItem : IContainerItem
        {
            return container.TryAddItem(item, int.MaxValue, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryAddItem<TRange>(this IContainer container, IContainerItem item, TRange range)
            where TRange : IMinMaxOwner<int>
        {
            return container.TryAddItem(item, range.Min, range.Max, int.MaxValue, out _);
        }

        /// <summary>
        /// 尝试将特定数量的<see cref="IContainerItem"/>添加到index槽位，
        /// 如果<see cref="IContainerItem"/>完全添加到槽位（指<see cref="IContainerItem"/>的数量剩余小于等于0），
        /// 则返回true，否则返回false
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryAddItem<TContainer, TItem>(this TContainer container, int slotIndex, TItem item,
            int preferredCount, out int addedCount)
            where TContainer : IContainer
            where TItem : IContainerItem
        {
            addedCount = 0;

            if (item == null)
            {
                return true;
            }

            if (item.Count <= 0)
            {
                return true;
            }

            container.CheckIndex(slotIndex);

            var itemInContainer = container.GetItem(slotIndex);

            if (itemInContainer == null)
            {
                container.SetItem(slotIndex, item);

                return true;
            }

            if (container.TryMergeItem(slotIndex, item, preferredCount, out addedCount) == false)
            {
                return false;
            }

            return item.Count <= 0;
        }

        /// <summary>
        /// 尝试将<see cref="IContainerItem"/>添加到index槽位，
        /// 如果<see cref="IContainerItem"/>完全添加到槽位（指<see cref="IContainerItem"/>的数量剩余小于等于0），
        /// 则返回true，否则返回false
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryAddItem<TContainer, TItem>(this TContainer container, int slotIndex, TItem item,
            out int addedCount)
            where TContainer : IContainer
            where TItem : IContainerItem
        {
            return container.TryAddItem(slotIndex, item, int.MaxValue, out addedCount);
        }
    }
}