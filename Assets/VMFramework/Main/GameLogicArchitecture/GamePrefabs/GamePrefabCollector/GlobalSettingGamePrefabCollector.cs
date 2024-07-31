﻿using System.Collections.Generic;
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
                if (generalSetting is IInitialGamePrefabProvider initialGamePrefabProvider)
                {
                    gamePrefabs.AddRange(initialGamePrefabProvider.GetInitialGamePrefabs());
                }
            }

            return new UniTask<IEnumerable<IGamePrefab>>(gamePrefabs);
        }
    }
}