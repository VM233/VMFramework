using System;
using VMFramework.Configuration;

namespace VMFramework.Parameters
{
    [Serializable]
    public class FixedReferenceValueParameterSourceConfig<TType> : IParameterSourceConfig<TType>
        where TType : class
    {
        public TargetReferenceConfig<TType> targetConfig = new();

        public IParameterSource<TType> GetParameterSource()
        {
            return new FixedValueParameterSource<TType>(targetConfig.Target);
        }
    }
}