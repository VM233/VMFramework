using System.Collections.Generic;

namespace VMFramework.GameLogicArchitecture
{
    public interface IGamePrefabProvider
    {
        public void GetGamePrefabs(ICollection<IGamePrefab> gamePrefabsCollection);
    }
}