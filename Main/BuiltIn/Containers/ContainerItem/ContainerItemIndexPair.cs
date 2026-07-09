namespace VMFramework.Containers
{
    public struct ContainerItemIndexPair
    {
        public int index;
        public IContainerItem item;

        public ContainerItemIndexPair(int index, IContainerItem item)
        {
            this.index = index;
            this.item = item;
        }

        public void Deconstruct(out int index, out IContainerItem item)
        {
            index = this.index;
            item = this.item;
        }
    }
}