#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using VMFramework.Core.Editor;

namespace VMFramework.Editor.BatchProcessor
{
    public sealed class OpenScriptOfTypeUnit : SingleButtonBatchProcessorUnit
    {
        protected override string ProcessButtonName => "Open Script";

        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            return selectedObjects.Any(obj => obj is Type);
        }

        protected override IEnumerable<object> OnProcess(IReadOnlyList<object> selectedObjects)
        {
            foreach (var obj in selectedObjects)
            {
                if (obj is Type type)
                {
                    type.OpenScriptOfType();
                }
            }
            
            return selectedObjects;
        }
    }
}
#endif