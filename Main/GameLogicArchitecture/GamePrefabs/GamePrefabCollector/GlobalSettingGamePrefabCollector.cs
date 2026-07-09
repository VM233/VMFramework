using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.Scripting;

namespace VMFramework.GameLogicArchitecture
{
    [Preserve]
    public sealed class GlobalSettingGamePrefabCollector : IGamePrefabCollector
    {
        UniTask<IEnumerable<IGamePrefab>> IGamePrefabCollector.CollectGamePrefabs()
        {
            var gamePrefabs = new List<IGamePrefab>();

            foreach (var generalSetting in GlobalSettingCollector.GetAllGeneralSettings())
            {
                if (generalSetting is IGamePrefabsProvider provider)
                {
                    provider.GetGamePrefabs(gamePrefabs);
                }
            }

            return new UniTask<IEnumerable<IGamePrefab>>(gamePrefabs);
        }
    }
}