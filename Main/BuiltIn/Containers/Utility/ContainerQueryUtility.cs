using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using VMFramework.Core;

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

            UnityEngine.Debug.LogWarning(
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
        public static IEnumerable<IContainerItem> GetValidItems<TContainer, TCollection>(this TContainer container,
            TCollection slotIndices)
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
        public static void GetValidItems<TContainer, TSlots, TItem>(this TContainer container, TSlots slotIndices,
            ICollection<TItem> items)
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
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IContainerItem> GetItemsByID<TContainer, TCollection>(this TContainer container,
            TCollection slotIndices)
            where TContainer : IContainer
            where TCollection : IEnumerable<int>
        {
            foreach (var index in slotIndices)
            {
                yield return container.GetItem(index);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetItemsByID<TContainer, TSlots, TItem>(this TContainer container, TSlots slotIndices,
            ICollection<TItem> items)
            where TContainer : IContainer
            where TSlots : IEnumerable<int>
            where TItem : IContainerItem
        {
            foreach (var index in slotIndices)
            {
                items.Add((TItem)container.GetItem(index));
            }
        }

        #endregion

        #region Get Range Items

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IContainerItem> GetRangeItems<TContainer>(this TContainer container, int start,
            int end)
            where TContainer : IContainer
        {
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
            for (int i = start; i <= end; i++)
            {
                items.Add((TItem)container.GetItem(i));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetRangeItems<TContainer, TItem, TMinMax>(this TContainer container, TMinMax minMax,
            ICollection<TItem> items)
            where TContainer : IContainer
            where TItem : IContainerItem
            where TMinMax : IMinMaxOwner<int>
        {
            var start = minMax.Min;
            var end = minMax.Max;
            
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
            for (int i = start; i <= end; i++)
            {
                if (container.TryGetValidItem(i, out var item))
                {
                    items.Add((TItem)item);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetRangeValidItems<TContainer, TItem, TMinMax>(this TContainer container, TMinMax minMax,
            ICollection<TItem> items)
            where TContainer : IContainer
            where TItem : IContainerItem
            where TMinMax : IMinMaxOwner<int>
        {
            var start = minMax.Min;
            var end = minMax.Max;
            
            for (int i = start; i <= end; i++)
            {
                if (container.TryGetValidItem(i, out var item))
                {
                    items.Add((TItem)item);
                }
            }
        }

        #endregion
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetItemByID<TContainer, TItem>(this TContainer container, string itemID, out TItem item)
            where TContainer : IContainer
            where TItem : IContainerItem
        {
            foreach (var containerItem in container.ValidItems)
            {
                if (containerItem is TItem typedItem && typedItem.id == itemID)
                {
                    item = typedItem;
                    return true;
                }
            }
            
            item = default;
            return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetValidItems<TContainer, TItemCollection>(
            [DisallowNull] this IEnumerable<TContainer> containers, [DisallowNull] TItemCollection validItems)
            where TContainer : IContainer
            where TItemCollection : ICollection<IContainerItem>
        {
            foreach (var container in containers)
            {
                if (container == null)
                {
                    continue;
                }

                foreach (var item in container.ValidItems)
                {
                    validItems.Add(item);
                }
            }
        }
    }
}