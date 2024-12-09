#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using VMFramework.Core.Editor;

namespace VMFramework.Editor.BatchProcessor
{
    public abstract class SingleButtonSpriteEditorDataUnit : SingleButtonBatchProcessorUnit
    {
        private SpriteDataProviderFactories factory;
        
        protected override void OnInit()
        {
            base.OnInit();

            factory = new();
            factory.Init();
        }

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

                if (texture.TryGetAssetPath(out string path) == false)
                {
                    continue;
                }

                var provider = factory.GetSpriteEditorDataProviderFromObject(texture);
                provider.InitSpriteEditorDataProvider();
                
                OnProcess(texture, provider, index, selectedObjects);
                
                provider.Apply();
                
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                
                index++;
            }
            
            return selectedObjects;
        }

        protected abstract void OnProcess(Texture2D texture, ISpriteEditorDataProvider provider, int index,
            IReadOnlyList<object> selectedObjects);
    }
}
#endif