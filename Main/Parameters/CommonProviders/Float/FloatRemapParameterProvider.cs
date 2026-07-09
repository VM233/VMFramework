using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.Parameters
{
    [ParameterDefine(VALUE_PARAMETER, typeof(float))]
    public class FloatRemapParameterProvider : MonoBehaviour, IParameterProvider
    {
        public const string VALUE_PARAMETER = "Value";

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> sourceConfig = new ParameterSourceConfig<float>();

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> sourceStartConfig;

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> sourceEndConfig;

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> targetMinConfig;

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> targetMaxConfig;

        protected IParameterSource<float> source;
        protected IParameterSource<float> sourceStart;
        protected IParameterSource<float> sourceEnd;
        protected IParameterSource<float> targetMin;
        protected IParameterSource<float> targetMax;

        protected virtual void Awake()
        {
            source = sourceConfig.GetParameterSource();
            sourceStart = sourceStartConfig.GetParameterSource();
            sourceEnd = sourceEndConfig.GetParameterSource();
            targetMin = targetMinConfig.GetParameterSource();
            targetMax = targetMaxConfig.GetParameterSource();
        }

        public virtual bool TryGetValue(out float value)
        {
            value = 0;

            if (source.TryGetValue(out var sourceValue) == false)
            {
                return false;
            }

            if (sourceStart.TryGetValue(out var sourceStartValue) == false)
            {
                return false;
            }

            if (sourceEnd.TryGetValue(out var sourceEndValue) == false)
            {
                return false;
            }

            if (targetMin.TryGetValue(out var targetMinValue) == false)
            {
                return false;
            }

            if (targetMax.TryGetValue(out var targetMaxValue) == false)
            {
                return false;
            }

            value = sourceValue.NormalizeTo(sourceStartValue, sourceEndValue, targetMinValue, targetMaxValue);
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