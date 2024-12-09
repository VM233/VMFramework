using System.Collections;
using System.Collections.Generic;

namespace VMFramework.Core.Pools
{
    public abstract class NormalPool<TItem> : INormalPool<TItem>
    {
        public abstract int Count { get; }
        public abstract int Capacity { get; }

        public abstract TItem Get(out bool isFreshlyCreated);

        public abstract bool Return(TItem item);

        public abstract void Clear();
        
        public abstract IEnumerator<TItem> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}