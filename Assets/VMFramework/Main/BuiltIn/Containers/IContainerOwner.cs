using System.Collections.Generic;

namespace VMFramework.Containers
{
    public interface IContainerOwner
    {
        public void GetContainers(ICollection<IContainer> containers);
    }
}
