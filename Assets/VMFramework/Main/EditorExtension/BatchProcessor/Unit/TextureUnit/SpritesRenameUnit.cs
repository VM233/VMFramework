#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace VMFramework.Editor.BatchProcessor
{
    public sealed class SpritesRenameUnit : SingleButtonSpriteEditorDataUnit
    {
        protected override string ProcessButtonName => "Rename Sprites";
        
        [SerializeField]
        private string newName;

        protected override void OnProcess(Texture2D texture, ISpriteEditorDataProvider provider, int index,
            IReadOnlyList<object> selectedObjects)
        {
            var spriteRects = provider.GetSpriteRects();

            for (int i = 0; i < spriteRects.Length; i++)
            {
                var spriteRect = spriteRects[i];

                spriteRect.name = $"{newName}_{i}";
            }
            
            provider.SetSpriteRects(spriteRects);
        }
    }
}
#endif