#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using VMFramework.Core.Editor;
using VMFramework.Core.Linq;
using VMFramework.Editor;

namespace VMFramework.GameLogicArchitecture.Editor
{
    public static class GamePrefabWrapperRemover
    {
        private static readonly List<IGamePrefab> gamePrefabsCache = new();
        
        [MenuItem(UnityMenuItemNames.GAME_PREFABS_TOOLS + "Remove Empty GamePrefab Wrappers")]
        public static void RemoveEmptyWrappers()
        {
            foreach (var wrapper in GamePrefabWrapperQueryTools.GetAllGamePrefabWrappers())
            {
                gamePrefabsCache.Clear();
                wrapper.GetGamePrefabs(gamePrefabsCache);

                if (gamePrefabsCache.IsNullOrEmptyOrAllNull())
                {
                    wrapper.DeleteAsset();
                }
            }
        }
        
        public static void RemoveGamePrefabWrapper(IGamePrefab gamePrefab)
        {
            if (gamePrefab == null)
            {
                return;
            }

            foreach (var wrapper in GamePrefabWrapperQueryTools.GetGamePrefabWrappers(gamePrefab)
                         .ToList())
            {
                if (gamePrefab.TryGetGamePrefabGeneralSettingWithWarning(out var gamePrefabGeneralSetting))
                {
                    gamePrefabGeneralSetting.RemoveFromInitialGamePrefabProviders(wrapper);
                }
                
                wrapper.DeleteAsset();
            }
        }

        public static void RemoveGamePrefabWrapperWhere<TGamePrefab>(Func<TGamePrefab, bool> predicate)
            where TGamePrefab : IGamePrefab
        {
            var gamePrefabGeneralSetting =
                GamePrefabGeneralSettingUtility.GetGamePrefabGeneralSetting(typeof(TGamePrefab));
            
            foreach (var room in GamePrefabManager.GetAllGamePrefabs<TGamePrefab>())
            {
                if (predicate(room))
                {
                    foreach (var wrapper in GamePrefabWrapperQueryTools.GetGamePrefabWrappers(room))
                    {
                        if (gamePrefabGeneralSetting != null)
                        {
                            gamePrefabGeneralSetting.RemoveFromInitialGamePrefabProviders(wrapper);
                        }
                        
                        wrapper.DeleteAsset();
                    }
                }
            }
        }
    }
}
#endif