using System.Collections.Generic;

namespace VMFramework.Properties
{
    public interface IReadOnlyDictionaryProperty<TKey, TValue>
        : IReadOnlyCollectionProperty<KeyValuePair<TKey, TValue>>, IReadOnlyKeyedProperty<TKey, TValue>
    {

    }
}