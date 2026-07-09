using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VMFramework.Configuration
{
    [Serializable]
    public class TargetsReferenceOrParentConfig<TTarget>
        where TTarget : class
    {
        public bool useParent = true;

        [HideIf(nameof(useParent))]
        public TargetsReferenceConfig<TTarget> targets = new();

        public TTarget[] GetTargets(Component component)
        {
            if (useParent)
            {
                var target = component.GetComponentInParent<TTarget>();
                return new[] { target };
            }

            return targets.GetTargets().ToArray();
        }
    }
}