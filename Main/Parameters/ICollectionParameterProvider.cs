using System.Collections.Generic;

namespace VMFramework.Parameters
{
    public interface ICollectionParameterProvider
    {
        public bool TryGetCollectionParameter<TValue>(string key, ICollection<TValue> collection);
    }
}