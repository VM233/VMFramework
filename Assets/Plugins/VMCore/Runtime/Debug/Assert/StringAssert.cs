using System;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class StringAssert
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertIsNotNullOrEmpty(this string str, string name)
        {
            if (str == null)
            {
                throw new ArgumentNullException(name);
            }
            
            if (str.IsEmpty())
            {
                throw new ArgumentException($"{name} is empty");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertIsNotNullOrWhiteSpace(this string str, string name)
        {
            if (str == null)
            {
                throw new ArgumentNullException(name);
            }
            
            if (str.IsWhiteSpace())
            {
                throw new ArgumentException($"{name} is white space");
            }
        }
    }
}