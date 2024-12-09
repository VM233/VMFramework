namespace VMFramework.Core.Pools
{
    public abstract class PoolPolicy<TItem> : IPoolPolicy<TItem>
    {
        public abstract TItem PreGet(TItem item);

        public abstract TItem Create();
        
        public abstract bool Return(TItem item);

        public abstract void Clear(TItem item);
    }
}