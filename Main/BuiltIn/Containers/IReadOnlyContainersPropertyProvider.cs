using System.Collections.Generic;
using VMFramework.Properties;

namespace VMFramework.Containers
{
    public interface IReadOnlyContainersPropertyProvider : IContainersProvider
    {
        public IReadOnlyCollectionProperty<IContainer> Containers { get; }

        void IContainersProvider.GetContainers(ICollection<IContainer> containers)
        {
            Containers.GetValues(containers);
        }
    }
}