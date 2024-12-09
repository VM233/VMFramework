using System;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public partial class UIToolkitTracingTooltipConfig : UIToolkitPanelConfig, ITooltipConfig
    {
        protected const string TOOLTIP_SETTING_CATEGORY = "Tooltip";

        public override Type GameItemType => typeof(UIToolkitTooltip);

        [TabGroup(TAB_GROUP_NAME, TOOLTIP_SETTING_CATEGORY)]
        [VisualElementName(typeof(Label))]
        [IsNotNullOrEmpty]
        [JsonProperty]
        public string titleLabelName;

        [TabGroup(TAB_GROUP_NAME, TOOLTIP_SETTING_CATEGORY)]
        [VisualElementName(typeof(Label))]
        [IsNotNullOrEmpty]
        [JsonProperty]
        public string descriptionLabelName;

        [TabGroup(TAB_GROUP_NAME, TOOLTIP_SETTING_CATEGORY)]
        [VisualElementName]
        [IsNotNullOrEmpty]
        [JsonProperty]
        public string propertyContainerName;
    }
}