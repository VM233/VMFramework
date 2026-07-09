#if UNITY_EDITOR
using System.Runtime.CompilerServices;
using UnityEditor;

namespace VMFramework.Core.Editor
{
    public static class SpriteRectUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpriteRect Copy(this SpriteRect spriteRect)
        {
            var copy = new SpriteRect
            {
                alignment = spriteRect.alignment,
                pivot = spriteRect.pivot,
                rect = spriteRect.rect,
                border = spriteRect.border,
                customData = spriteRect.customData
            };
            return copy;
        }
    }
}
#endif