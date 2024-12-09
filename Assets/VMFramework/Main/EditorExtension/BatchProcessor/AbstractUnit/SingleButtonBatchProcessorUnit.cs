#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VMFramework.Editor.BatchProcessor
{
    public abstract class SingleButtonBatchProcessorUnit : BatchProcessorUnit
    {
        protected const string BUTTON_GROUP = "ProcessButton";

        protected abstract string ProcessButtonName { get; }

        protected virtual Color ButtonColor => Color.white;

        [Button("@" + nameof(ProcessButtonName)), HorizontalGroup(BUTTON_GROUP)]
        [GUIColor(nameof(ButtonColor))]
        public void Process()
        {
            var selectedObjects = container.GetSelectedObjects().ToList();

            container.SetSelectedObjects(OnProcess(selectedObjects));
        }

        protected abstract IEnumerable<object> OnProcess(IReadOnlyList<object> selectedObjects);
    }
}
#endif