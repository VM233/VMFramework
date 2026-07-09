using System.Runtime.CompilerServices;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Maps
{
    public static class GamePrefabFillableMapUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool FillTile<TPoint, TGamePrefab>(this ITileFillableMap<TPoint, TGamePrefab> map,
            TPoint position, string id)
            where TGamePrefab : IGamePrefab
        {
            var gamePrefab = GamePrefabManager.GetGamePrefabStrictly<TGamePrefab>(id);
            return map.FillTile(position, gamePrefab);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FillRectangleTiles<TGamePrefab>(this ITilesRectangleFillableGridMap<TGamePrefab> map,
            RectangleInteger rectangle, string id)
            where TGamePrefab : IGamePrefab
        {
            var gamePrefab = GamePrefabManager.GetGamePrefabStrictly<TGamePrefab>(id);
            map.FillRectangleTiles(rectangle, gamePrefab);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FillCubeTiles<TGamePrefab>(this ITilesCubeFillableGridMap<TGamePrefab> map,
            CubeInteger cube, string id)
            where TGamePrefab : IGamePrefab
        {
            var gamePrefab = GamePrefabManager.GetGamePrefabStrictly<TGamePrefab>(id);
            map.FillCubeTiles(cube, gamePrefab);
        }
    }
}