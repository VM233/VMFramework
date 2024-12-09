using System;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public sealed class UICloseButtonPanelModifierConfig : PanelModifierConfig
    {
        public override Type GameItemType => typeof(UICloseButtonPanelModifier);

        [TabGroup(TAB_GROUP_NAME, MODIFIER_CATEGORY)]
        [VisualElementName(typeof(Button))]
        [IsNotNullOrEmpty]
        [JsonProperty]
        public string closeUIButtonName;
    }
}