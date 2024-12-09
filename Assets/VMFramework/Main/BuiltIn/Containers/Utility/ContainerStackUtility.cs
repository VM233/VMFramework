using System.Linq;
using System.Runtime.CompilerServices;
using VMFramework.Core;
using VMFramework.Core.Pools;

namespace VMFramework.Containers
{
    public static class ContainerStackUtility
    {
        /// <summary>
        /// 堆叠物品，将相同物品的数量合并到一起
        /// </summary>
        /// <param name="container"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StackItems<TContainer>(this TContainer container, int startIndex, int endIndex)
            where TContainer : IContainer
        {
            startIndex = startIndex.ClampMin(0);
            endIndex = endIndex.ClampMax(container.Count - 1);
            
            container.GetRangeItems(startIndex, endIndex).BuildIDDictionary(out var itemsByID);

            foreach (var (_, items) in itemsByID)
            {
                int totalCount = items.Sum(item => item.Count);

                if (totalCount <= 1)
                {
                    continue;
                }

                int maxStackCount = items[0].MaxStackCount;

                if (maxStackCount <= 1)
                {
                    continue;
                }

                int validItemNum = totalCount / maxStackCount;
                int leftCount = totalCount % maxStackCount;

                for (int i = 0; i < items.Count; i++)
                {
                    if (i < validItemNum)
                    {
                        items[i].Count = maxStackCount;
                    }
                    else if (i == validItemNum)
                    {
                        items[i].Count = leftCount;
                    }
                    else
                    {
                        items[i].Count = 0;
                    }
                }
            }
            
            itemsByID.ReturnToDefaultPoolRecursively();
        }
    }
}