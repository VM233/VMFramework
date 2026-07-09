using System;
using UnityEngine;
using VMFramework.OdinExtensions;

namespace VMFramework.Parameters
{
    [Serializable]
    public class BoolSwitchParameterSourceConfig<TValue> : IParameterSourceConfig<TValue>
    {
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<bool> boolSourceConfig = new ParameterSourceConfig<bool>();

        [SerializeReference]
        public IParameterSourceConfig<TValue> trueSourceConfig = new ParameterSourceConfig<TValue>();

        [SerializeReference]
        public IParameterSourceConfig<TValue> falseSourceConfig = new ParameterSourceConfig<TValue>();

        public IParameterSource<TValue> GetParameterSource()
        {
            return new BoolSwitchParameterSource<TValue>(boolSourceConfig.GetParameterSource(),
                trueSourceConfig?.GetParameterSource(), falseSourceConfig?.GetParameterSource());
        }
    }
}