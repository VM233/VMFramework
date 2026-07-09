#if UNITY_EDITOR
using System.Runtime.CompilerServices;

namespace VMFramework.Editor.GameEditor
{
    public static class MenuItemConfigUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ContextMenuItemConfig ToMenuItemConfig(this ToolbarButtonConfig buttonConfig)
        {
            return new(buttonConfig.path, buttonConfig.tooltip, buttonConfig.onClick);
        }
    }
}
#endif