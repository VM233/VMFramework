namespace VMFramework.Properties
{
    public delegate void PropertyChangedHandler<in TKey, in TValue>(IReadOnlyProperty property, bool hasPrevious,
        TValue previous, bool hasCurrent, TValue current, bool initial, TKey key);

    public interface IReadOnlyKeyedProperty<in TKey> : IReadOnlyProperty
    {
        public object GetObjectValue(TKey key);
        
        public bool ContainsKey(TKey key);
    }

    public interface IReadOnlyKeyedProperty<TKey, out TValue> : IReadOnlyKeyedProperty<TKey>
    {
        public event PropertyChangedHandler<TKey, TValue> OnChanged;
        
        public TValue GetValue(TKey key);

        object IReadOnlyKeyedProperty<TKey>.GetObjectValue(TKey key) => GetValue(key);
    }
}