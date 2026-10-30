using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace VMFramework.GameLogicArchitecture
{
    [UnityEngine.Scripting.RequireImplementors]
    public interface IGamePrefabCollector
    {
        public UniTask<IEnumerable<IGamePrefab>> CollectGamePrefabs();
    }
}
