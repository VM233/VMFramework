#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;

namespace VMFramework.Editor.BatchProcessor
{
    public sealed class RemoveNullUnit : SingleButtonBatchProcessorUnit
    {
        protected override string ProcessButtonName => "Remove Null";

        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            return selectedObjects.Any(o => o == null);
        }

        protected override IEnumerable<object> OnProcess(IReadOnlyList<object> selectedObjects)
        {
            return selectedObjects.Where(o => o != null);
        }
    }
}
#endif