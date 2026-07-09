#if FISHNET
using System.Collections.Generic;
using VMFramework.Core.Pools;

namespace VMFramework.Containers
{
    public readonly struct SomeItemsWrapper
    {
        public readonly List<IndexItemPair> items;

        public SomeItemsWrapper(IContainer container, HashSet<int> slotIndices)
        {
            items = ListPool<IndexItemPair>.Default.Get();
            items.Clear();
            foreach (int slotIndex in slotIndices)
            {
                items.Add(new(slotIndex, container.GetItem(slotIndex)));
            }
        }
        
        public SomeItemsWrapper(List<IndexItemPair> items)
        {
            this.items = items;
        }
    }
}
#endif