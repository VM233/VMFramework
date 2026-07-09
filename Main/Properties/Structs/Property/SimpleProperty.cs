using System;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.Properties
{
    [PreviewComposite]
    public class SimpleProperty<TValue> : IValueProperty<TValue>, IResettable
    {
        public delegate bool CanSetValueHandler(object owner, TValue previous, TValue next, bool initial);

        public string Name { get; set; }
        
        public object Owner { get; protected set; }

        protected TValue value;

        [ShowInInspector]
        [DelayedProperty]
        public TValue Value
        {
            get => value;
            set => SetValue(value, initial: false);
        }
        
        public int Priority => PriorityDefines.HIGH;

        public event PropertyDirtyHandler OnDirty;
        public event PropertyChangedHandler<TValue> OnChanged;

        public CanSetValueHandler canSetValueFunc;
        public Func<TValue> initialValueGetter;

        public virtual void SetInitialValue(TValue value)
        {
            initialValueGetter = () => value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetOwner(object owner)
        {
            Owner = owner;
        }

        public virtual void SetObjectValue(object value, bool initial)
        {
            SetValue((TValue)value, initial);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SetValue(TValue value, bool initial)
        {
            if (canSetValueFunc != null && canSetValueFunc(Owner, previous: this.value, next: value, initial) == false)
            {
                return;
            }

            var previous = this.value;
            this.value = value;
            OnChanged?.Invoke(this, previous, this.value, initial);
            OnDirty?.Invoke(this, initial);
        }

        public virtual TValue GetValue()
        {
            return value;
        }
        
        public bool TryReset()
        {
            if (initialValueGetter == null)
            {
                return false;
            }

            var newValue = initialValueGetter();
            SetValue(newValue, initial: true);
            return true;
        }

        public override string ToString()
        {
            if (value == null)
            {
                return "Null";
            }
            
            return value.ToString();
        }

        public static implicit operator TValue(SimpleProperty<TValue> property) => property.value;
    }
}