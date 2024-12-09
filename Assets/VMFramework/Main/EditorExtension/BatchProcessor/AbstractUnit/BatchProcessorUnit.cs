#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace VMFramework.Editor.BatchProcessor
{
    [HideDuplicateReferenceBox]
    [HideReferenceObjectPicker]
    public abstract class BatchProcessorUnit
    {
        protected BatchProcessorContainer container { get; private set; }

        public void Init(BatchProcessorContainer container)
        {
            this.container = container;
            
            OnInit();
        }

        protected virtual void OnInit()
        {
            
        }

        public abstract bool IsValid(IReadOnlyList<object> selectedObjects);

        public virtual void OnSelectedObjectsChanged(IList<object> selectedObjects)
        {
            
        }
    }
}

#endif