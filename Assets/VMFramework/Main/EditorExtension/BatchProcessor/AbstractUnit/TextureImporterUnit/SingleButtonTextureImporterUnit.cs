#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using VMFramework.Core.Editor;

namespace VMFramework.Editor.BatchProcessor
{
    public abstract class SingleButtonTextureImporterUnit : SingleButtonBatchProcessorUnit
    {
        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            return selectedObjects.All(obj => obj is not Sprite) && selectedObjects.Any(obj => obj is Texture2D);
        }

        protected sealed override IEnumerable<object> OnProcess(IReadOnlyList<object> selectedObjects)
        {
            int index = 0;
            foreach (var selectedObject in selectedObjects)
            {
                if (selectedObject is not Texture2D texture)
                {
                    continue;
                }

                if (texture.TryGetImporter(out var importer, out var path) == false)
                {
                    continue;
                }

                OnProcess(texture, importer, index, selectedObjects);

                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

                index++;
            }

            return selectedObjects;
        }

        protected abstract void OnProcess(Texture2D texture, TextureImporter importer, int index,
            IReadOnlyList<object> selectedObjects);
    }
}
#endif