using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI
{
    public abstract class ProviderWrapper : ControllerGameItem, IProviderWrapper
    {
        public abstract object Source { get; }
        
        public abstract void SetSource(object source);
    }
}