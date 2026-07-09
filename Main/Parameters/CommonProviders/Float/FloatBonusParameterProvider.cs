using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.Parameters
{
    [InfoBox("Result = Source * (1 + Bonus)")]
    [ParameterDefine(VALUE_PARAMETER, typeof(float))]
    public class FloatBonusParameterProvider : MonoBehaviour, IParameterProvider
    {
        public const string VALUE_PARAMETER = "Value";

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> sourceConfig = new ParameterSourceConfig<float>();

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> bonusConfig = new ParameterSourceConfig<float>();

        protected IParameterSource<float> source;
        protected IParameterSource<float> bonus;

        protected virtual void Awake()
        {
            source = sourceConfig.GetParameterSource();
            bonus = bonusConfig.GetParameterSource();
        }

        public virtual bool TryGetValue(out float result)
        {
            result = 0;
            if (source.TryGetValue(out var sourceValue) == false)
            {
                return false;
            }

            if (bonus.TryGetValue(out var bonusValue) == false)
            {
                return false;
            }

            result = sourceValue * (1 + bonusValue);
            return true;
        }

        public virtual bool TryGetParameter<TValue>(string key, ref TValue value)
        {
            if (key == VALUE_PARAMETER)
            {
                if (TryGetValue(out var result))
                {
                    if (result is TValue validValue)
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