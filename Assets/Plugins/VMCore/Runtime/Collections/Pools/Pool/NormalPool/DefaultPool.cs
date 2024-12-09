using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VMFramework.Core.Pools
{
    /// <summary>
    /// This pool is not thread-safe.
    /// If you need a thread-safe version, use the <see cref="DefaultConcurrentPool{TItem}"/> instead.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public sealed partial class DefaultPool<TItem> : NormalPool<TItem>, ICheckablePool<TItem>
    {
        private readonly Func<TItem, TItem> _preGetFunc;
        private readonly Func<TItem> _createFunc;
        private readonly Func<TItem, bool> _returnFunc;
        private readonly Action<TItem> _clearFunc;
        private readonly Queue<TItem> _items = new();
        
        public override int Count => _items.Count;
        
        public override int Capacity { get; }

        public DefaultPool(IPoolPolicy<TItem> policy) : this(policy, 36.Max(Environment.ProcessorCount * 4))
        {
            
        }
        
        public DefaultPool(IPoolPolicy<TItem> policy, int capacity)
        {
            policy.AssertIsNotNull(nameof(policy));
            
            // cache the target interface methods, to avoid interface lookup overhead
            _preGetFunc = policy.PreGet;
            _createFunc = policy.Create;
            _returnFunc = policy.Return;
            _clearFunc = policy.Clear;
            Capacity = capacity;
        }
        
        public override TItem Get(out bool isFreshlyCreated)
        {
            if (_items.TryDequeue(out var result))
            {
                isFreshlyCreated = false;
                return _preGetFunc(result);
            }
            
            isFreshlyCreated = true;
            return _preGetFunc(_createFunc());
        }

        public override bool Return(TItem item)
        {
            if (_returnFunc(item) == false)
            {
                _clearFunc(item);
                return false;
            }
            
            if (_items.Count >= Capacity)
            {
                _clearFunc(item);
                return false;
            }
            
            _items.Enqueue(item);
            return true;
        }

        public override void Clear()
        {
            foreach (var item in _items)
            {
                _clearFunc(item);
            }
            
            _items.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(TItem item) => _items.Contains(item);

        public override IEnumerator<TItem> GetEnumerator() => _items.GetEnumerator();
    }
}