#if FISHNET
using FishNet.Serializing;
using VMFramework.Core.Pools;

namespace VMFramework.Containers
{
    public static class SomeItemsWrapperSerializer
    {
        public static void WriteSomeItemsWrapper(this Writer writer, SomeItemsWrapper wrapper)
        {
            writer.WriteList(wrapper.items);
            wrapper.items.ReturnToDefaultPool();
        }

        public static SomeItemsWrapper ReadSomeItemsWrapper(this Reader reader)
        {
            var tempList = ListPool<IndexItemPair>.Default.Get();
            tempList.Clear();
            reader.ReadList(ref tempList);
            return new SomeItemsWrapper(tempList);
        }
    }
}
#endif