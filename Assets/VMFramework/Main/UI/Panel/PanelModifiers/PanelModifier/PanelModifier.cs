using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI
{
    public abstract class PanelModifier : ControllerGameItem, IPanelModifier
    {
        public IUIPanel Panel { get; private set; }
        
        protected IUIPanelConfig UIPanelConfig { get; private set; }
        
        public void Initialize(IUIPanel panel, IUIPanelConfig config)
        {
            Panel = panel;
            UIPanelConfig = config;
            OnInitialize();
        }

        protected virtual void OnInitialize()
        {
        }
    }
}