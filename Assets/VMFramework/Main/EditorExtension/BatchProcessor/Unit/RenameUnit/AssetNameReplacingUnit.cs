#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VMFramework.Editor.BatchProcessor
{
    public sealed class AssetNameReplacingUnit : SingleButtonRenameAssetUnit
    {
        protected override string ProcessButtonName => "Replace String";

        [HorizontalGroup]
        public string oldString;

        [HorizontalGroup]
        public string newString;

        protected override void OnSelectedObjectsChanged(IList<Object> selectedObjects)
        {
            base.OnSelectedObjectsChanged(selectedObjects);

            var distinctNames = selectedObjects.Select(x => x.name).Distinct().ToList();

            if (distinctNames.Count > 1)
            {
                return;
            }

            var name = distinctNames[0];
            
            oldString = name;
        }

        protected override string ProcessAssetName(string oldName, Object obj, int index,
            IReadOnlyList<object> selectedObjects)
        {
            return oldName.Replace(oldString, newString);
        }
    }
}
#endif