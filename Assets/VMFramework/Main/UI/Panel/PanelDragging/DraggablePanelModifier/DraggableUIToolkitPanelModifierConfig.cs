using System;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class DraggableUIToolkitPanelModifierConfig : PanelModifierConfig
    {
        public override Type GameItemType => typeof(DraggableUIToolkitPanelModifier);

        [TabGroup(TAB_GROUP_NAME, MODIFIER_CATEGORY)]
        [VisualElementName]
        [JsonProperty]
        public string draggableAreaName;

        [TabGroup(TAB_GROUP_NAME, MODIFIER_CATEGORY)]
        [VisualElementName]
        [JsonProperty]
        public string draggingContainerName;

        [TabGroup(TAB_GROUP_NAME, MODIFIER_CATEGORY)]
        [JsonProperty]
        public bool draggableOverflowScreen = false;

        public override void CheckSettings()
        {
            base.CheckSettings();
            
            draggableAreaName.AssertIsNotNull(nameof(draggableAreaName));
            draggingContainerName.AssertIsNotNull(nameof(draggingContainerName));
        }
    }
}