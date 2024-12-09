namespace VMFramework.Core.Pools
{
    public sealed class DefaultPoolPolicy<TItem> : PoolPolicy<TItem> where TItem : class, new()
    {
        public override TItem PreGet(TItem item) => item;

        public override TItem Create()
        {
            return new TItem();
        }

        public override bool Return(TItem item)
        {
            if (item is IResettable resettable)
            {
                return resettable.TryReset();
            }
            
            return true;
        }

        public override void Clear(TItem item)
        {
            
        }
    }
}