using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VMFramework.Configuration
{
    [Serializable]
    public class TargetReferenceOrParentConfig<TTarget>
        where TTarget : class
    {
        public bool useParent = true;

        [HideIf(nameof(useParent))]
        public TargetReferenceConfig<TTarget> target;

        public TTarget GetTarget(Component component)
        {
            if (useParent)
            {
                var target = component.GetComponentInParent<TTarget>();
                return target;
            }

            return target.Target;
        }
    }
}