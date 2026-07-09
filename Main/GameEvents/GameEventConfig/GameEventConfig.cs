using System;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.GameEvents
{
    public class GameEventConfig : LocalizedGamePrefab, IGameEventConfig
    {
        public override string IDSuffix => "event";

        public override Type GameItemType => typeof(ParameterlessGameEvent);
    }
}