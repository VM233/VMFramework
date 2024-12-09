using VMFramework.Configuration;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public sealed partial class TooltipGeneralSetting : GeneralSetting
    {
        private const string TOOLTIP_CATEGORY = "Tooltip";

        private const string TOOLTIP_ID_BIND_CATEGORY = TAB_GROUP_NAME + "/" + TOOLTIP_CATEGORY + "/Tooltip ID Bind";

        private const string TOOLTIP_PRIORITY_CATEGORY =
            TAB_GROUP_NAME + "/" + TOOLTIP_CATEGORY + "/Tooltip Priority Bind";

        [TabGroup(TAB_GROUP_NAME, TOOLTIP_CATEGORY), TitleGroup(TOOLTIP_ID_BIND_CATEGORY)]
        [GamePrefabID(typeof(ITooltipConfig))]
        [IsNotNullOrEmpty]
        [JsonProperty]
        public string defaultTooltipID;

        [TitleGroup(TOOLTIP_ID_BIND_CATEGORY)]
        [JsonProperty]
        public GameTagBasedConfigs<TooltipBindConfig> tooltipIDBindConfigs = new();

        [TitleGroup(TOOLTIP_PRIORITY_CATEGORY)]
        [JsonProperty, SerializeField]
        public DictionaryConfigs<string, PriorityPreset> tooltipPriorityPresets = new();

        [TitleGroup(TOOLTIP_PRIORITY_CATEGORY)]
        public GameTagBasedConfigs<TooltipPriorityBindConfig> tooltipPriorityBindConfigs = new();

        [TitleGroup(TOOLTIP_PRIORITY_CATEGORY)]
        public TooltipPriorityConfig defaultPriority;

        public override void CheckSettings()
        {
            base.CheckSettings();

            if (defaultTooltipID.IsNullOrEmpty())
            {
                Debugger.LogWarning($"{nameof(defaultTooltipID)} is not set.");
            }

            tooltipIDBindConfigs.CheckSettings();

            tooltipPriorityPresets.CheckSettings();

            tooltipPriorityBindConfigs.CheckSettings();
        }

        protected override void OnInit()
        {
            base.OnInit();

            tooltipIDBindConfigs.Init();

            tooltipPriorityPresets.Init();

            tooltipPriorityBindConfigs.Init();
        }
    }
}
