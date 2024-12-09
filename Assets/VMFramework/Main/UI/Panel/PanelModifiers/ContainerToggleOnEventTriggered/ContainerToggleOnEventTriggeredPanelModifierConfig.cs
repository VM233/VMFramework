using System;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using VMFramework.GameEvents;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public sealed class ContainerToggleOnEventTriggeredPanelModifierConfig : PanelModifierConfig
    {
        public override Type GameItemType => typeof(ContainerToggleOnEventTriggeredPanelModifier);

        [TabGroup(TAB_GROUP_NAME, MODIFIER_CATEGORY)]
        [VisualElementName]
        [IsNotNullOrEmpty]
        public string containerName;
        
        [TabGroup(TAB_GROUP_NAME, MODIFIER_CATEGORY)]
        [GamePrefabID(typeof(IGameEventConfig))]
        [IsNotNullOrEmpty]
        [JsonProperty]
        public string containerToggleGameEventID;
    }
}