using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture
{
    public static class GamePrefabTypeQuery
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasGameItem([DisallowNull] Type gamePrefabType)
        {
            var gamePrefab = (IGamePrefab)InstanceCache.Get(gamePrefabType);
            return gamePrefab.GameItemType != null;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type GetGameItemType([DisallowNull] Type gamePrefabType)
        {
            var gamePrefab = (IGamePrefab)InstanceCache.Get(gamePrefabType);
            return gamePrefab?.GameItemType;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Type> GetGamePrefabTypesByGameItemType(Type gameItemType)
        {
            foreach (var gamePrefabType in typeof(IGamePrefab).GetDerivedClasses(false, false))
            {
                if (gamePrefabType.IsInterface || gamePrefabType.IsAbstract)
                {
                    continue;
                }

                var gamePrefab = (IGamePrefab)InstanceCache.Get(gamePrefabType);

                if (gamePrefab.GameItemType == null)
                {
                    continue;
                }

                if (gamePrefab.GameItemType == gameItemType)
                {
                    yield return gamePrefabType;
                }
            }
            
            foreach (var gamePrefabType in typeof(IGamePrefab).GetDerivedClasses(false, false))
            {
                if (gamePrefabType.IsInterface || gamePrefabType.IsAbstract)
                {
                    continue;
                }

                var gamePrefab = (IGamePrefab)InstanceCache.Get(gamePrefabType);

                if (gamePrefab.GameItemType == null)
                {
                    continue;
                }

                if (gameItemType.IsDerivedFrom(gamePrefab.GameItemType, includingSelf: false))
                {
                    yield return gamePrefabType;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetGamePrefabTypeByGameItemType(Type gameItemType, out Type gamePrefabType)
        {
            gamePrefabType = GetGamePrefabTypesByGameItemType(gameItemType).FirstOrDefault();
            return gamePrefabType != null;
        }
    }
}