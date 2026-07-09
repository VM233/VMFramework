#if FISHNET
using FishNet.Serializing;

namespace VMFramework.Containers
{
    public static class IndexItemPairSerializer
    {
        public static void WriteIndexItemPair(this Writer writer, IndexItemPair pair)
        {
            writer.WriteInt32(pair.slotIndex);
            writer.WriteIContainerItem(pair.item);
        }

        public static IndexItemPair ReadIndexItemPair(this Reader reader)
        {
            int slotIndex = reader.ReadInt32();
            IContainerItem item = reader.ReadIContainerItem();
            return new IndexItemPair(slotIndex, item);
        }
    }
}
#endif