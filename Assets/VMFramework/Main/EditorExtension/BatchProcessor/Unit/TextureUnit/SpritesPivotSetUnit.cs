#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace VMFramework.Editor.BatchProcessor
{
    public sealed class SpritesPivotSetUnit : SingleButtonSpriteEditorDataUnit
    {
        protected override string ProcessButtonName => "Set Pivot";
        
        private SpriteAlignment alignment = SpriteAlignment.Center;
        
        [MinValue(0), MaxValue(1)]
        [SerializeField]
        [EnableIf(nameof(alignment), SpriteAlignment.Custom)]
        private Vector2 pivot = new(0.5f, 0.5f);

        protected override void OnProcess(Texture2D texture, ISpriteEditorDataProvider provider, int index,
            IReadOnlyList<object> selectedObjects)
        {
            var spriteRects = provider.GetSpriteRects();

            foreach (var spriteRect in spriteRects)
            {
                spriteRect.alignment = alignment;
                spriteRect.pivot = pivot;
            }
            
            provider.SetSpriteRects(spriteRects);
        }
    }
}
#endif