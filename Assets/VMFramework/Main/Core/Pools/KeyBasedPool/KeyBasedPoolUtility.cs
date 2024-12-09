using System.Runtime.CompilerServices;

namespace VMFramework.Core.Pools
{
    public static class KeyBasedPoolUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TItem Get<TKey, TItem>(this IKeyBasedPool<TKey, TItem> pool, TKey key)
        {
            return pool.Get(key, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Prewarm<TKey, TItem>(this IKeyBasedPool<TKey, TItem> pool, TKey key, int count)
        {
            var temp = ListPool<TItem>.Shared.Get();
            
            for (int i = 0; i < count; i++)
            {
                var item = pool.Get(key);
                temp.Add(item);
            }
            
            foreach (var item in temp)
            {
                pool.Return(item);
            }
            
            temp.ReturnToSharedPool();
        }
    }
}