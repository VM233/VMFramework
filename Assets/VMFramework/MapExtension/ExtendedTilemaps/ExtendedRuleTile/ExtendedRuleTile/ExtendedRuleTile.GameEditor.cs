#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Core.Linq;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;

namespace VMFramework.Maps
{
    public partial class ExtendedRuleTile : IGameEditorMenuTreeNode
    {
        Icon IGameEditorMenuTreeNode.Icon
        {
            get
            {
                if (TryGetIconFromSpriteConfig(defaultSpriteConfig, out var defaultIcon))
                {
                    return defaultIcon;
                }
                
                if (ruleSet.IsNullOrEmpty())
                {
                    return Icon.None;
                }
                    
                var lastRule = ruleSet[^1];

                if (TryGetIconFromSpriteConfig(lastRule.spriteConfig, out var lastIcon))
                {
                    return lastIcon;
                }
                
                return Icon.None;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryGetIconFromSpriteConfig(SpriteConfig spriteConfig, out Icon icon)
        {
            if (spriteConfig == null)
            {
                icon = Icon.None;
                return false;
            }

            var sprite = spriteConfig.sprite?.GetAvailableValues().FirstOrDefault()?.Sprite;

            icon = sprite;
            return sprite != null;
        }
    }
}
#endif