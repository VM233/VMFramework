using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GamePrefabManager
    {
        #region By Game Item Type

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IGamePrefab> GetGamePrefabsByGameItemType(Type gameItemType)
        {
            if (gameItemType != null && gameItemType.IsDerivedFrom<IGameItem>(true) == false)
            {
                yield break;
            }
            
            foreach (var (_, gamePrefabs) in allGamePrefabsByType)
            {
                var firstGamePrefab = gamePrefabs.First();

                if (gameItemType == null)
                {
                    if (firstGamePrefab.GameItemType != null)
                    {
                        continue;
                    }

                    foreach (var gamePrefab in gamePrefabs)
                    {
                        yield return gamePrefab;
                    }
                }
                else
                {
                    if (firstGamePrefab.GameItemType.IsDerivedFrom(gameItemType, true,
                            gameItemType.IsGenericTypeDefinition))
                    {
                        foreach (var gamePrefab in gamePrefabs)
                        {
                            yield return gamePrefab;
                        }
                    }
                }
            }
        }

        #endregion
        
        #region By Type

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TGamePrefab> GetGamePrefabsByType<TGamePrefab>()
            where TGamePrefab : IGamePrefab
        {
            foreach (var gamePrefab in allGamePrefabsByID.Values)
            {
                if (gamePrefab is TGamePrefab typedGamePrefab)
                {
                    yield return typedGamePrefab;
                }
            }
        }
        
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IGamePrefab> GetGamePrefabsByType(Type gamePrefabType)
        {
            if (gamePrefabType == typeof(IGamePrefab))
            {
                foreach (var gamePrefab in allGamePrefabsByID.Values)
                {
                    yield return gamePrefab;
                }
            }
            
            foreach (var (type, gamePrefabs) in allGamePrefabsByType)
            {
                if (type.IsDerivedFrom(gamePrefabType, true, gamePrefabType.IsGenericTypeDefinition))
                {
                    foreach (var gamePrefab in gamePrefabs)
                    {
                        yield return gamePrefab;
                    }
                }
            }
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IGamePrefab> GetGamePrefabsByTypes(params Type[] gamePrefabTypes)
        {
            if (gamePrefabTypes.Any(type => type == typeof(IGamePrefab)))
            {
                foreach (var gamePrefab in allGamePrefabsByID.Values)
                {
                    yield return gamePrefab;
                }
            }
            
            foreach (var (type, gamePrefabs) in allGamePrefabsByType)
            {
                foreach (var gamePrefabType in gamePrefabTypes)
                {
                    if (type.IsDerivedFrom(gamePrefabType, true, gamePrefabType.IsGenericTypeDefinition))
                    {
                        foreach (var gamePrefab in gamePrefabs)
                        {
                            yield return gamePrefab;
                        }

                        break;
                    }
                }
            }
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IGamePrefab> GetGamePrefabsByTypes(IEnumerable<Type> gamePrefabTypes)
        {
            return GetGamePrefabsByTypes(gamePrefabTypes.ToArray());
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TGamePrefab> GetGamePrefabsByTypes<TGamePrefab>(
            params Type[] gamePrefabTypes) where TGamePrefab : IGamePrefab
        {
            foreach (var gamePrefab in GetGamePrefabsByTypes(gamePrefabTypes))
            {
                if (gamePrefab is TGamePrefab typedGamePrefab)
                {
                    yield return typedGamePrefab;
                }
            }
        }

        #endregion
    }
}