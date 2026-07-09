using System;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.Parameters
{
    [Serializable]
    public class RandomFromChooserSourceConfig<TValue>
        : IParameterSourceConfig<TValue>, ICollectionParametersSourceConfig<TValue>
    {
        [HideLabel]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IChooser<TValue> chooser;

        public IParameterSource<TValue> GetParameterSource()
        {
            return new RandomFromChooserSource<TValue>(chooser.GenerateNewChooser());
        }

        public ICollectionParametersSource<TValue> GetCollectionParameterSource()
        {
            return new RandomFromChooserSource<TValue>(chooser.GenerateNewChooser());
        }
    }
}