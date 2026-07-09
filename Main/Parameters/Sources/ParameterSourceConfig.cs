using System;
using VMFramework.Configuration;
using VMFramework.OdinExtensions;

namespace VMFramework.Parameters
{
    [Serializable]
    public class ParameterSourceConfig<TType> : IParameterProviderOwner, IParameterSourceConfig<TType>
    {
        public TargetReferenceConfig<IParameterProvider> target = new(TargetReferenceType.Component);

        [ParameterName(nameof(valueType), isCollection: false)]
        [IsNotNullOrEmpty]
        public string parameterName;

        private static readonly Type valueType = typeof(TType);

        public IParameterProvider ParameterProvider => target.Target;

        public virtual IParameterSource<TType> GetParameterSource()
        {
            var parameterProvider = ParameterProvider;
            if (parameterProvider == null)
            {
                throw new ArgumentNullException(nameof(ParameterProvider));
            }

            return new ParameterSource<TType>(parameterProvider, parameterName);
        }
    }
}