using System;
using System.Collections.Generic;
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture
{
    public static class GamePrefabReferenceValidator
    {
        public static void Validate(IEnumerable<IGamePrefab> gamePrefabs)
        {
            var missingGamePrefabs = new List<IGamePrefab>();

            foreach (var gamePrefab in gamePrefabs)
            {
                if (gamePrefab is IPrefabProvider prefabProvider &&
                    prefabProvider.Prefab == null)
                {
                    missingGamePrefabs.Add(gamePrefab);
                }
            }

            if (missingGamePrefabs.Count == 0)
            {
                return;
            }

            missingGamePrefabs.Sort((left, right) =>
                StringComparer.Ordinal.Compare(left.id, right.id));
            throw new MissingGamePrefabReferencesException(missingGamePrefabs);
        }
    }
}
