using System.Runtime.CompilerServices;

namespace VMFramework.UI
{
    public static class UIPanelUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Open(this IUIPanel panel, IUIPanel source)
        {
            UIPanelManager.Instance.TryOpen(panel, source);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryOpen(this IUIPanel panel, IUIPanel source)
        {
            return UIPanelManager.Instance.TryOpen(panel, source);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Close(this IUIPanel panel)
        {
            UIPanelManager.Instance.TryClose(panel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryClose(this IUIPanel panel)
        {
            return UIPanelManager.Instance.TryClose(panel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Toggle(this IUIPanel controller)
        {
            UIPanelManager.Instance.Toggle(controller);
        }
    }
}