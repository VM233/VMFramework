using System;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using VMFramework.GameEvents;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public sealed class UICloseOnEventTriggeredPanelModifierConfig : PanelModifierConfig
    {
        public override Type GameItemType => typeof(UICloseOnEventTriggeredPanelModifier);

        [TabGroup(TAB_GROUP_NAME, MODIFIER_CATEGORY)]
        [GamePrefabID(typeof(IGameEventConfig))]
        [IsNotNullOrEmpty]
        [JsonProperty]
        public string uiCloseGameEventID;
    }
}