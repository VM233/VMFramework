using System;
using System.Collections.Generic;
using UnityEngine;
using VMFramework.OdinExtensions;

namespace VMFramework.Parameters
{
    [Serializable]
    public class PriorityCompositeParameterSourceConfig<TValue> : IParameterSourceConfig<TValue>
    {
        [IsNotNullOrEmpty]
        [SerializeReference]
        public List<IParameterSourceConfig<TValue>> configs = new();

        public IParameterSource<TValue> GetParameterSource()
        {
            var sources = new List<IParameterSource<TValue>>();
            foreach (var config in configs)
            {
                sources.Add(config.GetParameterSource());
            }

            return new PriorityCompositeParameterSource<TValue>(sources);
        }
    }
}