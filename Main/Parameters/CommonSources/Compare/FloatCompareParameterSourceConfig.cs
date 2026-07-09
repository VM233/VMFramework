using System;
using UnityEngine;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.Parameters
{
    [Serializable]
    public class FloatCompareParameterSourceConfig : IParameterSourceConfig<bool>
    {
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> sourceConfig = new ParameterSourceConfig<float>();

        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> comparandConfig = new FixedValueParameterSourceConfig<float>();

        public CompareResult compareResult = CompareResult.Equal;

        public virtual IParameterSource<bool> GetParameterSource()
        {
            return new FloatCompareParameterSource(sourceConfig.GetParameterSource(),
                comparandConfig.GetParameterSource(), compareResult);
        }
    }
}