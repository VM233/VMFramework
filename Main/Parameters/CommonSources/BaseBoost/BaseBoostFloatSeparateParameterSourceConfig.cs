using System;
using UnityEngine;
using VMFramework.OdinExtensions;
using VMFramework.Properties;

namespace VMFramework.Parameters
{
    [Serializable]
    public class BaseBoostFloatSeparateParameterSourceConfig : IParameterSourceConfig<BaseBoostFloat>
    {
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> baseConfig = new ParameterSourceConfig<float>();

        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> boostConfig = new FixedValueParameterSourceConfig<float>(0);

        public IParameterSource<BaseBoostFloat> GetParameterSource()
        {
            return new BaseBoostFloatSeparateParameterSource(baseConfig.GetParameterSource(),
                boostConfig.GetParameterSource());
        }
    }
}