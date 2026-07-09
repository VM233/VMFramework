using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VMFramework.Containers
{
    public static class ContainerItemSplitUtility
    {
        /// <summary>
        /// 如果分割的数量达到了目标数量，则返回true
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool SplitItems<TItem>(this IEnumerable<TItem> items, int targetCount,
            ICollection<IContainerItem> results, out int splitCount)
            where TItem : IContainerItem
        {
            splitCount = 0;
            if (targetCount <= 0)
            {
                return true;
            }

            int leftToConsume = targetCount;

            foreach (var item in items)
            {
                item.Split(leftToConsume, results, out var actualSplitCount);

                leftToConsume -= actualSplitCount;
                splitCount += actualSplitCount;

                if (leftToConsume <= 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}