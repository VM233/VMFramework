using UnityEngine.UIElements;

namespace VMFramework.UI
{
    public interface IUIToolkitPanelConfig : IUIPanelConfig, IVisualTreeAssetProvider
    {
        public PanelSettings PanelSettings { get; }
        
        public bool IgnoreMouseEvents { get; }
        
        public string UIMainPartName { get; }
    }
}