using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.Parameters
{
    [InfoBox("Y = Mul * Log_{LogBase}(1 + ExpBase ^ X)")]
    [ParameterDefine(VALUE_PARAMETER, typeof(float))]
    [ParameterDefine(ROUNDED_VALUE_PARAMETER, typeof(int))]
    [ParameterDefine(UINT_VALUE_PARAMETER, typeof(uint))]
    public class SoftPlusParameterProvider : MonoBehaviour, IParameterProvider
    {
        public const string VALUE_PARAMETER = "Value";
        public const string ROUNDED_VALUE_PARAMETER = "Rounded Value";
        public const string UINT_VALUE_PARAMETER = "UInt Value";

        [TitleGroup(ComponentNames.CONFIG), LabelText("X")]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> xConfig = new ParameterSourceConfig<float>();

        [TitleGroup(ComponentNames.CONFIG)]
        public bool invertX;

        [TitleGroup(ComponentNames.CONFIG), LabelText("Log Base")]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> logBaseConfig = new FixedValueParameterSourceConfig<float>(2.0f);

        [TitleGroup(ComponentNames.CONFIG), LabelText("Exp Base")]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> expBaseConfig = new FixedValueParameterSourceConfig<float>(1.5f);

        [TitleGroup(ComponentNames.CONFIG), LabelText("Mul")]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> mulConfig = new FixedValueParameterSourceConfig<float>(1.0f);

        protected IParameterSource<float> x;
        protected IParameterSource<float> logBase;
        protected IParameterSource<float> expBase;
        protected IParameterSource<float> mul;

        protected virtual void Awake()
        {
            x = xConfig.GetParameterSource();
            logBase = logBaseConfig.GetParameterSource();
            expBase = expBaseConfig.GetParameterSource();
            mul = mulConfig.GetParameterSource();
        }

        public virtual bool TryGetValue(out float value)
        {
            value = 0;

            if (x.TryGetValue(out var xValue) == false)
            {
                return false;
            }

            if (logBase.TryGetValue(out var logBaseValue) == false)
            {
                return false;
            }

            if (expBase.TryGetValue(out var expBaseValue) == false)
            {
                return false;
            }

            if (mul.TryGetValue(out var mulValue) == false)
            {
                return false;
            }

            if (invertX)
            {
                xValue = -xValue;
            }

            value = mulValue * Mathf.Log(1 + Mathf.Pow(expBaseValue, xValue), logBaseValue);
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
            else if (key == UINT_VALUE_PARAMETER)
            {
                if (TryGetValue(out var floatValue))
                {
                    if ((uint)floatValue.Round().ClampMin(0) is TValue validValue)
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