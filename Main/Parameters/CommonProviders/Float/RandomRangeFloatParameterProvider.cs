using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.Parameters
{
    [ParameterDefine(VALUE_PARAMETER, typeof(float))]
    public class RandomRangeFloatParameterProvider : MonoBehaviour, IParameterProvider
    {
        public const string VALUE_PARAMETER = "Value";

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> minConfig = new ParameterSourceConfig<float>();

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> maxConfig = new ParameterSourceConfig<float>();

        protected IParameterSource<float> minSource;
        protected IParameterSource<float> maxSource;

        protected virtual void Awake()
        {
            minSource = minConfig.GetParameterSource();
            maxSource = maxConfig.GetParameterSource();
        }

        public virtual bool TryGetValue(out float value)
        {
            value = 0;
            if (minSource.TryGetValue(out var min) == false)
            {
                return false;
            }

            if (maxSource.TryGetValue(out var max) == false)
            {
                return false;
            }

            value = GlobalRandom.Default.Range(min, max);
            return true;
        }

        public virtual bool TryGetParameter<TValue>(string key, ref TValue value)
        {
            if (key == VALUE_PARAMETER)
            {
                if (TryGetValue(out var floatValue))
                {
                    if (floatValue is TValue validValue)
                    {
                        value = validValue;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}