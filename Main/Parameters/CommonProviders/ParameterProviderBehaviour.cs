using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.Parameters
{
    public class ParameterProviderBehaviour<TTarget> : MonoBehaviour, IParameterProvider, IParametersInfoProvider
    {
        public const string PARAMETER_NAME = "Value";

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<TTarget> sourceConfig = new ParameterSourceConfig<TTarget>();

        protected IParameterSource<TTarget> source;

        protected virtual void Awake()
        {
            source = sourceConfig.GetParameterSource();
        }

        public virtual bool TryGetParameter<TValue>(string key, ref TValue value)
        {
            if (key == PARAMETER_NAME)
            {
                if (TryGetValue(out var targetValue))
                {
                    if (targetValue is TValue validValue)
                    {
                        value = validValue;
                        return true;
                    }
                }
            }

            return false;
        }

        protected virtual bool TryGetValue(out TTarget result)
        {
            return source.TryGetValue(out result);
        }

        public virtual IEnumerable<(string name, Type type)> GetParametersInfo()
        {
            yield return (PARAMETER_NAME, typeof(TTarget));
        }
    }
}