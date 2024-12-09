using System.Collections.Generic;

namespace VMFramework.GameLogicArchitecture
{
    public interface IGameTagsOwner
    {
        public ICollection<string> GameTags { get; }
    }
}