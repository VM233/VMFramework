using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.Containers;

namespace VMFramework.UI
{
    public class ContainerUIPanel : UIToolkitPanel, IContainerUIPanel
    {
        protected IContainerUIConfig ContainerUIConfig => (IContainerUIConfig)GamePrefab;

        public int ContainerUIPriority => ContainerUIConfig.ContainerUIPriority;

        public event Action<IContainerUIPanel, IContainer> OnBindContainerAdded;
        public event Action<IContainerUIPanel, IContainer> OnBindContainerRemoved;

        [ShowInInspector]
        private readonly HashSet<IContainer> bindContainers = new();

        public IReadOnlyCollection<IContainer> BindContainers => bindContainers;

        public void GetBindContainers(ICollection<IContainer> containers)
        {
            containers.AddRange(bindContainers);
        }

        public void AddBindContainer(IContainer newBindContainer)
        {
            if (bindContainers.Add(newBindContainer))
            {
                OnBindContainerAdded?.Invoke(this, newBindContainer);
            }
        }

        public void RemoveBindContainer(IContainer oldBindContainer)
        {
            if (bindContainers.Remove(oldBindContainer))
            {
                OnBindContainerRemoved?.Invoke(this, oldBindContainer);
            }
        }

        protected override void OnPostClose()
        {
            base.OnPostClose();
            
            foreach (var container in bindContainers)
            {
                OnBindContainerRemoved?.Invoke(this, container);
            }
            
            bindContainers.Clear();
        }
    }
}