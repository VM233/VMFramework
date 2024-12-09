using System;
using System.Collections.Generic;
using VMFramework.Containers;

namespace VMFramework.UI
{
    public interface IContainerUIPanel : IUIPanel
    {
        public int ContainerUIPriority { get; }
        
        public IReadOnlyCollection<IContainer> BindContainers { get; }

        public event Action<IContainerUIPanel, IContainer> OnBindContainerAdded;
        public event Action<IContainerUIPanel, IContainer> OnBindContainerRemoved;

        public void AddBindContainer(IContainer newBindContainer);
        
        public void RemoveBindContainer(IContainer oldBindContainer);
    }
}