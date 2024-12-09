namespace VMFramework.Core
{
    public delegate void EnabledChangedEventHandler(bool previous, bool current);
    
    public interface ITokenEnabledOwner
    {
        public bool IsEnabled { get; }

        public event EnabledChangedEventHandler OnEnabledChangedEvent;
        
        public void Enable(IToken token);
        
        public void Disable(IToken token);
    }
}