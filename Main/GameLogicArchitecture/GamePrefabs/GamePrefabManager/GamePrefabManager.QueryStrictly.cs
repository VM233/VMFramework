using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GamePrefabManager
    {
        #region Get Game Prefab

        /// <summary>
        /// 通过ID获得<see cref="IGamePrefab"/>，如果没有找到则会报错
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNull]
        public static IGamePrefab GetGamePrefabStrictly(string id)
        {
            id.AssertIsNotNullOrWhiteSpace(nameof(id));

            if (allGamePrefabsByID.TryGetValue(id, out var prefab))
            {
                return prefab;
            }

            throw new ArgumentException($"There is no {nameof(IGamePrefab)} with id:{id}");
        }

        /// <summary>
        /// 通过ID获得特定类型的<see cref="IGamePrefab"/>，如果没有找到则会报错
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetGamePrefabStrictly<T>(string id) where T : IGamePrefab
        {
            var prefab = GetGamePrefabStrictly(id);

            if (prefab is T typedGamePrefab)
            {
                return typedGamePrefab;
            }

            throw new ArgumentException($"The {nameof(IGamePrefab)} with id {id} is not of type {typeof(T)}");
        }

        #endregion

        #region Get Active Game Prefab

        /// <summary>
        /// 通过ID获得激活的<see cref="IGamePrefab"/>，如果没有找到则会报错
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IGamePrefab GetActiveGamePrefabStrictly(string id)
        {
            var prefab = GetGamePrefabStrictly(id);

            if (prefab.IsActive == false)
            {
                throw new ArgumentException($"The {nameof(IGamePrefab)} with id {id} is not active");
            }
            
            return prefab;
        }

        /// <summary>
        /// 通过ID获得激活的特定类型的<see cref="IGamePrefab"/>，如果没有找到则会报错
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetActiveGamePrefabStrictly<T>(string id) where T : IGamePrefab
        {
            var prefab = GetActiveGamePrefabStrictly(id);

            if (prefab is T typedPrefab)
            {
                return typedPrefab;
            }

            throw new ArgumentException($"The {nameof(IGamePrefab)} with id {id} is not of type {typeof(T)}");
        }

        #endregion
    }
}