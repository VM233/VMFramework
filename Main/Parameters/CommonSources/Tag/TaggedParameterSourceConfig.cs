using System;
using System.Collections.Generic;
using UnityEngine;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.Parameters
{
    [Serializable]
    public class TaggedParameterSourceConfig<TValue> : IParameterSourceConfig<TValue>
        where TValue : IGameTagsOwner
    {
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<TValue> config = new ParameterSourceConfig<TValue>();

        [GameTagID]
        public List<string> validTags = new();

        public IParameterSource<TValue> GetParameterSource()
        {
            return new TaggedParameterSource<TValue>(config.GetParameterSource(), validTags);
        }
    }
}