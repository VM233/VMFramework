using System;
using VMFramework.GameLogicArchitecture;
using Newtonsoft.Json;
using Sirenix.OdinInspector;

namespace VMFramework.UI
{
    public sealed partial class DebugPanelGeneralSetting : GamePrefabGeneralSetting
    {
        #region Setting MetaData

        public override Type BaseGamePrefabType => typeof(DebugEntry);

        #endregion

        [SuffixLabel("seconds")]
        [JsonProperty]
        [PropertyRange(0.1f, 1f)]
        public float updateInterval = 0.2f;
    }
}
