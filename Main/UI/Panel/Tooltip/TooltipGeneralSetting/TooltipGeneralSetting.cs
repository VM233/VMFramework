using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMFramework.Configuration;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    [CommonPresetAutoRegister(TOOLTIP_PRIORITY_PRESET_KEY, typeof(int))]
    public sealed partial class TooltipGeneralSetting : GeneralSetting
    {
        private const string TOOLTIP_CATEGORY = "Tooltip";

        private const string TOOLTIP_ID_BIND_CATEGORY = TAB_GROUP_NAME + "/" + TOOLTIP_CATEGORY + "/Tooltip ID Bind";

        private const string TOOLTIP_PRIORITY_CATEGORY =
            TAB_GROUP_NAME + "/" + TOOLTIP_CATEGORY + "/Tooltip Priority Bind";

        public const string TOOLTIP_PRIORITY_PRESET_KEY = "Tooltip Priority";

        [TabGroup(TAB_GROUP_NAME, TOOLTIP_CATEGORY), TitleGroup(TOOLTIP_ID_BIND_CATEGORY)]
        [GamePrefabID(typeof(IUIPanelConfig))]
        [IsNotNullOrEmpty]
        [JsonProperty]
        public string defaultTooltipID;

        #region Check & Init

        public override void CheckSettings()
        {
            base.CheckSettings();

            if (defaultTooltipID.IsNullOrEmpty())
            {
                UnityEngine.Debug.LogWarning($"{nameof(defaultTooltipID)} is not set.");
            }
        }

        #endregion
    }
}
