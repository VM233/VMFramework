using System;
using VMFramework.GameEvents;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Containers
{
    [GamePrefabTypeAutoRegister(ADDED_EVENT_ID)]
    [GamePrefabTypeAutoRegister(REMOVED_EVENT_ID)]
    public sealed class ContainerItemChangedEventConfig : GameEventConfig
    {
        public const string ADDED_EVENT_ID = "container_item_added_event";
        public const string REMOVED_EVENT_ID = "container_item_removed_event";

        public override Type GameItemType => typeof(ContainerItemChangedEvent);
    }
}