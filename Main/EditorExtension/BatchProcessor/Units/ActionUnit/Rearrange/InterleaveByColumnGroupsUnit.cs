#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Core;

namespace VMFramework.Editor.BatchProcessor
{
    public class InterleaveByColumnGroupsUnit : SingleButtonBatchProcessorUnit
    {
        [MinValue(2)]
        public int columnCount = 2;

        protected override string ProcessButtonName => "Interleave By Column Groups";

        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            return selectedObjects.Count >= 4 && selectedObjects.Count % columnCount == 0;
        }

        protected override IEnumerable<object> OnProcess(IReadOnlyList<object> selectedObjects)
        {
            var result = new List<object>();
            selectedObjects.InterleaveByColumnGroups(columnCount, result);
            return result;
        }
    }
}
#endif