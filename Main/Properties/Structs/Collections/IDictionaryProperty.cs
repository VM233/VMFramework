#nullable enable
using System.Collections.Generic;

namespace VMFramework.Properties
{
    public interface IDictionaryProperty<TKey, TValue>
        : IReadOnlyDictionaryProperty<TKey, TValue>, ICollectionProperty<KeyValuePair<TKey, TValue>>,
            IKeyedProperty<TKey, TValue>
    {
        public bool Remove(TKey key, bool initial, out TValue value);
    }
}