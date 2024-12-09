namespace VMFramework.Core.Pools
{
    /// <summary>
    /// Represents a policy for managing pooled items.
    /// </summary>
    public interface IPoolPolicy<TItem>
    {
        /// <summary>
        /// Runs some processing when an item is about to be retrieved from the pool.
        /// </summary>
        /// <param name="item"></param>
        public TItem PreGet(TItem item);
        
        /// <summary>
        /// Create a <typeparamref name="TItem"/>.
        /// </summary>
        /// <returns>The <typeparamref name="TItem"/> which was created.</returns>
        public TItem Create();
 
        /// <summary>
        /// Runs some processing when an item was returned to the pool.
        /// Can be used to reset the state of an item and indicate if the item should be returned to the pool.
        /// </summary>
        /// <param name="item">The item item to return to the pool.</param>
        /// <returns>
        /// <see langword="true" /> if the item should be returned to the pool.
        /// <see langword="false" /> if it's not possible/desirable for the pool to keep the item.
        /// </returns>
        public bool Return(TItem item);

        public void Clear(TItem item);
    }
}