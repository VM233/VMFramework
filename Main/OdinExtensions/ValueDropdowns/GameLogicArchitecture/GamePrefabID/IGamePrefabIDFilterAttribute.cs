using System.Collections.Generic;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.OdinExtensions
{
    public interface IGamePrefabIDFilterAttribute
    {
        public IEnumerable<IGamePrefab> GetGamePrefabs();
    }
}