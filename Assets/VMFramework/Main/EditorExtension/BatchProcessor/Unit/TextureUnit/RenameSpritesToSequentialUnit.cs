#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace VMFramework.Editor.BatchProcessor
{
    public sealed class RenameSpritesToSequentialUnit : SingleButtonSpriteUnit
    {
        protected override string ProcessButtonName => "Rename Sprites To Sequential";

        [SerializeField]
        private string newName;
        
        protected override void OnProcess(Sprite sprite, SpriteRect spriteRect, int processingIndex,
            ISpriteEditorDataProvider provider, IReadOnlyList<object> selectedObjects)
        {
            spriteRect.name = $"{newName}_{processingIndex}";
        }
    }
}
#endif