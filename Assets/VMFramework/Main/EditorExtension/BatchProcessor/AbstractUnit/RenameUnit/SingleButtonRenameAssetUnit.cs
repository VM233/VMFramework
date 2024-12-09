#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VMFramework.Core.Editor;

namespace VMFramework.Editor.BatchProcessor
{
    public abstract class SingleButtonRenameAssetUnit : SingleButtonBatchProcessorUnit
    {
        protected override Color ButtonColor => Color.yellow;

        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            return selectedObjects.Any(o => o is Object);
        }

        public sealed override void OnSelectedObjectsChanged(IList<object> selectedObjects)
        {
            base.OnSelectedObjectsChanged(selectedObjects);
            
            OnSelectedObjectsChanged(selectedObjects.Where(o => o is Object).Cast<Object>().ToList());
        }

        protected virtual void OnSelectedObjectsChanged(IList<Object> selectedObjects)
        {
            
        }

        protected sealed override IEnumerable<object> OnProcess(IReadOnlyList<object> selectedObjects)
        {
            int index = 0;
            foreach (var o in selectedObjects)
            {
                if (o is Object unityObject)
                {
                    unityObject.Rename(ProcessAssetName(unityObject.name, unityObject, index, selectedObjects));
                    index++;
                }

                yield return o;
            }
        }

        protected abstract string ProcessAssetName(string oldName, Object obj, int index,
            IReadOnlyList<object> selectedObjects);
    }
}
#endif