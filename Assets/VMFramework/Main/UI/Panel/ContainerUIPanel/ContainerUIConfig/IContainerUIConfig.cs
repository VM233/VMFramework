using System.Collections.Generic;

namespace VMFramework.UI
{
    public interface IContainerUIConfig : IUIPanelConfig
    {
        public int ContainerUIPriority { get; }
    }
}