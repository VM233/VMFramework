using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    [Serializable]
    public class TargetsReferenceConfig<TTarget>
        where TTarget : class
    {
        public TargetReferenceType type = TargetReferenceType.GameObject;

        [ComponentRequired(nameof(targetType))]
        [Required]
        [IsNotNullOrEmpty]
        [ShowIf(nameof(type), TargetReferenceType.GameObject)]
        public List<GameObject> gameObjects = new();

        [ComponentRequired(nameof(targetType))]
        [Required]
        [IsNotNullOrEmpty]
        [ShowIf(nameof(type), TargetReferenceType.Component)]
        public List<Component> components = new();

        private Type targetType = typeof(TTarget);

        public TargetsReferenceConfig()
        {

        }

        public TargetsReferenceConfig(TargetReferenceType type)
        {
            this.type = type;
        }

        public IEnumerable<TTarget> GetTargets()
        {
            if (type is TargetReferenceType.None)
            {
                yield break;
            }

            if (type is TargetReferenceType.GameObject)
            {
                foreach (var go in gameObjects)
                {
                    foreach (var component in go.GetComponents<TTarget>())
                    {
                        yield return component;
                    }
                }

                yield break;
            }

            if (type is TargetReferenceType.Component)
            {
                foreach (var component in components)
                {
                    if (component is TTarget target)
                    {
                        yield return target;
                    }
                }

                yield break;
            }
        }
    }
}