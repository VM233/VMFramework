using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace VMFramework.Properties
{
    public class HashSetProperty<TValue> : ICollectionProperty<TValue>
    {
        public string Name { get; set; }

        public int Count => collection.Count;

        public object Owner { get; protected set; }

        public object ObjectValue => collection;

        public event PropertyDirtyHandler OnDirty;
        public event IReadOnlyCollectionProperty<TValue>.CollectionChangedHandler OnCollectionChanged;

        [ShowInInspector]
        protected readonly HashSet<TValue> collection = new();

        public void SetOwner(object owner)
        {
            Owner = owner;
        }

        public IEnumerable<TValue> GetValues() => collection;

        public void GetValues(ICollection<TValue> values)
        {
            foreach (var value in collection)
            {
                values.Add(value);
            }
        }
        
        public void SetObjectValue(object value, bool initial)
        {
            throw new NotSupportedException();
        }

        public bool Add(TValue value, bool initial)
        {
            bool added = collection.Add(value);
            if (added)
            {
                OnCollectionChanged?.Invoke(this, value, added: true, initial);
                OnDirty?.Invoke(this, initial);
            }

            return added;
        }

        public void AddRange(IEnumerable<TValue> values, bool initial)
        {
            bool dirty = false;
            foreach (var value in values)
            {
                if (collection.Add(value))
                {
                    OnCollectionChanged?.Invoke(this, value, added: true, initial);
                    dirty = true;
                }
            }

            if (dirty)
            {
                OnDirty?.Invoke(this, initial);
            }
        }

        public bool Remove(TValue value, bool initial)
        {
            bool removed = collection.Remove(value);
            if (removed)
            {
                OnCollectionChanged?.Invoke(this, value, added: false, initial);
                OnDirty?.Invoke(this, initial);
            }

            return removed;
        }

        public void Clear()
        {
            if (collection.Count <= 0)
            {
                return;
            }

            foreach (var value in collection)
            {
                OnCollectionChanged?.Invoke(this, value, added: false, false);
            }

            collection.Clear();
            OnDirty?.Invoke(this, false);
        }
        
        public bool Contains(TValue value)
        {
            return collection.Contains(value);
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}