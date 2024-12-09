using System;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class ReferenceAssert
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertIsNotNull<T>(this T obj, string name) where T : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertIsNull<T>(this T obj, string name) where T : class
        {
            if (obj != null)
            {
                throw new ArgumentException($"{name} is not null");
            }
        }
    }
}