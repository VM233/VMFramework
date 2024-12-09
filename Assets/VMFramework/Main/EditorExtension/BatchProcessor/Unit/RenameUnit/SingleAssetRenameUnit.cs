﻿#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace VMFramework.Editor.BatchProcessor
{
    [UnitSettings(UnitPriority.Super)]
    public sealed class SingleAssetRenameUnit : SingleButtonRenameAssetUnit
    {
        protected override string ProcessButtonName => "Rename";

        public string newName;
        
        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            return selectedObjects.Count == 1;
        }

        protected override string ProcessAssetName(string oldName, Object obj, int index,
            IReadOnlyList<object> selectedObjects)
        {
            return newName;
        }
    }
}
#endif