using System.Collections.Generic;

namespace VMFramework.Containers
{
    public interface IContainersProvider
    {
        public void GetContainers(ICollection<IContainer> containers);
    }
}