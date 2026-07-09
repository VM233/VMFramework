using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture
{
    /// <summary>
    /// The <see cref="GamePrefabManager"/> is responsible for managing all <see cref="IGamePrefab"/>s in the game.
    /// Remember the API is not thread-safe.
    /// </summary>
    public static partial class GamePrefabManager
    {
        private static readonly Dictionary<string, IGamePrefab> allGamePrefabsByID = new();

        private static readonly Dictionary<IGamePrefab, string> allIDsByGamePrefab = new();

        private static readonly Dictionary<Type, HashSet<IGamePrefab>> allGamePrefabsByType = new();

        private static readonly Dictionary<string, HashSet<IGamePrefab>> allGamePrefabsByGameTag = new();

        public static IEnumerable<string> AllGamePrefabIDs => allGamePrefabsByID.Keys;

        public delegate void GamePrefabModificationEvent(IGamePrefab gamePrefab);

        public static event GamePrefabModificationEvent OnGamePrefabRegisteredEvent;

        public static event GamePrefabModificationEvent OnGamePrefabUnregisteredEvent;

        #region Register & Unregister

        public static bool RegisterGamePrefab(IGamePrefab gamePrefab)
        {
            if (gamePrefab == null)
            {
                return false;
            }

            if (gamePrefab.id.IsNullOrEmpty())
            {
                Debug.LogError($"The registered {gamePrefab.GetType().Name} has no ID!");
                return false;
            }

            if (allGamePrefabsByID.TryAdd(gamePrefab.id, gamePrefab) == false)
            {
                Debug.LogWarning($"The ID : {gamePrefab.id} of the registered {gamePrefab.GetType().Name} " +
                                 $"is already registered for another {nameof(IGamePrefab)}!");
                return false;
            }

            allIDsByGamePrefab.Add(gamePrefab, gamePrefab.id);

            var gamePrefabType = gamePrefab.GetType();

            if (allGamePrefabsByType.TryGetValue(gamePrefabType, out var gamePrefabsByType) == false)
            {
                gamePrefabsByType = new();
                allGamePrefabsByType.Add(gamePrefabType, gamePrefabsByType);
            }

            gamePrefabsByType.Add(gamePrefab);

            if (gamePrefab.GameTags != null)
            {
                foreach (var gameTag in gamePrefab.GameTags)
                {
                    if (allGamePrefabsByGameTag.TryGetValue(gameTag, out var gamePrefabsByGameTag) == false)
                    {
                        gamePrefabsByGameTag = new();
                        allGamePrefabsByGameTag.Add(gameTag, gamePrefabsByGameTag);
                    }

                    gamePrefabsByGameTag.Add(gamePrefab);
                }
            }

            OnGamePrefabRegisteredEvent?.Invoke(gamePrefab);

            return true;
        }

        public static bool UnregisterGamePrefab(string id)
        {
            if (id.IsNullOrEmpty())
            {
                return false;
            }

            if (allGamePrefabsByID.Remove(id, out IGamePrefab gamePrefab) == false)
            {
                return false;
            }

            allIDsByGamePrefab.Remove(gamePrefab);

            var gamePrefabType = gamePrefab.GetType();

            if (allGamePrefabsByType.TryGetValue(gamePrefabType, out var gamePrefabsByType) == false)
            {
                Debug.LogWarning($"Type {gamePrefabType} does not have any registered {nameof(IGamePrefab)}s!");
            }
            else
            {
                if (gamePrefabsByType.Remove(gamePrefab) == false)
                {
                    Debug.LogWarning(
                        $"The {nameof(IGamePrefab)} {gamePrefab} is not registered for type {gamePrefabType}!");
                }

                if (gamePrefabsByType.Count == 0)
                {
                    allGamePrefabsByType.Remove(gamePrefabType);
                }
            }

            if (gamePrefab.GameTags != null)
            {
                foreach (var gameTag in gamePrefab.GameTags)
                {
                    if (allGamePrefabsByGameTag.TryGetValue(gameTag, out var gamePrefabsByGameTag))
                    {
                        gamePrefabsByGameTag.Remove(gamePrefab);
                    }
                }
            }

            OnGamePrefabUnregisteredEvent?.Invoke(gamePrefab);

            return true;
        }

        public static bool UnregisterGamePrefab(IGamePrefab gamePrefab)
        {
            if (gamePrefab == null)
            {
                return false;
            }

            if (allIDsByGamePrefab.TryGetValue(gamePrefab, out var id))
            {
                return UnregisterGamePrefab(id);
            }

            return false;
        }

        #endregion

        #region Clear

        public static void Clear()
        {
            foreach (var id in allGamePrefabsByID.Keys.ToList())
            {
                UnregisterGamePrefab(id);
            }
        }

        #endregion
    }
}