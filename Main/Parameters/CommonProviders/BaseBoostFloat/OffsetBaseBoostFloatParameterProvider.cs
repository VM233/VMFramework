using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.OdinExtensions;
using VMFramework.Properties;

namespace VMFramework.Parameters
{
    [InfoBox("Y = X + offset")]
    [ParameterDefine(VALUE_PARAMETER, typeof(BaseBoostFloat))]
    [ParameterDefine(FLOAT_VALUE_PARAMETER, typeof(float))]
    [ParameterDefine(INVERTED_FLOAT_VALUE_PARAMETER, typeof(float))]
    public class OffsetBaseBoostFloatParameterProvider : MonoBehaviour, IParameterProvider
    {
        public const string VALUE_PARAMETER = "Value";
        public const string FLOAT_VALUE_PARAMETER = "Float Value";
        public const string INVERTED_FLOAT_VALUE_PARAMETER = "Inverted Float Value";

        [TitleGroup(ComponentNames.CONFIG)]
        public bool invert = false;
        
        [TitleGroup(ComponentNames.CONFIG)]
        public bool invertOffset = false;

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<BaseBoostFloat> xConfig = new ParameterSourceConfig<BaseBoostFloat>();

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<BaseBoostFloat> offsetConfig =
            new FixedValueParameterSourceConfig<BaseBoostFloat>();

        protected IParameterSource<BaseBoostFloat> x;
        protected IParameterSource<BaseBoostFloat> offset;

        protected virtual void Awake()
        {
            x = xConfig.GetParameterSource();
            offset = offsetConfig.GetParameterSource();
        }

        public virtual bool TryGetValue(out BaseBoostFloat value)
        {
            value = default;

            if (x.TryGetValue(out var xValue) == false)
            {
                return false;
            }

            if (offset.TryGetValue(out var offsetValue) == false)
            {
                return false;
            }

            if (invertOffset)
            {
                value = xValue - offsetValue;
            }
            else
            {
                value = xValue + offsetValue;
            }
            
            if (invert)
            {
                value = -value;
            }

            return true;
        }

        public virtual bool TryGetParameter<TValue>(string key, ref TValue value)
        {
            if (key == VALUE_PARAMETER)
            {
                if (TryGetValue(out var resultValue))
                {
                    if (resultValue is TValue validValue)
                    {
                        value = validValue;
                        return true;
                    }
                }
            }
            else if (key == FLOAT_VALUE_PARAMETER)
            {
                if (TryGetValue(out var resultValue))
                {
                    if (resultValue.Value is TValue validValue)
                    {
                        value = validValue;
                        return true;
                    }
                }
            }
            else if (key == INVERTED_FLOAT_VALUE_PARAMETER)
            {
                if (TryGetValue(out var resultValue))
                {
                    var invertedValue = -resultValue.Value;
                    if (invertedValue is TValue validValue)
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