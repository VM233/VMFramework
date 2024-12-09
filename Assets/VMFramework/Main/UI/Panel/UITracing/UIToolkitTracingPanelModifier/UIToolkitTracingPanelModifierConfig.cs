using System;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class UIToolkitTracingPanelModifierConfig : TracingPanelModifierConfig
    {
        public override Type GameItemType => typeof(UIToolkitTracingPanelModifier);
        
        [LabelWidth(200), TabGroup(TAB_GROUP_NAME, TRACING_UI_SETTING_CATEGORY)]
        [JsonProperty]
        public bool useRightPosition;

        [TabGroup(TAB_GROUP_NAME, TRACING_UI_SETTING_CATEGORY)]
        [LabelWidth(200)]
        [JsonProperty]
        public bool useTopPosition;

        [TabGroup(TAB_GROUP_NAME, TRACING_UI_SETTING_CATEGORY)]
        [VisualElementName]
        [IsNotNullOrEmpty]
        [JsonProperty]
        public string containerVisualElementName;
    }
}