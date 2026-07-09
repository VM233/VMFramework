using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture
{
    public static class GamePrefabCollectorManager
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async UniTask<IEnumerable<IGamePrefab>> Collect()
        {
            var result = new List<IGamePrefab>();
            
            foreach (var collectorType in typeof(IGamePrefabCollector).GetDerivedInstantiableClasses(false))
            {
                var collector = (IGamePrefabCollector)collectorType.CreateInstance();

                var gamePrefabs = await collector.CollectGamePrefabs();
                
                result.AddRange(gamePrefabs);
            }

            return result;
        }
    }
}