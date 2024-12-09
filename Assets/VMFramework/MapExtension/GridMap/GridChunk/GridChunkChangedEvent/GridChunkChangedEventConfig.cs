using System;
using VMFramework.GameEvents;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Maps
{
    [GamePrefabTypeAutoRegister(ID)]
    public sealed class GridChunkChangedEventConfig : GameEventConfig
    {
        public const string ID = "grid_chunk_changed_event";
        
        public override Type GameItemType => typeof(GridChunkChangedEvent);
    }
}