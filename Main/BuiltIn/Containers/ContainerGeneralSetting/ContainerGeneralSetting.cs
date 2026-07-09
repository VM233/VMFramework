using System;
using VMFramework.Configuration;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.Containers
{
    [CommonPresetAutoRegister(CONTAINER_INTENTION_PRESET_KEY, typeof(string))]
    public sealed partial class ContainerGeneralSetting : GamePrefabGeneralSetting
    {
        public const string CONTAINER_INTENTION_PRESET_KEY = "Container Intention";

        #region Meta Data

        public override Type BaseGamePrefabType => typeof(ContainerConfig);

        #endregion

        [IsNotNullOrEmpty]
        public string transformContainerName = "#Containers";
    }
}