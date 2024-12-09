using System;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Maps 
{
    public sealed partial class TileBaseConfigGeneralSetting : GamePrefabGeneralSetting
    {
        #region Meta Data

        public override Type BaseGamePrefabType => typeof(ITileBaseConfig);

        #endregion

        
    }
}
