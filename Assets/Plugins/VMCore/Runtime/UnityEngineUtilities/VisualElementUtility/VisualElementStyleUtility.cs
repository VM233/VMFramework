using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace VMFramework.Core
{
    public static class VisualElementStyleUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ToggleDisplay(this VisualElement visualElement)
        {
            if (visualElement.style.display.value == DisplayStyle.Flex)
            {
                visualElement.style.display = DisplayStyle.None;
            }
            else
            {
                visualElement.style.display = DisplayStyle.Flex;
            }
        }
        
        #region Set Position

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPosition(this VisualElement visualElement, Vector2 position,
            bool setRightTopAuto = true)
        {
            visualElement.style.left = position.x;
            visualElement.style.bottom = position.y;

            if (setRightTopAuto)
            {
                visualElement.style.right = StyleKeyword.Auto;
                visualElement.style.top = StyleKeyword.Auto;
                visualElement.style.width = visualElement.resolvedStyle.width;
                visualElement.style.height = visualElement.resolvedStyle.height;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPosition(this VisualElement visualElement, Vector2 position, bool useRight,
            bool useTop)
        {
            if (useRight)
            {
                visualElement.style.right = position.x;
            }
            else
            {
                visualElement.style.left = position.x;
            }

            if (useTop)
            {
                visualElement.style.top = position.y;
            }
            else
            {
                visualElement.style.bottom = position.y;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPosition(this VisualElement visualElement, Vector2 position, bool useRight,
            bool useTop, Vector2 bounds)
        {
            if (useRight)
            {
                visualElement.style.right = bounds.x - position.x - visualElement.resolvedStyle.width;
            }
            else
            {
                visualElement.style.left = position.x;
            }

            if (useTop)
            {
                visualElement.style.top = bounds.y - position.y - visualElement.resolvedStyle.height;
            }
            else
            {
                visualElement.style.bottom = position.y;
            }
        }

        #endregion
    }
}