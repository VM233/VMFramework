using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using VMFramework.GameEvents;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public sealed partial class EventsDisabledOnOpenPanelModifierConfig : PanelModifierConfig
    {
        public override Type GameItemType => typeof(EventsDisabledOnOpenPanelModifier);

        [TabGroup(TAB_GROUP_NAME, MODIFIER_CATEGORY)]
        [GamePrefabID(typeof(IGameEventConfig))]
        [ListDrawerSettings(ShowFoldout = false)]
        [DisallowDuplicateElements]
        [JsonProperty]
        public List<string> gameEventDisabledOnOpen = new();
    }
}