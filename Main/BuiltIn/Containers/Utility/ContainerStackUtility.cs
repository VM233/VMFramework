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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StackItems<TContainer>(this TContainer container, RangeInteger range)
            where TContainer : IContainer
        {
            var items = ListPool<IContainerItem>.Default.Get();
            items.Clear();

            container.PopItems(range, items);

            foreach (var item in items)
            {
                container.AddItem(item, new(range), out _);
            }
        }
    }
}