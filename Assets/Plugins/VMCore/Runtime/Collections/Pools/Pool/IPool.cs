using System.Collections.Generic;

namespace VMFramework.Core.Pools
{
    public interface IPool<in TItem>
    {
        /// <summary>
        /// Returns an item to the pool.
        /// </summary>
        /// <returns>true if the item was returned to the pool</returns>
        public bool Return(TItem item);

        /// <summary>
        /// Clear the pool, all items in the pool will be destroyed.
        /// </summary>
        public void Clear();
    }
}
