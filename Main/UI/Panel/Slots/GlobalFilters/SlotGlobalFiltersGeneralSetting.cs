using System;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI 
{
    public sealed partial class SlotGlobalFiltersGeneralSetting : GamePrefabGeneralSetting
    {
        #region Meta Data

        public override Type BaseGamePrefabType => typeof(SlotFilterConfig);

        #endregion
    }
}