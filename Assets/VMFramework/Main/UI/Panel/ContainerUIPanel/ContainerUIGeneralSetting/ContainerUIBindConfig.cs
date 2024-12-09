using VMFramework.Configuration;
using VMFramework.Containers;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public sealed class ContainerUIBindConfig : BaseConfig, IIDOwner<string>
    {
        [GamePrefabID(typeof(IContainerConfig))]
        [IsNotNullOrEmpty]
        public string containerID;

        [GamePrefabID(typeof(IContainerUIConfig))]
        [IsNotNullOrEmpty]
        public string uiID;

        string IIDOwner<string>.id => containerID;
    }
}