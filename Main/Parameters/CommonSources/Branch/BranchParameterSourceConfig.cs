using System;
using UnityEngine;
using VMFramework.OdinExtensions;

namespace VMFramework.Parameters
{
    [Serializable]
    public class BranchParameterSourceConfig<TValue> : IParameterSourceConfig<TValue>
    {
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<TValue> trueSourceConfig = new ParameterSourceConfig<TValue>();

        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<TValue> falseSourceConfig = new ParameterSourceConfig<TValue>();

        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<bool> conditionConfig = new ParameterSourceConfig<bool>();

        public IParameterSource<TValue> GetParameterSource()
        {
            return new BranchParameterSource<TValue>(trueSourceConfig.GetParameterSource(),
                falseSourceConfig.GetParameterSource(), conditionConfig.GetParameterSource());
        }
    }
}