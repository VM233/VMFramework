#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VMFramework.Editor.BatchProcessor
{
    public sealed class QueryUnityObjectsByNameUnit : SingleButtonBatchProcessorUnit
    {
        protected override string ProcessButtonName => "Query By Name";

        [SerializeField]
        private string queryContent;

        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            return selectedObjects.Any(obj => obj is Object);
        }

        protected override IEnumerable<object> OnProcess(IReadOnlyList<object> selectedObjects)
        {
            return selectedObjects.Where(o =>
                o is Object obj && obj.name.Contains(queryContent));
        }
    }
}
#endif