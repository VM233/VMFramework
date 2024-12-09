using System;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Containers
{
    public sealed partial class ContainerGeneralSetting : GamePrefabGeneralSetting
    {
        #region Meta Data

        public override Type BaseGamePrefabType => typeof(ContainerConfig);
        
        public override string GameItemName => nameof(Container);

        #endregion
    }
}
