#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VMFramework.OdinExtensions;
using Object = UnityEngine.Object;

namespace VMFramework.Editor.BatchProcessor
{
    public sealed class RemoveComponentUnit : SingleButtonBatchProcessorUnit
    {
        protected override string ProcessButtonName => "Remove Component";

        [DerivedType(typeof(Component), IncludingSelf = false, IncludingAbstract = false, IncludingGeneric = false,
            IncludingInterfaces = false)]
        [SerializeField]
        private Type componentType;

        [SerializeField]
        private bool removeAll;

        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            return selectedObjects.Any(obj => obj is GameObject);
        }

        protected override IEnumerable<object> OnProcess(IReadOnlyList<object> selectedObjects)
        {
            foreach (var selectedObject in selectedObjects)
            {
                if (selectedObject is GameObject gameObject)
                {
                    if (removeAll)
                    {
                        foreach (var component in gameObject.GetComponents(componentType))
                        {
                            Object.DestroyImmediate(component, true);
                        }
                    }
                    else
                    {
                        if (gameObject.TryGetComponent(componentType, out var component))
                        {
                            Object.DestroyImmediate(component, true);
                        }
                    }
                }

                yield return selectedObject;
            }
        }
    }
}
#endif