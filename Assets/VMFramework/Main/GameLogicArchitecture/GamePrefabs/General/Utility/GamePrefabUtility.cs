using System;
using System.Runtime.CompilerServices;

namespace VMFramework.GameLogicArchitecture
{
    public static class GamePrefabUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasGameItem(this Type type)
        {
            if (EmptyGamePrefabs.TryGet(type, out var gamePrefab) == false)
            {
                return false;
            }

            return gamePrefab.GameItemType != null;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type GetGameItemType(this Type type)
        {
            return EmptyGamePrefabs.Get(type)?.GameItemType;
        }
    }
}