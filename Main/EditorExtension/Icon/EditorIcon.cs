#if UNITY_EDITOR
using System;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VMFramework.Editor
{
    public readonly struct EditorIcon
    {
        public static readonly EditorIcon None = new(SdfIconType.None);
        
        public readonly EditorIconType iconType;
        
        public readonly SdfIconType sdfIconType;

        public readonly Sprite spriteIcon;
        
        public readonly Texture textureIcon;

        public EditorIcon(SdfIconType sdfIconType)
        {
            iconType = EditorIconType.SdfIcon;
            this.sdfIconType = sdfIconType;
            spriteIcon = null;
            textureIcon = null;
        }

        public EditorIcon(Sprite spriteIcon)
        {
            iconType = EditorIconType.Sprite;
            sdfIconType = SdfIconType.None;
            this.spriteIcon = spriteIcon;
            textureIcon = null;
        }

        public EditorIcon(Texture textureIcon)
        {
            iconType = EditorIconType.Texture;
            sdfIconType = SdfIconType.None;
            spriteIcon = null;
            this.textureIcon = textureIcon;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsNone()
        {
            return iconType switch
            {
                EditorIconType.Sprite => spriteIcon == null,
                EditorIconType.SdfIcon => sdfIconType == SdfIconType.None,
                EditorIconType.Texture => textureIcon == null,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public static implicit operator EditorIcon(SdfIconType sdfIconType) => new(sdfIconType);
        
        public static implicit operator EditorIcon(Sprite spriteIcon) => new(spriteIcon);
        
        public static implicit operator EditorIcon(Texture textureIcon) => new(textureIcon);
    }
}
#endif