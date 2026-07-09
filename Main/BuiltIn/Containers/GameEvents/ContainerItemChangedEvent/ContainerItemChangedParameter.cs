namespace VMFramework.Containers
{
    public readonly struct ContainerItemChangedParameter
    {
        public readonly IContainer container;

        public readonly int slotIndex;

        public readonly IContainerItem item;
        
        public ContainerItemChangedParameter(IContainer container, int slotIndex, IContainerItem item)
        {
            this.container = container;
            this.slotIndex = slotIndex;
            this.item = item;
        }
    }
}