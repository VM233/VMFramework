using System;
using UnityEngine;
using VMFramework.OdinExtensions;

namespace VMFramework.Parameters
{
    [Serializable]
    public class ConditionalParameterSourceConfig<TValue> : IParameterSourceConfig<TValue>
    {
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<TValue> sourceConfig = new ParameterSourceConfig<TValue>();

        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<bool> conditionConfig = new ParameterSourceConfig<bool>();

        public IParameterSource<TValue> GetParameterSource()
        {
            return new ConditionalParameterSource<TValue>(sourceConfig.GetParameterSource(),
                conditionConfig.GetParameterSource());
        }
    }
}