#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using VMFramework.Core;
using VMFramework.Core.Editor;
using VMFramework.Core.Linq;

namespace VMFramework.Editor.BatchProcessor
{
    internal sealed class CreatePrefabUnit : SingleButtonBatchProcessorUnit
    {
        protected override string ProcessButtonName => "Create Prefab";

        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            return selectedObjects.AnyIs<Type>();
        }

        protected override IEnumerable<object> OnProcess(IReadOnlyList<object> selectedObjects)
        {
            if (Selection.activeObject.IsFolder(out var path) == false)
            {
                UnityEngine.Debug.LogWarning($"Please select a folder to create the prefab in.");
                goto RETURN;
            }

            foreach (var obj in selectedObjects)
            {
                if (obj is Type type)
                {
                    var prefabName = type.Name.ToPascalCase(" ");
                    var prefabPath = path.PathCombine(prefabName + ".prefab");
                    prefabPath = AssetDatabase.GenerateUniqueAssetPath(prefabPath);
                    type.SaveComponentAsPrefab(prefabPath);
                    continue;
                }
            }
            
            RETURN:
            return selectedObjects;
        }
    }
}
#endif