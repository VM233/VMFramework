using System;
using Sirenix.OdinInspector;

namespace VMFramework.Parameters
{
    [Serializable]
    public class FixedValueParameterSourceConfig<TType> : IParameterSourceConfig<TType>
    {
        [Required]
        public TType value;

        public FixedValueParameterSourceConfig()
        {

        }

        public FixedValueParameterSourceConfig(TType value)
        {
            this.value = value;
        }

        public IParameterSource<TType> GetParameterSource()
        {
            return new FixedValueParameterSource<TType>(value);
        }
    }
}