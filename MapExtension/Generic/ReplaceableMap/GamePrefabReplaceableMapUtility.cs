using System.Runtime.CompilerServices;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Maps
{
    public static class GamePrefabReplaceableMapUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceTile<TPoint, TGamePrefab>(this ITileReplaceableMap<TPoint, TGamePrefab> map,
            TPoint position, string id)
            where TGamePrefab : IGamePrefab
        {
            var gamePrefab = GamePrefabManager.GetGamePrefabStrictly<TGamePrefab>(id);
            map.ReplaceTile(position, gamePrefab);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceRectangleTiles<TGamePrefab>(this ITilesRectangleReplaceableGridMap<TGamePrefab> map,
            RectangleInteger rectangle, string id)
            where TGamePrefab : IGamePrefab
        {
            var gamePrefab = GamePrefabManager.GetGamePrefabStrictly<TGamePrefab>(id);
            map.ReplaceRectangleTiles(rectangle, gamePrefab);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReplaceCubeTiles<TGamePrefab>(this ITilesCubeReplaceableGridMap<TGamePrefab> map,
            CubeInteger cube, string id)
            where TGamePrefab : IGamePrefab
        {
            var gamePrefab = GamePrefabManager.GetGamePrefabStrictly<TGamePrefab>(id);
            map.ReplaceCubeTiles(cube, gamePrefab);
        }
    }
}