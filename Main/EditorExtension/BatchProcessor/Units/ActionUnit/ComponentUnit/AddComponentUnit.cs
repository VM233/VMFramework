#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VMFramework.OdinExtensions;

namespace VMFramework.Editor.BatchProcessor
{
    public sealed class AddComponentUnit : SingleButtonBatchProcessorUnit
    {
        protected override string ProcessButtonName => "Add Component";

        [DerivedType(typeof(Component), IncludingSelf = false, IncludingAbstract = false, IncludingGeneric = false,
            IncludingInterfaces = false)]
        [SerializeField]
        private Type componentType;

        [SerializeField]
        private bool uniqueComponent;

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
                    if (uniqueComponent)
                    {
                        if (gameObject.GetComponent(componentType))
                        {
                            continue;
                        }
                    }

                    gameObject.AddComponent(componentType);
                }

                yield return selectedObject;
            }
        }
    }
}
#endif