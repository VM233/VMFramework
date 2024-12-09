using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VMFramework.Core.Pools
{
    public sealed class KeyBasedPool<TItem, TKey, TPool> : IKeyBasedPool<TKey, TItem>
        where TPool : INormalPool<TItem>
        where TItem : IIDOwner<TKey>
    {
        private readonly Dictionary<TKey, TPool> poolsDictionary = new();
        private readonly Func<TKey, TPool> createPoolFunc;

        public KeyBasedPool(Func<TKey, TPool> createPoolFunc)
        {
            this.createPoolFunc = createPoolFunc;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Return(TItem item)
        {
            if (poolsDictionary.TryGetValue(item.id, out var pool) == false)
            {
                pool = createPoolFunc(item.id);
                poolsDictionary.Add(item.id, pool);
            }
            
            return pool.Return(item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            foreach (var pool in poolsDictionary.Values)
            {
                pool.Clear();
            }

            poolsDictionary.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TItem Get(TKey key, out bool isFreshlyCreated)
        {
            if (poolsDictionary.TryGetValue(key, out var pool) == false)
            {
                pool = createPoolFunc(key);
                poolsDictionary.Add(key, pool);
            }

            return pool.Get(out isFreshlyCreated);
        }
    }
}