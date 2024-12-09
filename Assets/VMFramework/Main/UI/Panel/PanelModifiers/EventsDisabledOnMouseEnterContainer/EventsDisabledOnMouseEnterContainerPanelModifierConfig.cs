using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.GameEvents;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public sealed partial class EventsDisabledOnMouseEnterContainerPanelModifierConfig : PanelModifierConfig
    {
        public override Type GameItemType => typeof(EventsDisabledOnMouseEnterContainerPanelModifier);

        [TabGroup(TAB_GROUP_NAME, MODIFIER_CATEGORY)]
        [VisualElementName]
        [IsNotNullOrEmpty]
        public string containerName;

        [TabGroup(TAB_GROUP_NAME, MODIFIER_CATEGORY)]
        [ListDrawerSettings(ShowFoldout = false)]
        [GamePrefabID(typeof(IGameEventConfig))]
        public List<string> gameEventsDisabledOnMouseEnter = new();
    }
}