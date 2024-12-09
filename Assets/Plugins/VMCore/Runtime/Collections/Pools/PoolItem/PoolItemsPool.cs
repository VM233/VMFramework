using System.Collections.Generic;

namespace VMFramework.Core.Pools
{
    /// <summary>
    /// This pool is not thread-safe.
    /// </summary>
    public abstract class PoolItemsPool<TPoolItem> : NormalPool<TPoolItem> where TPoolItem : IPoolItem
    {
        protected readonly Queue<TPoolItem> pool = new();
        
        public override int Count => pool.Count;

        protected abstract TPoolItem CreateItem();
        
        public override int Capacity { get; }
        
        protected PoolItemsPool(int capacity)
        {
            Capacity = capacity;
        }
        
        public sealed override TPoolItem Get(out bool isFreshlyCreated)
        {
            if (pool.TryDequeue(out TPoolItem item))
            {
                isFreshlyCreated = false;
                item.OnGet();
                return item;
            }
            
            item = CreateItem();
            isFreshlyCreated = true;
            
            return item;
        }

        public sealed override bool Return(TPoolItem item)
        {
            if (pool.Count >= Capacity)
            {
                item.OnClear();
                return false;
            }
            
            pool.Enqueue(item);
            item.OnReturn();
            return true;
        }

        public sealed override void Clear()
        {
            foreach (var item in pool)
            {
                item.OnClear();
            }
            
            pool.Clear();
        }

        public sealed override IEnumerator<TPoolItem> GetEnumerator() => pool.GetEnumerator();
    }
}