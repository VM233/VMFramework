using System.Collections.Generic;

namespace VMFramework.Parameters
{
    public struct CollectionParameterSource<TType> : ICollectionParametersSource<TType>
    {
        public ICollectionParameterProvider provider;
        public string parameterName;

        public CollectionParameterSource(ICollectionParameterProvider provider, string parameterName)
        {
            this.provider = provider;
            this.parameterName = parameterName;
        }

        public bool TryGetValues(ICollection<TType> collection)
        {
            return provider.TryGetCollectionParameter(parameterName, collection);
        }
    }
}