using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using VMFramework.Core.Pools;

namespace VMFramework.GameLogicArchitecture.Editor
{
    public static class GamePrefabProviderGameItemUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasAnyGameItem(this IGamePrefabsProvider provider)
        {
            if (provider == null)
            {
                return false;
            }

            var gamePrefabs = ListPool<IGamePrefab>.Default.Get();
            gamePrefabs.Clear();
            provider.GetGamePrefabs(gamePrefabs);
            
            var result = gamePrefabs.Any(gamePrefab => gamePrefab?.GameItemType != null);
            
            gamePrefabs.ReturnToDefaultPool();
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasAnyGameItem<TProviders>(this TProviders providers) where TProviders : IEnumerable<IGamePrefabsProvider>
        {
            if (providers == null)
            {
                return false;
            }
            
            return providers.Any(gamePrefabProvider => gamePrefabProvider.HasAnyGameItem());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Type> GetGameItemTypes(this IGamePrefabsProvider provider)
        {
            if (provider == null)
            {
                yield break;
            }
            
            var gamePrefabs = ListPool<IGamePrefab>.Default.Get();
            gamePrefabs.Clear();
            provider.GetGamePrefabs(gamePrefabs);

            foreach (var gamePrefab in gamePrefabs)
            {
                if (gamePrefab?.GameItemType != null)
                {
                    yield return gamePrefab.GameItemType;
                }
            }
            
            gamePrefabs.ReturnToDefaultPool();
        }
    }
}