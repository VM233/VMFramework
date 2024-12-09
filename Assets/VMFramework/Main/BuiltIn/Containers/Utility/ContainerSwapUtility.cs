using System.Runtime.CompilerServices;

namespace VMFramework.Containers
{
    public static class ContainerSwapUtility
    {
        /// <summary>
        /// 和另一个容器的某个槽位交换两个槽位中的物品
        /// </summary>
        /// <param name="container"></param>
        /// <param name="index"></param>
        /// <param name="targetContainer"></param>
        /// <param name="targetIndex"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapItem(this IContainer container, int index, IContainer targetContainer,
            int targetIndex)
        {
            container.CheckIndex(index);
            targetContainer.CheckIndex(targetIndex);
            
            var thisItem = container.GetItem(index);
            var targetItem = targetContainer.GetItem(targetIndex);

            if (thisItem == null && targetItem == null)
            {
                return;
            }

            container.SetItem(index, targetItem);
            targetContainer.SetItem(targetIndex, thisItem);
        }

        /// <summary>
        /// 将此容器的某个槽位中的物品添加到另一个容器的某个槽位中，如果目标槽位已经有物品，则交换两个槽位中的物品
        /// </summary>
        /// <param name="container"></param>
        /// <param name="index"></param>
        /// <param name="targetContainer"></param>
        /// <param name="targetIndex"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddOrSwapItemTo(this IContainer container, int index, IContainer targetContainer,
            int targetIndex)
        {
            container.CheckIndex(index);
            targetContainer.CheckIndex(targetIndex);
            
            var thisItem = container.GetItem(index);
            var targetItem = targetContainer.GetItem(targetIndex);
            
            if (targetItem == null)
            {
                container.SwapItem(index, targetContainer, targetIndex);
                return;
            }

            if (thisItem == null)
            {
                container.SwapItem(index, targetContainer, targetIndex);
                return;
            }

            if (targetContainer.TryAddItem(targetIndex, thisItem, out _) == false)
            {
                container.SwapItem(index, targetContainer, targetIndex);
            }
        }
    }
}