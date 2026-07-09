using System;
using Sirenix.OdinInspector;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Effects
{
    public sealed partial class EffectGeneralSetting : GamePrefabGeneralSetting
    {
        #region Meta Data

        public override Type BaseGamePrefabType => typeof(IEffectConfig);

        #endregion

        [TabGroup(TAB_GROUP_NAME, MISCELLANEOUS_CATEGORY)]
        public string containerName = "@Effect";
    }
}
