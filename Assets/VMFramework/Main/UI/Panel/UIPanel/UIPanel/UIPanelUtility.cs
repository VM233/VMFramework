using System.Runtime.CompilerServices;

namespace VMFramework.UI
{
    public static class UIPanelUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Open(this IUIPanel panel, IUIPanel source)
        {
            UIPanelManager.TryOpen(panel, source);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryOpen(this IUIPanel panel, IUIPanel source)
        {
            return UIPanelManager.TryOpen(panel, source);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Close(this IUIPanel panel)
        {
            UIPanelManager.TryClose(panel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryClose(this IUIPanel panel)
        {
            return UIPanelManager.TryClose(panel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Toggle(this IUIPanel controller)
        {
            UIPanelManager.Toggle(controller);
        }
    }
}