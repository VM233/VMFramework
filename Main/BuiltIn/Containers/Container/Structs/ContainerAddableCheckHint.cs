namespace VMFramework.Containers
{
    public struct ContainerAddableCheckHint
    {
        public bool ignoreExistingItems;
        
        public ContainerAddableCheckHint(bool ignoreExistingItems)
        {
            this.ignoreExistingItems = ignoreExistingItems;
        }
    }
}