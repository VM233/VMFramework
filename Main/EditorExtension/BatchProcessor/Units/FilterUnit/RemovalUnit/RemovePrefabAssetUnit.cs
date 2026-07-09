#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Editor.BatchProcessor
{
    public sealed class RemovePrefabAssetUnit : SingleButtonBatchProcessorUnit
    {
        protected override string ProcessButtonName => "Remove Prefab Asset";

        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            return selectedObjects.Any(o =>
                o is GameObject gameObject && gameObject.IsPrefabAsset());
        }

        protected override IEnumerable<object> OnProcess(IReadOnlyList<object> selectedObjects)
        {
            return selectedObjects.Where(o =>
                o is GameObject gameObject && gameObject.IsPrefabAsset());
        }
    }
}
#endif