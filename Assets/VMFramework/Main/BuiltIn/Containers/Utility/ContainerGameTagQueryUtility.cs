using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Containers
{
    public static class ContainerGameTagQueryUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetAllItemsGameTags(this IContainer container, ICollection<string> gameTags)
        {
            var itemIDs = HashSetPool<string>.Default.Get();
            var gameTagIDs = HashSetPool<string>.Default.Get();
            itemIDs.Clear();
            gameTagIDs.Clear();

            foreach (var item in container.GetAllItems())
            {
                if (item == null)
                {
                    continue;
                }

                if (itemIDs.Add(item.id))
                {
                    gameTagIDs.UnionWith(item.GameTags);
                }
            }

            foreach (var itemTypeID in gameTagIDs)
            {
                gameTags.Add(itemTypeID);
            }
        }
        
        #region Get All Items By Game Type

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetItemsByGameTag<TContainer, TItem>(
            this TContainer container, string tagID, [NotNull] ICollection<TItem> items)
            where TContainer : IContainer
            where TItem : IContainerItem
        {
            foreach (var item in container.GetAllItems())
            {
                if (item is not TItem typedItem)
                {
                    continue;
                }

                if (typedItem.HasTag(tagID))
                {
                    items.Add(typedItem);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetItemsByAnyGameTag<TContainer, TEnumerable, TItem>(
            this TContainer container, TEnumerable tags, [NotNull] ICollection<TItem> items)
            where TContainer : IContainer
            where TEnumerable : IEnumerable<string>
            where TItem : IContainerItem
        {
            if (tags == null)
            {
                return;
            }

            foreach (var item in container.GetAllItems())
            {
                if (item is not TItem typedItem)
                {
                    continue;
                }

                if (item.HasAnyTag(tags))
                {
                    items.Add(typedItem);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetItemsByAllGameTags<TContainer, TEnumerable, TItem>(
            this TContainer container, TEnumerable typeIDs, [NotNull] ICollection<TItem> items)
            where TContainer : IContainer
            where TEnumerable : IEnumerable<string>
            where TItem : IContainerItem
        {
            if (typeIDs == null)
            {
                return;
            }

            foreach (var item in container.GetAllItems())
            {
                if (item is not TItem typedItem)
                {
                    continue;
                }

                if (item.HasAllTags(typeIDs))
                {
                    items.Add(typedItem);
                }
            }
        }

        #endregion
    }
}