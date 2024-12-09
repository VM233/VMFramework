#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace VMFramework.Editor.BatchProcessor
{
    public sealed class RenameAssetsToSequentialUnit : SingleButtonRenameAssetUnit
    {
        protected override string ProcessButtonName => "Rename To Sequential";

        public string newName;
        
        protected override string ProcessAssetName(string oldName, Object obj, int index,
            IReadOnlyList<object> selectedObjects)
        {
            return newName + index;
        }
    }
}
#endif