#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VMFramework.Procedure;
using VMFramework.Procedure.Editor;

namespace VMFramework.GameLogicArchitecture.Editor
{
    internal sealed class GamePrefabWrapperEditorInitializer : IEditorInitializer
    {
        private static readonly List<IGamePrefab> gamePrefabsCache = new();
        private static readonly HashSet<GamePrefabGeneralSetting> gamePrefabGeneralSettings = new();

        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.Init, OnInit, this));
        }

        private static void OnInit(Action onDone)
        {
            GamePrefabWrapperCreator.OnGamePrefabWrapperCreated += gamePrefabWrapper =>
            {
                gamePrefabsCache.Clear();
                gamePrefabWrapper.GetGamePrefabs(gamePrefabsCache);

                if (gamePrefabsCache.Count == 0 || gamePrefabsCache.All(gamePrefab => gamePrefab == null))
                {
                    Debug.LogWarning($"No Valid Game Prefabs found in {gamePrefabWrapper.name}");
                    return;
                }

                gamePrefabGeneralSettings.Clear();
                foreach (var gamePrefab in gamePrefabsCache)
                {
                    if (gamePrefab.TryGetGamePrefabGeneralSetting(out var gamePrefabGeneralSetting))
                    {
                        gamePrefabGeneralSettings.Add(gamePrefabGeneralSetting);
                    }
                }

                if (gamePrefabGeneralSettings.Count == 0)
                {
                    Debug.LogWarning($"No {nameof(GamePrefabGeneralSetting)} Found for " +
                                     $"any of the Game Prefabs in {gamePrefabWrapper.name}");
                    return;
                }

                if (gamePrefabGeneralSettings.Count > 1)
                {
                    Debug.LogWarning($"Multiple {nameof(GamePrefabGeneralSetting)} Found for " +
                                     $"the Game Prefabs in {gamePrefabWrapper.name}. " +
                                     $"This is not recommended.");
                }

                foreach (var gamePrefabGeneralSetting in gamePrefabGeneralSettings)
                {
                    gamePrefabGeneralSetting.AddToInitialGamePrefabProviders(gamePrefabWrapper);
                }
            };
            
            GamePrefabWrapperInitializeUtility.Refresh();
            GamePrefabWrapperInitializeUtility.CreateAutoRegisterGamePrefabs();
            
            onDone();
        }
    }
}
#endif