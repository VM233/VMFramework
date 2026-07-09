#if FISHNET
using FishNet.Serializing;
using VMFramework.Core.Pools;

namespace VMFramework.Containers
{
    public static class AllItemsWrapperSerializer
    {
        public static void WriteAllItemsWrapper(this Writer writer, AllItemsWrapper wrapper)
        {
            writer.WriteInt32(wrapper.count);
            writer.WriteArray(wrapper.items, 0, wrapper.count);
            
            wrapper.items.ReturnToDefaultPool();
        }

        public static AllItemsWrapper ReadAllItemsWrapper(this Reader reader)
        {
            int count = reader.ReadInt32();
            var items = ArrayDefaultPool<IContainerItem>.GetByMinLength(count);
            reader.ReadArray(ref items);
            
            return new AllItemsWrapper(items, count);
        }
    }
}
#endif