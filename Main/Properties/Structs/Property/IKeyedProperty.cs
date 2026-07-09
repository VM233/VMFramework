namespace VMFramework.Properties
{
    public interface IKeyedProperty<TKey, TValue> : IProperty, IReadOnlyKeyedProperty<TKey, TValue>
    {
        public TValue this[TKey argument] { get; set; }
        
        public void SetValue(TKey key, TValue value, bool initial);
    }
}