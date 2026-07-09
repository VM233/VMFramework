using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace VMFramework.GameLogicArchitecture
{
    public interface IGamePrefabCollector
    {
        public UniTask<IEnumerable<IGamePrefab>> CollectGamePrefabs();
    }
}