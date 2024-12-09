namespace VMFramework.Core.Pools
{
    public interface INormalPool<TItem> : IPool<TItem>, IReadOnlyLimitedCollection<TItem>
    {
        /// <summary>
        /// Get an item from the pool, if there is no item in the pool, a new item will be created automatically,
        /// and the isFreshlyCreated variable will be returned to indicate whether it is a newly created item.
        /// </summary>
        /// <param name="isFreshlyCreated"></param>
        /// <returns></returns>
        public TItem Get(out bool isFreshlyCreated);
    }
}