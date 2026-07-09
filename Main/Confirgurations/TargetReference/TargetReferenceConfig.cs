using System;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    [Serializable]
    public class TargetReferenceConfig<TTarget>
        where TTarget : class
    {
        public TargetReferenceType type = TargetReferenceType.GameObject;

        [ComponentRequired(nameof(targetType))]
        [Required]
        [ShowIf(nameof(type), TargetReferenceType.GameObject)]
        public GameObject gameObject;

        [ComponentRequired(nameof(targetType))]
        [Required]
        [ShowIf(nameof(type), TargetReferenceType.Component)]
        public Component component;

        [SerializeReference]
        [ShowIf(nameof(type), TargetReferenceType.Class)]
        public TTarget classReference;

        private Type targetType = typeof(TTarget);

        public TTarget Target
        {
            get
            {
                if (type == TargetReferenceType.None)
                {
                    return null;
                }

                if (type == TargetReferenceType.GameObject)
                {
                    if (gameObject == null)
                    {
                        return null;
                    }
                    
                    return gameObject.GetComponent<TTarget>();
                }

                if (type == TargetReferenceType.Component)
                {
                    return component as TTarget;
                }

                if (type == TargetReferenceType.Class)
                {
                    return classReference;
                }

                return null;
            }
        }

        public TargetReferenceConfig()
        {

        }

        public TargetReferenceConfig(TargetReferenceType type)
        {
            this.type = type;
        }
    }
}