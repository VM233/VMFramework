using VMFramework.Core;

namespace VMFramework.UI
{
    public interface IPanelModifier : IController
    {
        public IUIPanel Panel { get; }
        
        public int InitializePriority { get; }
        
        public void Initialize(IUIPanel panel, IUIPanelConfig config);

        public void Deinitialize();
    }
}