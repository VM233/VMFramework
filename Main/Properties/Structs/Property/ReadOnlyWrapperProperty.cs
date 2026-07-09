using System;

namespace VMFramework.Properties
{
    public class ReadOnlyWrapperProperty<TValue> : IReadOnlyValueProperty<TValue>, IProperty
    {
        public string Name => WrappedProperty?.Name;

        public IReadOnlyValueProperty<TValue> WrappedProperty { get; protected set; }

        public object Owner { get; protected set; }

        public object ObjectValue => WrappedProperty.ObjectValue;

        public TValue DefaultValue { get; set; } = default;

        public event PropertyDirtyHandler OnDirty;

        public event PropertyChangedHandler<TValue> OnChanged;

        protected readonly PropertyChangedHandler<TValue> onWrapperChangedFunc;

        protected TValue lastValue;

        public ReadOnlyWrapperProperty()
        {
            onWrapperChangedFunc = OnWrapperChanged;
        }

        public void SetOwner(object owner)
        {
            Owner = owner;
        }
        
        public void SetObjectValue(object value, bool initial)
        {
            throw new NotSupportedException();
        }

        public void Set(IReadOnlyValueProperty<TValue> property)
        {
            bool isDirty = false;
            TValue currentValue = DefaultValue;

            if (WrappedProperty != null)
            {
                isDirty = true;
                WrappedProperty.OnChanged -= onWrapperChangedFunc;
            }

            WrappedProperty = property;

            if (property != null)
            {
                currentValue = property.GetValue();
                property.OnChanged += onWrapperChangedFunc;
                isDirty = true;
            }

            if (isDirty)
            {
                OnDirty?.Invoke(this, true);
                OnChanged?.Invoke(this, lastValue, currentValue, true);
            }
        }

        protected void OnWrapperChanged(object owner, TValue previous, TValue current, bool initial)
        {
            OnDirty?.Invoke(this, initial);
            OnChanged?.Invoke(this, previous, current, initial);
            lastValue = current;
        }

        public TValue GetValue()
        {
            return WrappedProperty.GetValue();
        }
    }
}