#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;
using VMFramework.Core.Linq;
using VMFramework.Editor;

namespace VMFramework.GameLogicArchitecture.Editor
{
    public static class GamePrefabWrapperInitializeUtility
    {
        public static event Action OnGamePrefabWrappersRefresh;

        private static readonly Action<IGamePrefab, string, string> onGamePrefabIDChangedFunc = OnGamePrefabIDChanged;

        private static readonly List<IGamePrefab> gamePrefabsCache = new();

        private static readonly Dictionary<IGamePrefab, GamePrefabWrapper> gamePrefabWrapperSources = new();
        private static readonly AuthoringAssetPathIndex sourceAssets = new();

        internal static bool OwnsAssetPath(string path) => sourceAssets.ContainsSource(path);
        internal static bool OwnsRemovedAssetPath(string path) => sourceAssets.ContainsSourceOrAncestor(path);

        public static void Refresh()
        {
            foreach (var previous in gamePrefabWrapperSources.Keys)
                previous.OnIDChangedEvent -= onGamePrefabIDChangedFunc;
            gamePrefabWrapperSources.Clear();
            sourceAssets.Clear();
            GamePrefabManager.Clear();

            foreach (var root in GlobalSettingFileEditorManager.GetGlobalSettings())
            {
                sourceAssets.Add((UnityEngine.Object)root);
                foreach (var setting in root.GetAllGeneralSettings())
                {
                    sourceAssets.Add((UnityEngine.Object)setting);
                    if (setting is not GamePrefabGeneralSetting prefabs) continue;
                    prefabs.RefreshInitialGamePrefabProviders();
                    foreach (var provider in prefabs.initialGamePrefabProviders)
                        sourceAssets.Add((UnityEngine.Object)provider);
                }
            }

            foreach (var wrapper in GamePrefabWrapperQueryTools.GetAllGamePrefabWrappers())
            {
                sourceAssets.Add(wrapper);
                gamePrefabsCache.Clear();
                wrapper.GetGamePrefabs(gamePrefabsCache);

                if (gamePrefabsCache.Count == 0)
                {
                    Debug.LogWarning(
                        $"There are no {nameof(IGamePrefab)}s in {wrapper.name}. " +
                        $"Wrapper path: {wrapper.GetAssetPath()}", wrapper);
                    continue;
                }

                if (gamePrefabsCache.IsAnyNull())
                {
                    Debug.LogWarning(
                        $"There are null {nameof(IGamePrefab)}s in {wrapper.name}. " +
                        $"Wrapper path: {wrapper.GetAssetPath()}", wrapper);
                }

                gamePrefabsCache.RemoveAllNull();

                foreach (var gamePrefab in gamePrefabsCache)
                {
                    if (gamePrefab.id.IsNullOrEmpty())
                    {
                        Debug.LogWarning(
                            $"There is a {nameof(IGamePrefab)} with no ID set in {wrapper.name}. " +
                            $"Wrapper path: {wrapper.GetAssetPath()}", wrapper);
                        continue;
                    }

                    if (GamePrefabManager.RegisterGamePrefab(gamePrefab) == false)
                    {
                        GamePrefabManager.TryGetGamePrefab(gamePrefab.id, out var existing);
                        var existedWrapper = gamePrefabWrapperSources[existing];
                        Debug.LogWarning(
                            $"Registering Failed for {gamePrefab.id} in {wrapper.name}." +
                            $"It has already been registered in {existedWrapper.name}", existedWrapper);
                        continue;
                    }

                    gamePrefabWrapperSources.Add(gamePrefab, wrapper);

                    gamePrefab.OnIDChangedEvent += onGamePrefabIDChangedFunc;
                }
            }

            OnGamePrefabWrappersRefresh?.Invoke();
        }

        public static void CreateAutoRegisterGamePrefabs()
        {
            var autoRegisterInfos = GamePrefabAutoRegisterCollector.Collect();
            
            bool newWrappersCreated = false;

            foreach (var info in autoRegisterInfos)
            {
                var id = info.id;
                var gamePrefabType = info.gamePrefabType;

                if (GamePrefabManager.TryGetGamePrefab(id, out var existedGamePrefab))
                {
                    if (existedGamePrefab.GetType() != gamePrefabType)
                    {
                        Debug.LogWarning($"The {nameof(GamePrefab)} with ID {id} already exists, " +
                                         $"but its type is not the same as the one to be created. " +
                                         $"The type to be created is {gamePrefabType} but " +
                                         $"the existing type is {existedGamePrefab.GetType()}");
                    }

                    continue;
                }

                var wrapper =
                    GamePrefabWrapperCreator.CreateGamePrefabWrapper(id, gamePrefabType, GamePrefabWrapperType.Single);

                if (wrapper == null)
                {
                    continue;
                }
                
                gamePrefabsCache.Clear();
                wrapper.GetGamePrefabs(gamePrefabsCache);

                foreach (var gamePrefab in gamePrefabsCache)
                {
                    if (gamePrefab is IGamePrefabAutoRegisterProvider autoRegisterProvider)
                    {
                        autoRegisterProvider.OnGamePrefabAutoRegister();
                    }

                }

                wrapper.EnforceSave();
                
                newWrappersCreated = true;
            }

            if (newWrappersCreated)
            {
                GlobalSettingFileEditorManager.SaveAll();
                Refresh();
            }
        }

        private static void OnGamePrefabIDChanged(IGamePrefab gamePrefab, string oldID, string newID)
        {
            GamePrefabManager.UnregisterGamePrefab(gamePrefab);
            GamePrefabManager.RegisterGamePrefab(gamePrefab);
        }
    }
}

#endif
