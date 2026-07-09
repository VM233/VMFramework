using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.Parameters
{
    [InfoBox("Y = X + offset")]
    [ParameterDefine(VALUE_PARAMETER, typeof(float))]
    [ParameterDefine(INVERTED_VALUE_PARAMETER, typeof(float))]
    public class OffsetFloatParameterProvider : MonoBehaviour, IParameterProvider
    {
        public const string VALUE_PARAMETER = "Value";
        public const string INVERTED_VALUE_PARAMETER = "Inverted Value";

        [TitleGroup(ComponentNames.CONFIG)]
        public bool invert = false;

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> xConfig = new ParameterSourceConfig<float>();

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> offsetConfig = new FixedValueParameterSourceConfig<float>(0);

        protected IParameterSource<float> x;
        protected IParameterSource<float> offset;

        protected virtual void Awake()
        {
            x = xConfig.GetParameterSource();
            offset = offsetConfig.GetParameterSource();
        }

        public virtual bool TryGetValue(out float value)
        {
            value = 0;

            if (x.TryGetValue(out var xValue) == false)
            {
                return false;
            }

            if (offset.TryGetValue(out var offsetValue) == false)
            {
                return false;
            }

            value = xValue + offsetValue;
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
            else if (key == INVERTED_VALUE_PARAMETER)
            {
                if (TryGetValue(out var resultValue))
                {
                    var invertedValue = -resultValue;
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