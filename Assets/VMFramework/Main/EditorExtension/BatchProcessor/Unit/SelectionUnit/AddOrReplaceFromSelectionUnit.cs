#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace VMFramework.Editor.BatchProcessor
{
    [UnitSettings(UnitPriority.High)]
    public sealed class AddOrReplaceFromSelectionUnit : BatchProcessorUnit
    {
        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            return Selection.objects.Length != 0;
        }

        [ResponsiveButtonGroup]
        private void AddFromSelection()
        {
            container.AddSelectedObjects(Selection.objects);
        }

        [ResponsiveButtonGroup]
        private void ReplaceFromSelection()
        {
            container.SetSelectedObjects(Selection.objects);
        }

        [ResponsiveButtonGroup]
        private void AddFromSelectionWithoutDuplicate()
        {
            var selectedObjects = Selection.objects;
            var newSelectedObjects = new List<Object>();

            foreach (var selectedObject in selectedObjects)
            {
                if (container.ContainsSelectedObject(selectedObject) == false)
                {
                    newSelectedObjects.Add(selectedObject);
                }
            }

            container.AddSelectedObjects(newSelectedObjects);
        }

        [ResponsiveButtonGroup]
        private void ReplaceFromSelectionWithoutDuplicate()
        {
            container.SetSelectedObjects(Selection.objects.Distinct());
        }
    }
}

#endif