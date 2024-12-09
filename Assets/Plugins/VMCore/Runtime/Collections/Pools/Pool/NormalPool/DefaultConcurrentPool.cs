using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace VMFramework.Core.Pools
{
    /// <summary>
    /// Threads-safe pool implementation using a <see cref="ConcurrentQueue{T}"/> to store item references.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public sealed partial class DefaultConcurrentPool<TItem> : NormalPool<TItem> where TItem : class
    {
        private readonly Func<TItem, TItem> _preGetFunc;
        private readonly Func<TItem> _createFunc;
        private readonly Func<TItem, bool> _returnFunc;
        private readonly Action<TItem> _clearFunc;
        private readonly int _capacity;
        private int _count;

        private readonly ConcurrentQueue<TItem> _items = new();
        private TItem _fastItem;

        public DefaultConcurrentPool(IPoolPolicy<TItem> policy) : this(policy, Environment.ProcessorCount * 4)
        {
        }

        public DefaultConcurrentPool(IPoolPolicy<TItem> policy, int capacity)
        {
            policy.AssertIsNotNull(nameof(policy));
            
            // cache the target interface methods, to avoid interface lookup overhead
            _preGetFunc = policy.PreGet;
            _createFunc = policy.Create;
            _returnFunc = policy.Return;
            _clearFunc = policy.Clear;
            _capacity = capacity - 1; // -1 to account for _fastItem
        }

        public override int Count => _count;
        public override int Capacity => _capacity + 1;

        public override TItem Get(out bool isFreshlyCreated)
        {
            var item = _fastItem;
            if (item == null || Interlocked.CompareExchange(ref _fastItem, null, item) != item)
            {
                if (_items.TryDequeue(out item))
                {
                    Interlocked.Decrement(ref _count);
                    isFreshlyCreated = false;
                    return _preGetFunc(item);
                }

                // no object available, so go get a brand new one
                isFreshlyCreated = true;
                return _preGetFunc(_createFunc());
            }

            isFreshlyCreated = false;
            return _preGetFunc(item);
        }

        public override bool Return(TItem item)
        {
            if (!_returnFunc(item))
            {
                // policy says to drop this object
                return false;
            }

            if (_fastItem != null || Interlocked.CompareExchange(ref _fastItem, item, null) != null)
            {
                if (Interlocked.Increment(ref _count) <= _capacity)
                {
                    _items.Enqueue(item);
                    return true;
                }

                // no room, clean up the count and drop the object on the floor
                Interlocked.Decrement(ref _count);
                return false;
            }

            return true;
        }

        public override void Clear()
        {
            _clearFunc?.Invoke(_fastItem);
            _fastItem = null;

            while (_items.TryDequeue(out var item))
            {
                _clearFunc?.Invoke(item);
            }

            Interlocked.Exchange(ref _count, 0);
        }

        public override IEnumerator<TItem> GetEnumerator()
        {
            if (_fastItem != null)
            {
                yield return _fastItem;
            }

            foreach (var item in _items)
            {
                yield return item;
            }
        }
    }
}