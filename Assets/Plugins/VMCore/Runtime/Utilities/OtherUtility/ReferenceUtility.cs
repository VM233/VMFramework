using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class ReferenceUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Exchange<T>(ref T reference, T newValue, out T oldValue)
        {
            oldValue = reference;
            reference = newValue;
        }
    }
}