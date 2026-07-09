using System;
using VMFramework.Configuration;
using VMFramework.OdinExtensions;

namespace VMFramework.Parameters
{
    [Serializable]
    public class CollectionParameterSourceConfig<TType>
        : ICollectionParametersSourceConfig<TType>, ICollectionParameterProviderOwner
    {
        public TargetReferenceConfig<ICollectionParameterProvider> target = new(TargetReferenceType.Component);

        [ParameterName(nameof(valueType), isCollection: true)]
        [IsNotNullOrEmpty]
        public string parameterName;

        private static readonly Type valueType = typeof(TType);

        public ICollectionParameterProvider CollectionParameterProvider => target.Target;

        public ICollectionParametersSource<TType> GetCollectionParameterSource()
        {
            var collectionParameterProvider = CollectionParameterProvider;

            if (collectionParameterProvider == null)
            {
                throw new ArgumentNullException(nameof(collectionParameterProvider));
            }

            return new CollectionParameterSource<TType>(collectionParameterProvider, parameterName);
        }
    }
}