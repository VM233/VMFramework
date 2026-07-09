using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Core.Pools;

namespace VMFramework.Properties
{
    public class DictionaryProperty<TKey, TValue> : IDictionaryProperty<TKey, TValue>
    {
        public string Name { get; set; }

        public IReadOnlyDictionary<TKey, TValue> Dictionary => dict;

        public object ObjectValue => dict;

        public int Count => dict.Count;

        public TValue this[TKey key]
        {
            get => GetValue(key);
            set => SetValue(key, value, initial: false);
        }

        public object Owner { get; protected set; }

        public event PropertyDirtyHandler OnDirty;

        public event IReadOnlyCollectionProperty<KeyValuePair<TKey, TValue>>.CollectionChangedHandler
            OnCollectionChanged;

        public event PropertyChangedHandler<TKey, TValue> OnChanged;

        [ShowInInspector]
        [DictionaryDrawerSettings(IsReadOnly = true)]
        protected readonly Dictionary<TKey, TValue> dict = new();

        public void SetOwner(object owner)
        {
            Owner = owner;
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> GetValues() => dict;

        public TValue GetValue(TKey key) => dict.GetValueOrDefault(key);

        public bool ContainsKey(TKey key)
        {
            return dict.ContainsKey(key);
        }

        public void GetValues(ICollection<KeyValuePair<TKey, TValue>> values)
        {
            foreach (var kvp in dict)
            {
                values.Add(kvp);
            }
        }

        public void SetObjectValue(object value, bool initial)
        {
            throw new NotSupportedException();
        }

        public void SetValue(TKey key, TValue value, bool initial)
        {
            bool oldValueExists = dict.TryGetValue(key, out var previous);

            dict[key] = value;

            if (oldValueExists == false)
            {
                OnCollectionChanged?.Invoke(property: this, new KeyValuePair<TKey, TValue>(key, value), added: true,
                    initial);
            }

            OnChanged?.Invoke(property: this, hasPrevious: oldValueExists, previous, hasCurrent: true, current: value,
                initial, key);
            OnDirty?.Invoke(property: this, initial);
        }

        public void ChangeToValues(IReadOnlyDictionary<TKey, TValue> newValues, bool initial)
        {
            var keysToRemove = HashSetPool<TKey>.Default.Get();
            keysToRemove.Clear();
            keysToRemove.UnionWith(dict.Keys);
            keysToRemove.ExceptWith(newValues.Keys);

            foreach (var key in keysToRemove)
            {
                Remove(key, initial, out _);
            }

            foreach (var (key, value) in newValues)
            {
                SetValue(key, value, initial);
            }

            keysToRemove.ReturnToDefaultPool();
        }

        public bool Add(KeyValuePair<TKey, TValue> value, bool initial)
        {
            bool added = dict.TryAdd(value.Key, value.Value);
            if (added)
            {
                OnCollectionChanged?.Invoke(property: this, value, added: true, initial);
                OnChanged?.Invoke(property: this, hasPrevious: false, previous: default, hasCurrent: true,
                    current: value.Value, initial, value.Key);
                OnDirty?.Invoke(property: this, initial);
            }

            return added;
        }

        public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> values, bool initial)
        {
            bool dirty = false;
            foreach (var value in values)
            {
                if (dict.TryAdd(value.Key, value.Value))
                {
                    OnCollectionChanged?.Invoke(property: this, value, added: true, initial);
                    OnChanged?.Invoke(property: this, hasPrevious: false, previous: default, hasCurrent: true,
                        current: value.Value, initial, value.Key);
                    dirty = true;
                }
            }

            if (dirty)
            {
                OnDirty?.Invoke(property: this, initial);
            }
        }

        public bool Remove(TKey key, bool initial, out TValue value)
        {
            bool removed = dict.Remove(key, out value);

            if (removed)
            {
                OnChanged?.Invoke(property: this, hasPrevious: true, previous: value, hasCurrent: false,
                    current: default, initial, key);
                OnCollectionChanged?.Invoke(property: this, new KeyValuePair<TKey, TValue>(key, value), added: false,
                    initial);
                OnDirty?.Invoke(property: this, initial);
            }

            return removed;
        }

        bool ICollectionProperty<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> value, bool initial)
        {
            return Remove(value.Key, initial, out _);
        }

        public virtual bool Contains(KeyValuePair<TKey, TValue> value)
        {
            if (dict.TryGetValue(value.Key, out var currentValue))
            {
                return EqualityComparer<TValue>.Default.Equals(currentValue, value.Value);
            }

            return false;
        }

        public void Clear()
        {
            if (dict.Count <= 0)
            {
                return;
            }

            foreach (var value in dict)
            {
                OnChanged?.Invoke(property: this, hasPrevious: true, previous: value.Value, hasCurrent: false,
                    current: default, initial: false, value.Key);
                OnCollectionChanged?.Invoke(property: this, value, added: false, initial: false);
            }

            dict.Clear();
            OnDirty?.Invoke(this, initial: false);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}