using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GamePrefabManager
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetGamePrefabName(string id)
        {
            var gamePrefab = GetGamePrefab(id);
            
            return gamePrefab?.Name;
        }

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetGamePrefabNameWithWarning(string id)
        {
            if (TryGetGamePrefabWithWarning(id, out var gamePrefab))
            {
                return gamePrefab.Name;
            }
            
            return null;
        }
    }
}