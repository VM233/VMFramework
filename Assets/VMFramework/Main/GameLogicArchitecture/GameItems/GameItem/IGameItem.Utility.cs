using System;
using System.Runtime.CompilerServices;

namespace VMFramework.GameLogicArchitecture
{
    public partial interface IGameItem
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type GetGameItemType(string id)
        {
            if (GamePrefabManager.TryGetGamePrefab(id, out var gamePrefab))
            {
                return gamePrefab.GameItemType;
            }
            
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetGameItemType(string id, out Type gameItemType)
        {
            if (GamePrefabManager.TryGetGamePrefab(id, out var gamePrefab))
            {
                gameItemType = gamePrefab.GameItemType;
                return true;
            }
            
            gameItemType = null;
            return false;
        }
    }
}