#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VMFramework.Editor.BatchProcessor
{
    public abstract class DoubleButtonBatchProcessorUnit : BatchProcessorUnit
    {
        protected virtual string ProcessButtonOneName => "处理1";

        protected virtual string ProcessButtonTwoName => "处理2";

        protected virtual Color ButtonColor => Color.white;

        [Button("@" + nameof(ProcessButtonOneName)), ResponsiveButtonGroup]
        [GUIColor(nameof(ButtonColor))]
        public void ProcessOne()
        {
            var selectedObjects = container.GetSelectedObjects().ToList();

            container.SetSelectedObjects(
                OnProcessOne(selectedObjects));
        }

        [Button("@" + nameof(ProcessButtonTwoName)), ResponsiveButtonGroup]
        [GUIColor(nameof(ButtonColor))]
        public void ProcessTwo()
        {
            var selectedObjects = container.GetSelectedObjects().ToList();

            container.SetSelectedObjects(
                OnProcessTwo(selectedObjects));
        }

        protected abstract IEnumerable<object> OnProcessOne(
            IEnumerable<object> selectedObjects);

        protected abstract IEnumerable<object> OnProcessTwo(
            IEnumerable<object> selectedObjects);
    }
}
#endif