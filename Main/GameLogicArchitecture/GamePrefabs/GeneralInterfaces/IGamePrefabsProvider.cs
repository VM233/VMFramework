using System.Collections.Generic;

namespace VMFramework.GameLogicArchitecture
{
    public interface IGamePrefabsProvider
    {
        public void GetGamePrefabs(ICollection<IGamePrefab> gamePrefabsCollection);
    }
}