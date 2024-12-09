#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace VMFramework.Editor.BatchProcessor
{
    public abstract class GamePrefabUnregisterUnit<TAsset> : SingleButtonBatchProcessorUnit
    {
        protected virtual bool IsAsset(object obj, out TAsset asset)
        {
            if (obj is TAsset typedAsset)
            {
                asset = typedAsset;
                return true;
            }
            
            asset = default;
            return false;
        }

        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            return EditorApplication.isPlaying == false && selectedObjects.Any(obj => IsAsset(obj, out _));
        }

        protected sealed override IEnumerable<object> OnProcess(IReadOnlyList<object> selectedObjects)
        {
            foreach (var obj in selectedObjects)
            {
                if (IsAsset(obj, out var asset))
                {
                    OnProcessAsset(asset);
                }
            }
            
            return selectedObjects;
        }

        protected abstract void OnProcessAsset(TAsset asset);
    }
}
#endif