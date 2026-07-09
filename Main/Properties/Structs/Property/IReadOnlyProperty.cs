using System.Diagnostics.Contracts;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Properties
{
    public delegate void PropertyDirtyHandler(IReadOnlyProperty property, bool initial);

    public interface IReadOnlyProperty : INameOwner
    {
        public object Owner { get; }

        public object ObjectValue { get; }

        public event PropertyDirtyHandler OnDirty;
    }

    public interface IReadOnlyProperty<out TValue> : IReadOnlyProperty
    {
        object IReadOnlyProperty.ObjectValue => GetValue();
        
        [Pure]
        public TValue GetValue();
    }
}