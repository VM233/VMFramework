using System.Collections.Generic;

namespace VMFramework.Containers
{
    public interface IContainerProvider : IContainersProvider
    {
        public IContainer Container { get; }

        void IContainersProvider.GetContainers(ICollection<IContainer> containers)
        {
            var container = Container;
            if (container != null)
            {
                containers.Add(Container);
            }
        }
    }
}