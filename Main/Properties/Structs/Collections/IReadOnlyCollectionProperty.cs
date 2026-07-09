using System.Collections.Generic;

namespace VMFramework.Properties
{
    public interface IReadOnlyCollectionProperty<TValue> : IReadOnlyProperty, IReadOnlyCollection<TValue>
    {
        public delegate void CollectionChangedHandler(IReadOnlyProperty property, TValue value, bool added, bool initial);
        
        public event CollectionChangedHandler OnCollectionChanged;

        public void GetValues(ICollection<TValue> values);

        public bool Contains(TValue value);
    }
}