using System.Runtime.CompilerServices;
using UnityEngine.UIElements;

namespace VMFramework.UI
{
    public static class PanelModifierUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VisualElement RootVisualElement(this IPanelModifier modifier)
        {
            return ((IUIToolkitPanel)modifier.Panel).RootVisualElement;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UIDocument UIDocument(this IPanelModifier modifier)
        {
            return ((IUIToolkitPanel)modifier.Panel).UIDocument;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IUIToolkitPanel UIToolkitPanel(this IPanelModifier modifier)
        {
            return (IUIToolkitPanel)modifier.Panel;
        }
    }
}