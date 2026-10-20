using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using VMFramework.Core;
using VMFramework.Procedure;

namespace VMFramework.GameLogicArchitecture
{
    public static class GamePrefabCollectorManager
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async UniTask<IReadOnlyList<IGamePrefab>> Collect(CancellationToken cancellationToken)
        {
            var result = new List<IGamePrefab>();
            
            var types = await InitializationTypeDiscovery.FindAsync(typeof(IGamePrefabCollector), false, cancellationToken);
            foreach (var collectorType in types)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var collector = (IGamePrefabCollector)collectorType.CreateInstance();

                var gamePrefabs = await collector.CollectGamePrefabs();
                cancellationToken.ThrowIfCancellationRequested();
                
                result.AddRange(gamePrefabs);
            }

            return result;
        }
    }
}
