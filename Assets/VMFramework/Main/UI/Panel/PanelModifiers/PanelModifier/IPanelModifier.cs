using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI
{
    public interface IPanelModifier : IControllerGameItem
    {
        public IUIPanel Panel { get; }
        
        public void Initialize(IUIPanel panel, IUIPanelConfig config);
    }
}