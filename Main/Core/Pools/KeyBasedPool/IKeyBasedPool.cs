namespace VMFramework.Core.Pools
{
    public interface IKeyBasedPool<in TKey, TItem> : IPool<TItem>
    {
        public TItem Get(TKey key, out bool isFreshlyCreated);
    }
}