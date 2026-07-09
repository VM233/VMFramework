using UnityEngine.UIElements;

namespace VMFramework.UI
{
    public interface IUIToolkitPanelConfig : IUIPanelConfig, IVisualTreeAssetProvider
    {
        public PanelSettings PanelSettings { get; }
        
        public bool IgnoreMouseEvents { get; }
        
        public UIToolkitPanelCloseMode CloseMode { get; }
    }
}