using System.Collections.Generic;

namespace VMFramework.Parameters
{
    public interface ICollectionParametersSource<TType>
    {
        public bool TryGetValues(ICollection<TType> collection);
    }
}