using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class DebugStringUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string FormatDebugNameValue<TValue>(this string name, TValue value)
        {
            return name == null ? value.ToString() : $"{name}: {value}";
        }
    }
}