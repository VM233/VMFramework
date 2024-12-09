#if UNITY_EDITOR
using System.Runtime.CompilerServices;

namespace VMFramework.Editor
{
    public static class GameItemBaseTypeUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetName(this GameItemBaseType type)
        {
            return type.ToString();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetInterfaceName(this GameItemBaseType type)
        {
            return type switch
            {
                GameItemBaseType.GameItem => "IGameItem",
                GameItemBaseType.ControllerGameItem => "IGameItem",
                GameItemBaseType.VisualGameItem => "IVisualGameItem",
#if FISHNET
                GameItemBaseType.UUIDGameItem => "IGameItem",
                GameItemBaseType.UUIDVisualGameItem => "IVisualGameItem",
#endif
                _ => "IGameItem"
            };
        }
    }
}
#endif