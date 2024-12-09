namespace VMFramework.Core.Pools
{
    public class CollectionPool<TCollection> where TCollection : class, new()
    {
        /// <summary>
        /// A pool that can be used by a single thread. The pool is not thread-safe.
        /// </summary>
        public static DefaultPool<TCollection> Default { get; } = new(new DefaultPoolPolicy<TCollection>(), 500);

        /// <summary>
        /// A pool that can be shared across multiple threads. The pool is thread-safe.
        /// </summary>
        public static DefaultConcurrentPool<TCollection> Shared { get; } = new(new DefaultPoolPolicy<TCollection>());
    }
}