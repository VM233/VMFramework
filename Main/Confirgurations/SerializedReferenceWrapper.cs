using System;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    [Serializable]
    public class SerializedReferenceWrapper<TValue>
        where TValue : class
    {
        [HideLabel]
        [SerializeReference]
        [IsNotNullOrEmpty]
        public TValue value;
    }
}