namespace VMFramework.Properties
{
    public interface IProperty : IReadOnlyProperty
    {
        public void SetObjectValue(object value, bool initial);
        
        public void SetOwner(object owner);
    }

    public interface IProperty<TValue> : IProperty, IReadOnlyProperty<TValue>
    {
        public TValue Value { get; set; }

        public void SetValue(TValue value, bool initial);
    }
}