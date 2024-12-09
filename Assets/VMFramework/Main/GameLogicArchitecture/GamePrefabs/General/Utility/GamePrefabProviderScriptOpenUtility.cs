#if UNITY_EDITOR
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Core.Editor;
using VMFramework.Core.Linq;
using VMFramework.Core.Pools;

namespace VMFramework.GameLogicArchitecture.Editor
{
    public static class GamePrefabProviderScriptOpenUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OpenGamePrefabScripts(this IGamePrefabProvider provider)
        {
            if (provider == null)
            {
                Debug.LogError($"{nameof(provider)} is null! Cannot open {nameof(GamePrefab)} scripts!");
                return;
            }
            
            var gamePrefabsCache = ListPool<IGamePrefab>.Default.Get();
            gamePrefabsCache.Clear();
            provider.GetGamePrefabs(gamePrefabsCache);

            try
            {
                gamePrefabsCache.OpenScriptOfObjects();
            }
            finally
            {
                gamePrefabsCache.ReturnToDefaultPool();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OpenGameItemScripts(this IGamePrefabProvider provider)
        {
            if (provider == null)
            {
                Debug.LogError($"{nameof(provider)} is null! Cannot open {nameof(GameItem)} scripts!");
                return;
            }

            provider.GetGameItemTypes().Examine(gameItemType => gameItemType.OpenScriptOfType());
        }
    }
}
#endif