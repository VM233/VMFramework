using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.OdinExtensions;
using VMFramework.Timers;

namespace VMFramework.Parameters
{
    public class FloatParameterConverter : ParameterProviderBehaviour<float>
    {
        public const string INVERTED_VALUE_PARAMETER = "Inverted Value";
        public const string ROUNDED_VALUE_PARAMETER = "Rounded Value";
        public const string ROUNDED_FLOAT_VALUE_PARAMETER = "Rounded Float Value";
        public const string RANDOM_ROUNDED_VALUE_PARAMETER = "Random Rounded Value";
        public const string ROUNDED_UINT_VALUE_PARAMETER = "Rounded UInt Value";
        public const string INVERTED_RANDOM_ROUNDED_VALUE_PARAMETER = "Inverted Random Rounded Value";
        public const string LOGIC_TICK_REAL_TIME_PARAMETER = "Logic Tick Real Time";
        public const string RANDOM_ROUNDED_LOGIC_TICK_PARAMETER = "Random Rounded Logic Tick";

        [TitleGroup(ComponentNames.CONFIG)]
        public bool invert;

        [TitleGroup(ComponentNames.CONFIG)]
        public bool enableMinValue;

        [TitleGroup(ComponentNames.CONFIG)]
        [ShowIf(nameof(enableMinValue))]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> minConfig = new FixedValueParameterSourceConfig<float>(0);

        [TitleGroup(ComponentNames.CONFIG)]
        public bool enableMaxValue;

        [TitleGroup(ComponentNames.CONFIG)]
        [ShowIf(nameof(enableMaxValue))]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<float> maxConfig = new FixedValueParameterSourceConfig<float>(1);

        protected IParameterSource<float> min;
        protected IParameterSource<float> max;

        protected override void Awake()
        {
            base.Awake();

            if (enableMinValue)
            {
                min = minConfig.GetParameterSource();
            }

            if (enableMaxValue)
            {
                max = maxConfig.GetParameterSource();
            }
        }

        protected override bool TryGetValue(out float result)
        {
            if (base.TryGetValue(out result) == false)
            {
                return false;
            }

            if (invert)
            {
                result = -result;
            }

            if (enableMinValue)
            {
                if (min.TryGetValue(out var minValue))
                {
                    result = result.ClampMin(minValue);
                }
            }

            if (enableMaxValue)
            {
                if (max.TryGetValue(out var maxValue))
                {
                    result = result.ClampMax(maxValue);
                }
            }

            return true;
        }

        public override bool TryGetParameter<TValue>(string key, ref TValue value)
        {
            if (key == INVERTED_VALUE_PARAMETER)
            {
                if (TryGetValue(out var floatValue))
                {
                    if (-floatValue is TValue validValue)
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
            else if (key == ROUNDED_FLOAT_VALUE_PARAMETER)
            {
                if (TryGetValue(out var floatValue))
                {
                    if (floatValue.Round().F() is TValue validValue)
                    {
                        value = validValue;
                        return true;
                    }
                }
            }
            else if (key == RANDOM_ROUNDED_VALUE_PARAMETER)
            {
                if (TryGetValue(out var floatValue))
                {
                    if (floatValue.RandomRound() is TValue validValue)
                    {
                        value = validValue;
                        return true;
                    }
                }
            }
            else if (key == ROUNDED_UINT_VALUE_PARAMETER)
            {
                if (TryGetValue(out var floatValue))
                {
                    if ((uint)floatValue.Round() is TValue validValue)
                    {
                        value = validValue;
                        return true;
                    }
                }
            }
            else if (key == INVERTED_RANDOM_ROUNDED_VALUE_PARAMETER)
            {
                if (TryGetValue(out var floatValue))
                {
                    if (-floatValue.RandomRound() is TValue validValue)
                    {
                        value = validValue;
                        return true;
                    }
                }
            }
            else if (key == LOGIC_TICK_REAL_TIME_PARAMETER)
            {
                if (TryGetValue(out var floatValue))
                {
                    if ((float)(floatValue.Round() * LogicTickManager.Instance.TickGap) is TValue validValue)
                    {
                        value = validValue;
                        return true;
                    }
                }
            }
            else if (key == RANDOM_ROUNDED_LOGIC_TICK_PARAMETER)
            {
                if (TryGetValue(out var floatValue))
                {
                    var ticks = (floatValue / LogicTickManager.Instance.TickGap).ClampMin(0);
                    var randomTick = (uint)ticks.RandomRound();

                    if (randomTick is TValue validValue)
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

            yield return (INVERTED_VALUE_PARAMETER, typeof(float));
            yield return (ROUNDED_VALUE_PARAMETER, typeof(int));
            yield return (ROUNDED_FLOAT_VALUE_PARAMETER, typeof(float));
            yield return (RANDOM_ROUNDED_VALUE_PARAMETER, typeof(int));
            yield return (ROUNDED_UINT_VALUE_PARAMETER, typeof(uint));
            yield return (INVERTED_RANDOM_ROUNDED_VALUE_PARAMETER, typeof(int));
            yield return (LOGIC_TICK_REAL_TIME_PARAMETER, typeof(float));
            yield return (RANDOM_ROUNDED_LOGIC_TICK_PARAMETER, typeof(uint));
        }
    }
}