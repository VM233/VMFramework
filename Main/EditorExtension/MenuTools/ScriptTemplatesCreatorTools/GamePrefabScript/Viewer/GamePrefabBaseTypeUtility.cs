#if UNITY_EDITOR
using System.Runtime.CompilerServices;

namespace VMFramework.Editor
{
    public static class GamePrefabBaseTypeUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetName(this GamePrefabBaseType type)
        {
            return type.ToString();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetInterfaceName(this GamePrefabBaseType type)
        {
            return "I" + type;
        }
    }
}
#endif