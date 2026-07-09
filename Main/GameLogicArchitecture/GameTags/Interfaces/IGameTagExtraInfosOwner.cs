using System.Collections.Generic;

namespace VMFramework.GameLogicArchitecture
{
    public interface IGameTagExtraInfosOwner
    {
        public IReadOnlyDictionary<string, GameTagExtraInfo> GameTagExtraInfos { get; }
    }
}