using UnityEngine.UIElements;

namespace VMFramework.UI
{
    public abstract class UIToolkitPanelModifier : PanelModifier
    {
        public IUIToolkitPanel UIToolkitPanel => (IUIToolkitPanel)Panel;
        
        public VisualElement RootVisualElement => UIToolkitPanel.RootVisualElement;
    }
}