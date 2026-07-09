using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using VMFramework.Core;
using VMFramework.Core.Pools;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GamePrefabManager
    {
        #region Get Game Prefab By Game Tag

        /// <summary>
        /// 通过<see cref="GameTag"/>获取<see cref="IGamePrefab"/>
        /// </summary>
        /// <param name="gameTagID"></param>
        /// <returns></returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyCollection<IGamePrefab> GetGamePrefabsByGameTag(string gameTagID)
        {
            if (allGamePrefabsByGameTag.TryGetValue(gameTagID, out var gamePrefabs))
            {
                return gamePrefabs;
            }

            return Array.Empty<IGamePrefab>();
        }

        /// <summary>
        /// 通过<see cref="GameTag"/>获取特定类型的<see cref="IGamePrefab"/>
        /// </summary>
        /// <param name="gameTagID"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> GetGamePrefabsByGameTag<T>(string gameTagID)
            where T : IGamePrefab
        {
            if (allGamePrefabsByGameTag.TryGetValue(gameTagID, out var gamePrefabs))
            {
                foreach (var gamePrefab in gamePrefabs)
                {
                    if (gamePrefab is T typedPrefab)
                    {
                        yield return typedPrefab;
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetGamePrefabsByAllGameTags<TGamePrefab>(IReadOnlyCollection<string> gameTagIDs,
            [DisallowNull] ICollection<TGamePrefab> resultGamePrefabs)
            where TGamePrefab : IGamePrefab
        {
            var result = HashSetPool<IGamePrefab>.Default.Get();
            result.Clear();

            foreach (var gameTagID in gameTagIDs)
            {
                if (allGamePrefabsByGameTag.TryGetValue(gameTagID, out var gamePrefabs))
                {
                    if (result.Count <= 0)
                    {
                        result.UnionWith(gamePrefabs);
                    }
                    else
                    {
                        result.IntersectWith(gamePrefabs);
                    }
                }
            }

            foreach (var gamePrefab in result)
            {
                if (gamePrefab is TGamePrefab typedPrefab)
                {
                    resultGamePrefabs.Add(typedPrefab);
                }
            }

            result.ReturnToDefaultPool();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetGamePrefabsByAnyGameTags<TGamePrefab>(IReadOnlyCollection<string> gameTagIDs,
            [DisallowNull] ICollection<TGamePrefab> resultGamePrefabs)
            where TGamePrefab : IGamePrefab
        {
            var result = HashSetPool<IGamePrefab>.Default.Get();
            result.Clear();

            foreach (var gameTagID in gameTagIDs)
            {
                if (allGamePrefabsByGameTag.TryGetValue(gameTagID, out var gamePrefabs))
                {
                    result.UnionWith(gamePrefabs);
                }
            }

            foreach (var gamePrefab in result)
            {
                if (gamePrefab is TGamePrefab typedPrefab)
                {
                    resultGamePrefabs.Add(typedPrefab);
                }
            }

            result.ReturnToDefaultPool();
        }

        #endregion

        #region Get Random Game Prefab By Game Tag

        /// <summary>
        /// 通过<see cref="GameTag"/>获取随机的<see cref="IGamePrefab"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IGamePrefab GetRandomGamePrefabByGameTag(string gameTagID, Random random)
        {
            return random.ChooseOrDefault(GetGamePrefabsByGameTag(gameTagID));
        }

        /// <summary>
        ///  通过<see cref="GameTag"/>获取随机的特定类型的<see cref="IGamePrefab"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetRandomGamePrefabByGameTag<T>(string gameTagID, Random random)
            where T : IGamePrefab
        {
            return random.ChooseOrDefault(GetGamePrefabsByGameTag<T>(gameTagID));
        }

        #endregion

        #region Get IDs By Game Tag

        /// <summary>
        /// 获取特定<see cref="GameTag"/>的ID列表
        /// </summary>
        /// <param name="gameTagID"></param>
        /// <returns></returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<string> GetIDsByGameTag(string gameTagID)
        {
            if (allGamePrefabsByGameTag.TryGetValue(gameTagID, out var gamePrefabs))
            {
                return gamePrefabs.Select(p => p.id);
            }

            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// 获取特定<see cref="GameTag"/>的ID列表
        /// </summary>
        /// <param name="gameTagID"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<string> GetIDsByGameTag<T>(string gameTagID)
            where T : IGamePrefab
        {
            if (allGamePrefabsByGameTag.TryGetValue(gameTagID, out var gamePrefabs))
            {
                return gamePrefabs.Where(p => p is T).Select(p => p.id);
            }

            return Enumerable.Empty<string>();
        }

        #endregion

        #region Get Random ID By Game Tag

        /// <summary>
        /// 获取特定<see cref="GameTag"/>的随机ID
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetRandomIDByGameTag(Random random, string gameTagID)
        {
            return random.ChooseOrDefault(GetIDsByGameTag(gameTagID));
        }

        /// <summary>
        /// 获取特定<see cref="GameTag"/>的随机ID
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetRandomIDByGameTag<T>(Random random, string gameTagID)
            where T : IGamePrefab
        {
            return random.ChooseOrDefault(GetIDsByGameTag<T>(gameTagID));
        }

        #endregion

        #region Contains Game Tag

        /// <summary>
        /// 所有的<see cref="IGamePrefab"/>里是否包含特定<see cref="GameTag"/>
        /// </summary>
        /// <param name="gameTagID"></param>
        /// <returns></returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ContainsGameTag(string gameTagID)
        {
            return allGamePrefabsByGameTag.ContainsKey(gameTagID);
        }

        #endregion
    }
}