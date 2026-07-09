using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.Properties;

namespace VMFramework.Parameters
{
    public class BaseBoostFloatParameterConverter : ParameterProviderBehaviour<BaseBoostFloat>
    {
        public const string FLOAT_VALUE_PARAMETER = "Float Value";

        public const string RANDOM_ROUND_INT_PARAMETER = "Random Rounded Value";

        [TitleGroup(ComponentNames.CONFIG)]
        public bool enableBaseMin;

        [TitleGroup(ComponentNames.CONFIG)]
        [ShowIf(nameof(enableBaseMin))]
        public IParameterSourceConfig<float> baseMinConfig;

        [TitleGroup(ComponentNames.CONFIG)]
        public bool enableBoostMin;

        [TitleGroup(ComponentNames.CONFIG)]
        [ShowIf(nameof(enableBoostMin))]
        public IParameterSourceConfig<float> boostMinConfig;

        [TitleGroup(ComponentNames.CONFIG)]
        public bool enableBaseMax;

        [TitleGroup(ComponentNames.CONFIG)]
        [ShowIf(nameof(enableBaseMax))]
        public IParameterSourceConfig<float> baseMaxConfig;

        [TitleGroup(ComponentNames.CONFIG)]
        public bool enableBoostMax;

        [TitleGroup(ComponentNames.CONFIG)]
        [ShowIf(nameof(enableBoostMax))]
        public IParameterSourceConfig<float> boostMaxConfig;

        protected IParameterSource<float> baseMin;
        protected IParameterSource<float> boostMin;
        protected IParameterSource<float> baseMax;
        protected IParameterSource<float> boostMax;

        protected override void Awake()
        {
            base.Awake();

            if (enableBaseMin)
            {
                baseMin = baseMinConfig.GetParameterSource();
            }

            if (enableBoostMin)
            {
                boostMin = boostMinConfig.GetParameterSource();
            }

            if (enableBaseMax)
            {
                baseMax = baseMaxConfig.GetParameterSource();
            }

            if (enableBoostMax)
            {
                boostMax = boostMaxConfig.GetParameterSource();
            }
        }

        protected override bool TryGetValue(out BaseBoostFloat result)
        {
            if (base.TryGetValue(out result) == false)
            {
                return false;
            }

            if (enableBaseMin && baseMin.TryGetValue(out var baseMinValue))
            {
                result.baseValue = result.baseValue.ClampMin(baseMinValue);
            }

            if (enableBoostMin && boostMin.TryGetValue(out var boostMinValue))
            {
                result.boostValue = result.boostValue.ClampMin(boostMinValue);
            }

            if (enableBaseMax && baseMax.TryGetValue(out var baseMaxValue))
            {
                result.baseValue = result.baseValue.ClampMax(baseMaxValue);
            }

            if (enableBoostMax && boostMax.TryGetValue(out var boostMaxValue))
            {
                result.boostValue = result.boostValue.ClampMax(boostMaxValue);
            }

            return true;
        }

        public override bool TryGetParameter<TValue>(string key, ref TValue value)
        {
            if (key == FLOAT_VALUE_PARAMETER)
            {
                if (TryGetValue(out var targetValue))
                {
                    if (targetValue.Value is TValue validValue)
                    {
                        value = validValue;
                        return true;
                    }
                }
            }
            else if (key == RANDOM_ROUND_INT_PARAMETER)
            {
                if (TryGetValue(out var targetValue))
                {
                    if (targetValue.Value.RandomRound() is TValue validValue)
                    {
                        value = validValue;
                        return true;
                    }
                }
            }

            return base.TryGetParameter(key, ref value);
        }

        public override IEnumerable<(string name, Type type)> GetParametersInfo()
        {
            foreach (var (name, type) in base.GetParametersInfo())
            {
                yield return (name, type);
            }

            yield return (FLOAT_VALUE_PARAMETER, typeof(float));
            yield return (RANDOM_ROUND_INT_PARAMETER, typeof(int));
        }
    }
}