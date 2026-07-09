using System;
using UnityEngine;
using VMFramework.OdinExtensions;

namespace VMFramework.Parameters
{
    [Serializable]
    public class SequenceFloatToIntMapSourceConfig : IParameterSourceConfig<int>
    {
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> floatSourceConfig = new ParameterSourceConfig<float>();

        public float floatOffset = -0.5f;

        public float floatScale = 1;

        public int intOffset = 0;

        public int intScale = 1;

        public IParameterSource<int> GetParameterSource()
        {
            return new SequenceFloatToIntMapSource(floatSourceConfig.GetParameterSource(), floatOffset, floatScale,
                intOffset, intScale);
        }
    }
}