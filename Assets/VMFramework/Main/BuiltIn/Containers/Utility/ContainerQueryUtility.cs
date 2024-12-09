using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using VMFramework.Core;
using VMFramework.Core.Linq;

namespace VMFramework.Containers
{
    public static class ContainerQueryUtility
    {
        #region Get Items By ID

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IContainerItem> GetItemsByID<TContainer>(this TContainer container, string itemID)
            where TContainer : IContainer
        {
            foreach (var item in container.ValidItems)
            {
                if (item.id == itemID)
                {
                    yield return item;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetItemsByID<TContainer, TItem>(this IContainer container, string itemID,
            ICollection<TItem> items)
            where TContainer : IContainer
            where TItem : IContainerItem
        {
            foreach (var item in container.ValidItems)
            {
                if (item is TItem typedItem && typedItem.id == itemID)
                {
                    items.Add(typedItem);
                }
            }
        }

        #endregion

        #region Try Get Valid Item

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetValidItem<TContainer>(this TContainer container, int index, out IContainerItem item)
            where TContainer : IContainer
        {
            item = container.GetItem(index);
            return item != null;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetValidItem<TContainer, TItem>(this TContainer container, int index, out TItem item)
            where TContainer : IContainer
            where TItem : IContainerItem
        {
            var containerItem = container.GetItem(index);
            if (containerItem is TItem typedItem)
            {
                item = typedItem;
                return true;
            }

            item = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetValidItemWithWarning<TContainer, TItem>(this TContainer container, int index,
            out TItem item)
            where TContainer : IContainer
            where TItem : IContainerItem
        {
            var containerItem = container.GetItem(index);

            if (containerItem is TItem typedItem)
            {
                item = typedItem;
                return true;
            }

            Debugger.LogWarning(
                $"Failed to get valid {typeof(TItem).Name} from container : {container} at index : {index}");
            item = default;
            return false;
        }

        #endregion

        #region Get Items

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TItem> GetAllItems<TItem>(this IContainer container)
            where TItem : IContainerItem
        {
            foreach (var containerItem in container.GetAllItems())
            {
                if (containerItem is TItem item)
                {
                    yield return item;
                }
            }
        }

        #endregion

        #region Get Items
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IContainerItem> GetItemsByID<TContainer, TCollection>(this TContainer container, TCollection slotIndices)
            where TContainer : IContainer
            where TCollection : IEnumerable<int>
        {
            foreach (var index in slotIndices)
            {
                yield return container.GetItem(index);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IContainerItem> GetValidItems<TContainer, TCollection>(this TContainer container, TCollection slotIndices)
            where TContainer : IContainer
            where TCollection : IEnumerable<int>
        {
            foreach (var index in slotIndices)
            {
                if (container.TryGetValidItem(index, out var item))
                {
                    yield return item;
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetItemsByID<TContainer, TSlots, TItem>(this TContainer container, TSlots slotIndices, ICollection<TItem> items)
            where TContainer : IContainer
            where TSlots : IEnumerable<int>
            where TItem : IContainerItem
        {
            foreach (var index in slotIndices)
            {
                items.Add((TItem)container.GetItem(index));
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetValidItems<TContainer, TSlots, TItem>(this TContainer container, TSlots slotIndices, ICollection<TItem> items)
            where TContainer : IContainer
            where TSlots : IEnumerable<int>
            where TItem : IContainerItem
        {
            foreach (var index in slotIndices)
            {
                if (container.TryGetValidItem(index, out var item))
                {
                    items.Add((TItem)item);
                }
            }
        }

        #endregion

        #region Get Range Items

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IContainerItem> GetRangeItems<TContainer>(this TContainer container, int start, int end)
            where TContainer : IContainer
        {
            (start, end) = container.ClampSlotRange(start, end);

            for (int i = start; i <= end; i++)
            {
                yield return container.GetItem(i);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IContainerItem> GetRangeValidItems<TContainer>(this TContainer container, int start,
            int end)
            where TContainer : IContainer
        {
            (start, end) = container.ClampSlotRange(start, end);

            for (int i = start; i <= end; i++)
            {
                if (container.TryGetValidItem(i, out var item))
                {
                    yield return item;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetRangeItems<TContainer, TItem>(this TContainer container, int start, int end,
            ICollection<TItem> items)
            where TContainer : IContainer
            where TItem : IContainerItem
        {
            (start, end) = container.ClampSlotRange(start, end);

            for (int i = start; i <= end; i++)
            {
                items.Add((TItem)container.GetItem(i));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetRangeValidItems<TContainer, TItem>(this TContainer container, int start, int end,
            ICollection<TItem> items)
            where TContainer : IContainer
            where TItem : IContainerItem
        {
            (start, end) = container.ClampSlotRange(start, end);

            for (int i = start; i <= end; i++)
            {
                if (container.TryGetValidItem(i, out var item))
                {
                    items.Add((TItem)item);
                }
            }
        }

        #endregion

        #region Get Slot Index

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static bool TryGetSlotIndexWithWarning(this IContainer container, IContainerItem item, out int slotIndex)
        // {
        //     foreach (var validSlotIndex in container.ValidSlotIndices)
        //     {
        //         if (container.GetItem(validSlotIndex) == item)
        //         {
        //             slotIndex = validSlotIndex;
        //             return true;
        //         }
        //     }
        //
        //     Debugger.LogWarning($"item : {item} not found in container : {container}");
        //     slotIndex = -1;
        //     return false;
        // }
        //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static bool TryGetSlotIndex(this IContainer container, IContainerItem item, out int slotIndex)
        // {
        //     foreach (var validSlotIndex in container.ValidSlotIndices)
        //     {
        //         if (container.GetItem(validSlotIndex) == item)
        //         {
        //             slotIndex = validSlotIndex;
        //             return true;
        //         }
        //     }
        //
        //     slotIndex = -1;
        //     return false;
        // }

        #endregion

        #region Has Valid Item

        /// <summary>
        ///     判断容器是否有有效的物品
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasValidItem(this IContainer container)
        {
            return container.ValidSlotIndices.Count > 0;
        }

        #endregion

        #region Contains

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsItem(this IContainer container, IContainerItem item)
        {
            return item.SourceContainer == container;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsItem(this IContainer container, string itemID)
        {
            foreach (var item in container.GetAllItems())
            {
                if (item.id == itemID)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}