#if UNITY_EDITOR
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;
using VMFramework.Core.Linq;
using VMFramework.Core.Pools;
using VMFramework.Localization;

namespace VMFramework.GameLogicArchitecture.Editor
{
    public static class GamePrefabWrapperCreator
    {
        public static event Action<GamePrefabWrapper> OnGamePrefabWrapperCreated;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IGamePrefab CreateDefaultGamePrefab(string id, Type gamePrefabType)
        {
            if (gamePrefabType == null)
            {
                throw new ArgumentNullException(nameof(gamePrefabType));
            }

            if (gamePrefabType.TryCreateInstance() is not IGamePrefab gamePrefab)
            {
                throw new Exception($"Could not create instance of {gamePrefabType.Name}.");
            }

            gamePrefab.id = id;

            return gamePrefab;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GamePrefabWrapper CreateGamePrefabWrapper(string id, Type gamePrefabType,
            GamePrefabWrapperType wrapperType)
        {
            var gamePrefab = CreateDefaultGamePrefab(id, gamePrefabType);

            return CreateGamePrefabWrapper(gamePrefab, wrapperType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GamePrefabWrapper CreateGamePrefabWrapper(IGamePrefab gamePrefab,
            GamePrefabWrapperType wrapperType)
        {
            if (gamePrefab.TryGetGamePrefabGeneralSettingWithWarning(out var gamePrefabSetting) == false)
            {
                return null;
            }

            string path = gamePrefabSetting.GamePrefabFolderPath;

            if (path.EndsWith("/") == false)
            {
                path += "/";
            }

            path += gamePrefab.id.ToPascalCase();

            return CreateGamePrefabWrapper(path, wrapperType, gamePrefab);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GamePrefabWrapper CreateGamePrefabWrapper(string wrapperName, params IGamePrefab[] gamePrefabs)
        {
            GamePrefabGeneralSetting gamePrefabSetting = null;

            foreach (var gamePrefab in gamePrefabs)
            {
                if (gamePrefab.TryGetGamePrefabGeneralSettingWithWarning(out var newGamePrefabSetting) == false)
                {
                    return null;
                }

                if (gamePrefabSetting == null)
                {
                    gamePrefabSetting = newGamePrefabSetting;
                    continue;
                }

                if (gamePrefabSetting != newGamePrefabSetting)
                {
                    throw new InvalidOperationException($"Cannot create a {nameof(GamePrefabWrapper)} " +
                                                        $"with multiple {nameof(GamePrefabGeneralSetting)}s.");
                }
            }

            if (gamePrefabSetting == null)
            {
                return null;
            }
            
            string path = gamePrefabSetting.GamePrefabFolderPath;

            if (path.EndsWith("/") == false)
            {
                path += "/";
            }

            path += wrapperName;

            return CreateGamePrefabWrapper(path, GamePrefabWrapperType.Multiple, gamePrefabs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GamePrefabWrapper CreateGamePrefabWrapper(string path, string id, Type gamePrefabType,
            GamePrefabWrapperType wrapperType)
        {
            var gamePrefab = CreateDefaultGamePrefab(id, gamePrefabType);

            return CreateGamePrefabWrapper(path, wrapperType, gamePrefab);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GamePrefabWrapper CreateGamePrefabWrapper(string path,
            GamePrefabWrapperType wrapperType, params IGamePrefab[] gamePrefabs)
        {
            if (gamePrefabs.Length > 1 && wrapperType == GamePrefabWrapperType.Single)
            {
                throw new InvalidOperationException(
                    $"Cannot create a {nameof(GamePrefabWrapper)} with multiple game prefabs " +
                    $"in {nameof(GamePrefabWrapperType.Single)} mode.");
            }
            
            foreach (var gamePrefab in gamePrefabs)
            {
                gamePrefab.id.AssertIsNotNull(nameof(gamePrefab.id));

                if (gamePrefab.id.IsWhiteSpace())
                {
                    throw new ArgumentException($"{nameof(gamePrefab.id)} ID cannot be empty or whitespace.");
                }

                if (gamePrefab is ILocalizedStringOwnerConfig localizedStringOwner)
                {
                    localizedStringOwner.AutoConfigureLocalizedString(default);
                }
            }

            return wrapperType switch
            {
                GamePrefabWrapperType.Single => CreateGamePrefabWrapper<GamePrefabSingleWrapper>(path,
                    gamePrefabs),
                GamePrefabWrapperType.Multiple => CreateGamePrefabWrapper<GamePrefabMultipleWrapper>(path,
                    gamePrefabs),
                _ => throw new ArgumentOutOfRangeException(nameof(wrapperType), wrapperType, null)
            };
        }

        private static TWrapper CreateGamePrefabWrapper<TWrapper>(string path,
            params IGamePrefab[] gamePrefabs)
            where TWrapper : GamePrefabWrapper
        {
            if (path.EndsWith(".asset") == false)
            {
                path += ".asset";
            }

            AssetDatabase.Refresh();

            TWrapper gamePrefabWrapper = null;

            if (path.ExistsAsset())
            {
                gamePrefabWrapper = AssetDatabase.LoadAssetAtPath<TWrapper>(path);

                if (gamePrefabWrapper == null)
                {
                    Debugger.LogWarning($"Asset already exists at {path}.");
                    return null;
                }
                
                var existingGamePrefabs = ListPool<IGamePrefab>.Default.Get();
                existingGamePrefabs.Clear();
                gamePrefabWrapper.GetGamePrefabs(existingGamePrefabs);

                if (existingGamePrefabs.Count != 0 && existingGamePrefabs.IsAllNull() == false)
                {
                    Debugger.LogWarning($"{typeof(TWrapper).Name} already exists at {path} " +
                                        $"with {existingGamePrefabs.Count} game prefabs.");
                    
                    existingGamePrefabs.ReturnToDefaultPool();
                    return null;
                }
                
                existingGamePrefabs.ReturnToDefaultPool();
            }

            path.CreateFolderByAssetPath();

            if (gamePrefabs.Length == 0)
            {
                Debugger.LogError($"No {nameof(IGamePrefab)} provided to create {typeof(TWrapper).Name}.");
                return null;
            }

            if (gamePrefabs.IsAllNull())
            {
                Debugger.LogError(
                    $"All {nameof(IGamePrefab)} provided to create {typeof(TWrapper).Name} are null.");
                return null;
            }

            if (gamePrefabWrapper == null)
            {
                gamePrefabWrapper = path.CreateScriptableObjectAsset<TWrapper>();

                if (gamePrefabWrapper == null)
                {
                    Debugger.LogError($"Could not create {typeof(TWrapper).Name} " +
                                   $"of {gamePrefabs.Join(", ")} on Path : {path}");
                    return null;
                }
            }

            gamePrefabWrapper.InitGamePrefabs(gamePrefabs);
            
            // var gamePrefabsCache = ListPool<IGamePrefab>.Default.Get();
            // gamePrefabsCache.Clear();
            // gamePrefabWrapper.GetGamePrefabs(gamePrefabsCache);
            //
            // foreach (var gamePrefab in gamePrefabsCache)
            // {
            //     if (gamePrefab.TryGetGamePrefabGeneralSettingWithWarning(out var gamePrefabSetting))
            //     {
            //         gamePrefabSetting.AddDefaultGameTypeToGamePrefabWrapper(gamePrefabWrapper);
            //     }
            // }
            //
            // gamePrefabsCache.ReturnToDefaultPool();

            EditorApplication.delayCall += () =>
            {
                gamePrefabWrapper.EnforceSave();
                OnGamePrefabWrapperCreated?.Invoke(gamePrefabWrapper);
            };
            
            return gamePrefabWrapper;
        }
    }
}
#endif