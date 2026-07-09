using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.OdinExtensions;
using VMFramework.Timers;

namespace VMFramework.Parameters
{
    public class IntParameterConverter : ParameterProviderBehaviour<int>
    {
        public const string FLOAT_VALUE_PARAMETER = "Float Value";
     
        public const string UINT_VALUE_PARAMETER = "UInt Value";
        
        public const string LOGIC_TICK_REAL_TIME_PARAMETER = "Logic Tick Real Time";

        [TitleGroup(ComponentNames.CONFIG)]
        public bool enableMinValue;

        [TitleGroup(ComponentNames.CONFIG)]
        [ShowIf(nameof(enableMinValue))]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<int> minConfig = new FixedValueParameterSourceConfig<int>(0);

        [TitleGroup(ComponentNames.CONFIG)]
        public bool enableMaxValue;

        [TitleGroup(ComponentNames.CONFIG)]
        [ShowIf(nameof(enableMaxValue))]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<int> maxConfig = new FixedValueParameterSourceConfig<int>(1);

        protected IParameterSource<int> min;
        protected IParameterSource<int> max;
        
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

        protected override bool TryGetValue(out int result)
        {
            if (base.TryGetValue(out result) == false)
            {
                return false;
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
            if (key == FLOAT_VALUE_PARAMETER)
            {
                if (TryGetValue(out int target))
                {
                    var floatValue = (float)target;

                    if (floatValue is TValue validValue)
                    {
                        value = validValue;
                        return true;
                    }
                }
            }
            else if (key == UINT_VALUE_PARAMETER)
            {
                if (TryGetValue(out int target))
                {
                    var uintValue = (uint)(target.ClampMin(0));
                    
                    if (uintValue is TValue validValue)
                    {
                        value = validValue;
                        return true;
                    }
                }
            }
            else if (key == LOGIC_TICK_REAL_TIME_PARAMETER)
            {
                if (TryGetValue(out int target))
                {
                    var floatValue = (float)(target * LogicTickManager.Instance.TickGap);
                    
                    if (floatValue is TValue validValue)
                    {
                        value = validValue;
                        return true;
                    }
                }
            }
            else
            {
                return base.TryGetParameter(key, ref value);
            }

            return false;
        }

        public override IEnumerable<(string name, Type type)> GetParametersInfo()
        {
            foreach (var (name, type) in base.GetParametersInfo())
            {
                yield return (name, type);
            }

            yield return (FLOAT_VALUE_PARAMETER, typeof(float));
            yield return (UINT_VALUE_PARAMETER, typeof(uint));
            yield return (LOGIC_TICK_REAL_TIME_PARAMETER, typeof(float));
        }
    }
}