#if UNITY_EDITOR
using System;

namespace VMFramework.Editor.GameEditor
{
    public struct ContextMenuItemConfig
    {
        public string name;
        public string tooltip;
        public Action onClick;
            
        public ContextMenuItemConfig(string name, Action onClick)
        {
            this.name = name;
            tooltip = name;
            this.onClick = onClick;
        }
            
        public ContextMenuItemConfig(string name, string tooltip, Action onClick)
        {
            this.name = name;
            this.tooltip = tooltip;
            this.onClick = onClick;
        }
    }
}
#endif