namespace VMFramework.Properties
{
    public delegate void PropertyChangedHandler<in TValue>(IReadOnlyProperty property, TValue previous, TValue current,
        bool initial);
    
    public interface IReadOnlyValueProperty<out TValue> : IReadOnlyProperty<TValue>
    {
        public event PropertyChangedHandler<TValue> OnChanged;
    }
}