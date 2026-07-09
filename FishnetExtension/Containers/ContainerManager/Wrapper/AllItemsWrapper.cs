#if FISHNET
using VMFramework.Core.Pools;

namespace VMFramework.Containers
{
    public readonly struct AllItemsWrapper
    {
        public readonly IContainerItem[] items;
        public readonly int count;
        
        public AllItemsWrapper(IContainerItem[] items, int count)
        {
            this.count = count;
            this.items = items;
        }

        public AllItemsWrapper(IContainer container)
        {
            count = container.Count;
            items = ArrayDefaultPool<IContainerItem>.GetByMinLength(count);
            container.CopyAllItemsToArray(items);
        }
    }
}
#endif