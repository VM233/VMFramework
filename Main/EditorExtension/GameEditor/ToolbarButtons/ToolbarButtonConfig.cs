#if UNITY_EDITOR
using System;

namespace VMFramework.Editor.GameEditor
{
    public struct ToolbarButtonConfig
    {
        public string path;
        public string tooltip;
        public Action onClick;
            
        public ToolbarButtonConfig(string path, Action onClick)
        {
            this.path = path;
            tooltip = path;
            this.onClick = onClick;
        }
            
        public ToolbarButtonConfig(string path, string tooltip, Action onClick)
        {
            this.path = path;
            this.tooltip = tooltip;
            this.onClick = onClick;
        }
    }
}
#endif