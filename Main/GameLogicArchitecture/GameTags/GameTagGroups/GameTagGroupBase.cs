using System.Collections.Generic;
using UnityEngine;

namespace VMFramework.GameLogicArchitecture
{
    public abstract partial class GameTagGroupBase : ScriptableObject
    {
        public abstract IEnumerable<GameTagInfo> GetGameTagInfos();
    }
}