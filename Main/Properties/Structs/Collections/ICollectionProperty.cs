using System.Collections.Generic;

namespace VMFramework.Properties
{
    public interface ICollectionProperty<TValue> : IProperty, IReadOnlyCollectionProperty<TValue>
    {
        public bool Add(TValue value, bool initial);
        
        public bool Remove(TValue value, bool initial);
        
        public void AddRange(IEnumerable<TValue> values, bool initial);
        
        public void Clear();
    }
}