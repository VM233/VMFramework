using System;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class ComparableAssert
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertIsAbove<T>(this T comparable, T value, string comparableName,
            string valueName = null) where T : IComparable<T>
        {
            if (comparable.Above(value) == false)
            {
                valueName ??= value.ToString();
                throw new ArgumentOutOfRangeException($"{comparableName} is not above {valueName}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertIsAboveOrEqual<T>(this T comparable, T value, string comparableName,
            string valueName = null) where T : IComparable<T>
        {
            if (comparable.AboveOrEqual(value) == false)
            {
                valueName ??= value.ToString();
                throw new ArgumentOutOfRangeException(
                    $"{comparableName} is not above or equal to {valueName}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertIsBelow<T>(this T comparable, T value, string comparableName,
            string valueName = null) where T : IComparable<T>
        {
            if (comparable.Below(value) == false)
            {
                valueName ??= value.ToString();
                throw new ArgumentOutOfRangeException($"{comparableName} is not below {valueName}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertIsBelowOrEqual<T>(this T comparable, T value, string comparableName,
            string valueName = null) where T : IComparable<T>
        {
            if (comparable.BelowOrEqual(value) == false)
            {
                valueName ??= value.ToString();
                throw new ArgumentOutOfRangeException(
                    $"{comparableName} is not below or equal to {valueName}");
            }
        }
    }
}