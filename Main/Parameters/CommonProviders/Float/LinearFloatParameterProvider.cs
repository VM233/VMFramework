using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.OdinExtensions;
using VMFramework.Properties;

namespace VMFramework.Parameters
{
    [InfoBox("Y = k * (X + offset) + b")]
    [ParameterDefine(VALUE_PARAMETER, typeof(float))]
    [ParameterDefine(ROUNDED_VALUE_PARAMETER, typeof(int))]
    [ParameterDefine(BASE_BOOST_VALUE_PARAMETER, typeof(BaseBoostFloat))]
    public class LinearFloatParameterProvider : MonoBehaviour, IParameterProvider
    {
        public const string VALUE_PARAMETER = "Value";
        public const string ROUNDED_VALUE_PARAMETER = "Rounded Value";
        public const string BASE_BOOST_VALUE_PARAMETER = "Base Boost Value";

        [TitleGroup(ComponentNames.CONFIG)]
        [LabelText("X")]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> xConfig = new ParameterSourceConfig<float>();

        [TitleGroup(ComponentNames.CONFIG)]
        [LabelText("k")]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> kConfig = new FixedValueParameterSourceConfig<float>(1);

        [TitleGroup(ComponentNames.CONFIG)]
        [LabelText("offset")]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> offsetConfig = new FixedValueParameterSourceConfig<float>(0);

        [TitleGroup(ComponentNames.CONFIG)]
        [LabelText("b")]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> bConfig = new FixedValueParameterSourceConfig<float>(0);

        protected IParameterSource<float> x;
        protected IParameterSource<float> k;
        protected IParameterSource<float> offset;
        protected IParameterSource<float> b;

        protected virtual void Awake()
        {
            x = xConfig.GetParameterSource();
            k = kConfig.GetParameterSource();
            offset = offsetConfig.GetParameterSource();
            b = bConfig.GetParameterSource();
        }

        public virtual bool TryGetValue(out float value)
        {
            value = 0;

            if (x.TryGetValue(out var xValue) == false)
            {
                return false;
            }

            if (k.TryGetValue(out var kValue) == false)
            {
                return false;
            }

            if (offset.TryGetValue(out var offsetValue) == false)
            {
                return false;
            }

            if (b.TryGetValue(out var bValue) == false)
            {
                return false;
            }

            value = kValue * (xValue + offsetValue) + bValue;
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
            else if (key == ROUNDED_VALUE_PARAMETER)
            {
                if (TryGetValue(out var floatValue))
                {
                    if (floatValue.Round() is TValue validValue)
                    {
                        value = validValue;
                        return true;
                    }
                }
            }
            else if (key == BASE_BOOST_VALUE_PARAMETER)
            {
                if (TryGetValue(out var floatValue))
                {
                    if (new BaseBoostFloat(floatValue) is TValue validValue)
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