#if FISHNET
using VMFramework.Properties;

namespace VMFramework.Containers
{
    public partial class Container
    {
        public IValueProperty<bool> IsOpen => isOpenProperty;

        protected readonly SimpleProperty<bool> isOpenProperty = new();

        public Container()
        {
            isOpenProperty.SetOwner(this);
        }
    }
}
#endif