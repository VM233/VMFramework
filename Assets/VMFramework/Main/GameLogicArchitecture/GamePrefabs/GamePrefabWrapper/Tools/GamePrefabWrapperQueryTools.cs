#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using VMFramework.Core;
using VMFramework.Core.Editor;
using VMFramework.Core.Pools;

namespace VMFramework.GameLogicArchitecture.Editor
{
    public static class GamePrefabWrapperQueryTools
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<GamePrefabWrapper> GetAllGamePrefabWrappers()
        {
            return typeof(GamePrefabWrapper).FindAssetsOfType().Cast<GamePrefabWrapper>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetGamePrefabWrapper(string id, out GamePrefabWrapper wrapper)
        {
            wrapper = GetGamePrefabWrapper(id);
            return wrapper != null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GamePrefabWrapper GetGamePrefabWrapper(string id)
        {
            if (GamePrefabManager.TryGetGamePrefab(id, out var gamePrefab) == false)
            {
                return null;
            }
            
            return GetGamePrefabWrapper(gamePrefab);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GamePrefabWrapper GetGamePrefabWrapper(IGamePrefab gamePrefab)
        {
            return GetGamePrefabWrappers(gamePrefab).FirstOrDefault();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<GamePrefabWrapper> GetGamePrefabWrappers(IGamePrefab gamePrefab)
        {
            if (gamePrefab == null)
            {
                yield break;
            }

            var gamePrefabsCache = ListPool<IGamePrefab>.Default.Get();
            
            foreach (var gamePrefabWrapper in GetAllGamePrefabWrappers())
            {
                gamePrefabsCache.Clear();
                gamePrefabWrapper.GetGamePrefabs(gamePrefabsCache);
                
                foreach (var existingGamePrefab in gamePrefabsCache)
                {
                    if (existingGamePrefab == gamePrefab)
                    {
                        yield return gamePrefabWrapper;
                        break;
                    }
                }
            }
            
            gamePrefabsCache.ReturnToDefaultPool();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<GamePrefabWrapper> GetGamePrefabWrappers(Type gamePrefabType)
        {
            if (gamePrefabType == null)
            {
                yield break;
            }

            if (gamePrefabType.IsDerivedFrom<IGamePrefab>(true) == false)
            {
                yield break;
            }

            var gamePrefabsCache = ListPool<IGamePrefab>.Default.Get();
            
            foreach (var gamePrefabWrapper in GetAllGamePrefabWrappers())
            {
                gamePrefabsCache.Clear();
                gamePrefabWrapper.GetGamePrefabs(gamePrefabsCache);
                
                foreach (var existingGamePrefab in gamePrefabsCache)
                {
                    if (existingGamePrefab.GetType().IsDerivedFrom(gamePrefabType, true))
                    {
                        yield return gamePrefabWrapper;
                        break;
                    }
                }
            }
            
            gamePrefabsCache.ReturnToDefaultPool();
        }
    }
}
#endif