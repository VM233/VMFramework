#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;

namespace VMFramework.Editor.BatchProcessor
{
    public sealed class RemoveDuplicateUnit : SingleButtonBatchProcessorUnit
    {
        protected override string ProcessButtonName => "Remove Duplicate";

        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            return selectedObjects.Distinct().Count() != selectedObjects.Count;
        }

        protected override IEnumerable<object> OnProcess(IReadOnlyList<object> selectedObjects)
        {
            return selectedObjects.Distinct();
        }
    }
}
#endif