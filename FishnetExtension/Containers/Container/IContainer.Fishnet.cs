using VMFramework.Properties;

#if FISHNET
namespace VMFramework.Containers
{
    public partial interface IContainer
    {
        public delegate void OpenStateChangedHandler(IContainer container, bool isOpen);
        
        public IValueProperty<bool> IsOpen { get; }
    }
}
#endif