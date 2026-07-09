#if UNITY_EDITOR
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector.Editor;

namespace VMFramework.OdinExtensions
{
    public static class OdinMenuEditorWindowUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SelectValue(this OdinMenuEditorWindow window, object obj, bool additive = false)
        {
            if (window.MenuTree.TryFindMenuItem(obj, out var menuItem) == false)
            {
                return;
            }

            window.MenuTree.Selection.Select(menuItem, additive);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SelectValue<TValue>(this OdinMenuEditorWindow window, bool additive = false)
        {
            if (window.MenuTree.TryFindMenuItem<TValue>(out var menuItem) == false)
            {
                return;
            }
            
            window.MenuTree.Selection.Select(menuItem, additive);
        }
    }
}
#endif