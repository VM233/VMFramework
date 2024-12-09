using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GamePrefabManager
    {
        #region Try Get Game Prefab

        /// <summary>
        /// Try to get a <see cref="IGamePrefab"/> by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="targetPrefab"></param>
        /// <returns></returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetGamePrefabWithWarning(string id, out IGamePrefab targetPrefab)
        {
            if (id.CheckIsNull(nameof(id)) == false)
            {
                targetPrefab = default;
                return false;
            }

            if (allGamePrefabsByID.TryGetValue(id, out targetPrefab))
            {
                return true;
            }

            Debugger.LogWarning($"GamePrefab with ID {id} not found.");
            return false;
        }

        /// <summary>
        /// Try to get a specific type of <see cref="IGamePrefab"/> by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="targetPrefab"></param>
        /// <typeparam name="TGamePrefab"></typeparam>
        /// <returns></returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetGamePrefabWithWarning<TGamePrefab>(string id, out TGamePrefab targetPrefab)
            where TGamePrefab : IGamePrefab
        {
            if (id.CheckIsNull(nameof(id)) == false)
            {
                targetPrefab = default;
                return false;
            }

            if (allGamePrefabsByID.TryGetValue(id, out var prefab) == false)
            {
                Debug.LogWarning($"GamePrefab with ID {id} not found.");
                targetPrefab = default;
                return false;
            }

            if (prefab is TGamePrefab typedPrefab)
            {
                targetPrefab = typedPrefab;
                return true;
            }

            Debug.LogWarning($"GamePrefab with ID {id} is not of type {typeof(TGamePrefab)}.");
            targetPrefab = default;
            return false;
        }

        #endregion

        #region Contains

        /// <summary>
        /// Checks if a <see cref="IGamePrefab"/> with the given ID exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGamePrefabWithWarning(string id)
        {
            return TryGetGamePrefabWithWarning(id, out _);
        }

        /// <summary>
        /// Checks if a specific type of <see cref="IGamePrefab"/> with the given ID exists.
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="TGamePrefab"></typeparam>
        /// <returns></returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGamePrefabWithWarning<TGamePrefab>(string id)
            where TGamePrefab : IGamePrefab
        {
            return TryGetGamePrefabWithWarning<TGamePrefab>(id, out _);
        }

        #endregion
    }
}