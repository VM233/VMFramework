#if FISHNET
namespace VMFramework.Containers
{
    public readonly struct IndexItemPair
    {
        public readonly int slotIndex;
        public readonly IContainerItem item;

        public IndexItemPair(int slotIndex, IContainerItem item)
        {
            this.slotIndex = slotIndex;
            this.item = item;
        }
        
        public void Deconstruct(out int slotIndex, out IContainerItem item)
        {
            slotIndex = this.slotIndex;
            item = this.item;
        }
    }
}
#endif