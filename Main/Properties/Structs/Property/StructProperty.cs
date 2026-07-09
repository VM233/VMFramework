using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using VMFramework.OdinExtensions;

namespace VMFramework.Properties
{
    public delegate void StructPropertyChangedHandler<in TOwner, in T>(TOwner owner, T previous, T current,
        bool initial);

    [PreviewComposite]
    public class StructProperty<TOwner, T>
        where TOwner : class
        where T : struct
    {
        public TOwner Owner { get; private set; }

        private T value;

        [ShowInInspector]
        [DelayedProperty]
        public T Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => value;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                var previous = this.value;
                this.value = value;
                OnValueChanged?.Invoke(Owner, previous, value, initial: false);
            }
        }

        public event StructPropertyChangedHandler<TOwner, T> OnValueChanged;

        public StructProperty(T value)
        {
            this.value = value;
            OnValueChanged = null;
        }

        public void SetOwner(TOwner owner)
        {
            this.Owner = owner;
        }

        [Button]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(T value, bool initial)
        {
            var previous = this.value;
            this.value = value;
            OnValueChanged?.Invoke(Owner, previous, value, initial);
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public static implicit operator T(StructProperty<TOwner, T> property) => property.Value;
    }
}