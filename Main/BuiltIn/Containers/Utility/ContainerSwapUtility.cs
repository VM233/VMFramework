using System.Runtime.CompilerServices;

namespace VMFramework.Containers
{
    public static class ContainerSwapUtility
    {
        /// <summary>
        /// 和另一个容器的某个槽位交换两个槽位中的物品
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapItem(this IContainer container, int index, IContainer targetContainer, int targetIndex,
            string intention)
        {
            container.MandatoryCheckIndex(index);
            targetContainer.MandatoryCheckIndex(targetIndex);

            var thisItem = container.GetItem(index);
            var targetItem = targetContainer.GetItem(targetIndex);

            if (thisItem == null && targetItem == null)
            {
                return;
            }

            if (thisItem != null)
            {
                if (targetContainer.IsMatchFilters(targetIndex, thisItem, intention) == false)
                {
                    return;
                }
            }

            if (targetItem != null)
            {
                if (container.IsMatchFilters(index, targetItem, intention) == false)
                {
                    return;
                }
            }

            container.SetItem(index, targetItem);
            targetContainer.SetItem(targetIndex, thisItem);
        }

        /// <summary>
        /// 将此容器的某个槽位中的物品添加到另一个容器的某个槽位中，如果目标槽位已经有物品，则交换两个槽位中的物品
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddOrSwapItemTo(this IContainer container, int index, IContainer targetContainer,
            int targetIndex, string intention)
        {
            container.MandatoryCheckIndex(index);
            targetContainer.MandatoryCheckIndex(targetIndex);

            var thisItem = container.GetItem(index);
            var targetItem = targetContainer.GetItem(targetIndex);

            if (targetItem == null)
            {
                container.PopItemBySlotIndexTo(index, targetContainer, new(null, targetIndex));
                return;
            }

            if (thisItem == null)
            {
                targetContainer.PopItemBySlotIndexTo(targetIndex, container, new(intention, index));
                return;
            }

            var addResult = targetContainer.AddItem(thisItem, new(targetIndex), out _);

            if (addResult == ItemMergeResult.None)
            {
                container.SwapItem(index, targetContainer, targetIndex, intention);
            }
        }
    }
}