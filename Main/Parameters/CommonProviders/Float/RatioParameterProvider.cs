using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.Parameters
{
    [ParameterDefine(RATIO_PARAMETER, typeof(float))]
    public class RatioParameterProvider : MonoBehaviour, IParameterProvider
    {
        public const string RATIO_PARAMETER = "Ratio";

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> currentConfig = new ParameterSourceConfig<float>();

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> totalConfig = new ParameterSourceConfig<float>();
        
        [TitleGroup(ComponentNames.CONFIG)]
        public float valueWhenDivideByZero = 0f;

        [TitleGroup(ComponentNames.CONFIG)]
        public bool enableMin;

        [TitleGroup(ComponentNames.CONFIG)]
        [ShowIf(nameof(enableMin))]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> minConfig = new FixedValueParameterSourceConfig<float>(0f);

        [TitleGroup(ComponentNames.CONFIG)]
        public bool enableMax;

        [TitleGroup(ComponentNames.CONFIG)]
        [ShowIf(nameof(enableMax))]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> maxConfig = new FixedValueParameterSourceConfig<float>(1f);

        protected IParameterSource<float> current;
        protected IParameterSource<float> total;

        protected IParameterSource<float> min;
        protected IParameterSource<float> max;

        protected virtual void Awake()
        {
            current = currentConfig.GetParameterSource();
            total = totalConfig.GetParameterSource();

            if (enableMin)
            {
                min = minConfig.GetParameterSource();
            }

            if (enableMax)
            {
                max = maxConfig.GetParameterSource();
            }
        }

        protected virtual bool TryGetValue(out float value)
        {
            value = 0f;

            if (current.TryGetValue(out var currentValue) == false)
            {
                return false;
            }

            if (total.TryGetValue(out var totalValue) == false)
            {
                return false;
            }

            if (totalValue == 0f)
            {
                value = valueWhenDivideByZero;
                return true;
            }

            value = currentValue / totalValue;

            if (enableMin)
            {
                if (min.TryGetValue(out var minValue) == false)
                {
                    return false;
                }

                value = value.ClampMin(minValue);
            }

            if (enableMax)
            {
                if (max.TryGetValue(out var maxValue) == false)
                {
                    return false;
                }

                value = value.ClampMax(maxValue);
            }

            return true;
        }

        public virtual bool TryGetParameter<TValue>(string key, ref TValue value)
        {
            if (key == RATIO_PARAMETER)
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