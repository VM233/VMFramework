using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Containers
{
    public static class ContainerPopUtility
    {
        #region Pop By Slot Index

        /// <summary>
        /// 尝试弹出槽位上的物品，如果槽位无效或者物品为null，则返回false，其他情况返回true
        /// </summary>
        public static bool PopItemBySlotIndex<TContainer, TItem>([NotNull] this TContainer container, int index,
            out TItem item)
            where TContainer : IContainer
            where TItem : IContainerItem
        {
            container.MandatoryCheckIndex(index);
            item = (TItem)container.GetItem(index);

            if (item == null)
            {
                return false;
            }

            container.SetItem(index, null);

            return true;
        }

        /// <summary>
        /// 尝试弹出槽位上的物品到指定的容器
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ItemMergeResult PopItemBySlotIndexTo<TFromContainer, TToContainer>(
            [NotNull] this TFromContainer container, int slotIndex, [NotNull] TToContainer targetContainer,
            ContainerAddArguments arguments)
            where TFromContainer : IContainer
            where TToContainer : IContainer
        {
            var item = container.GetItem(slotIndex);
            return targetContainer.AddItem(item, arguments, out _);
        }

        /// <summary>
        /// 尝试弹出槽位上的物品到指定的容器
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ItemMergeResult PopItemBySlotIndexTo<TFromContainer, TToContainer>(
            [NotNull] this TFromContainer container, int slotIndex, [NotNull] TToContainer targetContainer,
            ContainerAddArguments arguments, out int actualInsertCount)
            where TFromContainer : IContainer
            where TToContainer : IContainer
        {
            var item = container.GetItem(slotIndex);
            return targetContainer.AddItem(item, arguments, out actualInsertCount);
        }

        #endregion

        #region Pop By Tag
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PopAllItemsByTag<TItem>(this IContainer container, string tag, ICollection<TItem> items)
        {
            var count = container.Count;
            for (var i = 0; i < count; i++)
            {
                var item = container.GetItem(i);
                if (item == null)
                {
                    continue;
                }
                
                if (item.HasTag(tag) == false)
                {
                    continue;
                }

                container.SetItem(i, null);
                var typedItem = (TItem)item;
                items.Add(typedItem);
            }
        }

        #endregion

        #region Pop Items

        /// <summary>
        /// 尝试将容器中的所有物品弹出到指定的容器中。
        /// 如果全部物品都弹出到目标容器中，则返回true，否则返回false。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ItemMergeResult PopAllItemsTo(this IContainer container, IContainer targetContainer,
            ContainerAddArguments arguments)
        {
            var result = ItemMergeResult.None;

            var validItems = ListPool<IContainerItem>.Default.Get();
            validItems.Clear();

            validItems.AddRange(container.ValidItems);
            foreach (var item in validItems)
            {
                var currentResult = container.PopItemBySlotIndexTo(item.SlotIndex, targetContainer, arguments);

                if (result == ItemMergeResult.FullMerge)
                {
                    continue;
                }

                if (currentResult != ItemMergeResult.None)
                {
                    result = currentResult;
                }
            }

            validItems.ReturnToDefaultPool();

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PopAllItems<TContainer, TItem>(this TContainer container, ICollection<TItem> items)
            where TContainer : IContainer
            where TItem : IContainerItem
        {
            var count = container.Count;
            for (var i = 0; i < count; i++)
            {
                var item = container.GetItem(i);
                if (item == null)
                {
                    continue;
                }

                container.SetItem(i, null);
                var typedItem = (TItem)item;
                items.Add(typedItem);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PopItems<TContainer, TItem>(this TContainer container, RangeInteger range,
            ICollection<TItem> items)
            where TContainer : IContainer
        {
            range = container.ClampSlotRange(range);

            foreach (var index in range)
            {
                var item = container.GetItem(index);
                if (item == null)
                {
                    continue;
                }

                container.SetItem(index, null);
                var typedItem = (TItem)item;
                items.Add(typedItem);
            }
        }

        #endregion

        /// <summary>
        /// 尝试弹出指定数量的物品，如果没有弹出任何物品，则会返回false，
        /// 如果有部分或全部物品被弹出，会返回true。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool PopItemByPreferredCount<TContainer>([NotNull] this TContainer container, int slotIndex,
            int preferredCount, ICollection<IContainerItem> items, out int actualSplitCount)
            where TContainer : IContainer
        {
            container.MandatoryCheckIndex(slotIndex);
            var currentItem = container.GetItem(slotIndex);

            if (currentItem == null)
            {
                actualSplitCount = 0;
                return false;
            }

            currentItem.Split(preferredCount, items, out actualSplitCount);

            return actualSplitCount > 0;
        }
    }
}