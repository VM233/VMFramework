using System;
using System.Runtime.CompilerServices;
using Vector = UnityEngine.Vector2;
using Number = System.Single;

namespace VMFramework.Core
{
    public static class Vector2Assert
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertIsAnyNumberAbove(this Vector vector, Number comparison, string vectorName,
            string comparisonName = null)
        {
            if (vector.AnyNumberAbove(comparison) == false)
            {
                comparisonName ??= comparison.ToString();
                throw new ArgumentOutOfRangeException(
                    $"{vectorName} is all number below or equal to {comparisonName}");
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertIsAllNumberAbove(this Vector vector, Number comparison, string vectorName,
            string comparisonName = null)
        {
            if (vector.AllNumberAbove(comparison) == false)
            {
                comparisonName ??= comparison.ToString();
                throw new ArgumentOutOfRangeException(
                    $"{vectorName} is not all number above {comparisonName}");
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertIsAnyNumberBelow(this Vector vector, Number comparison, string vectorName,
            string comparisonName = null)
        {
            if (vector.AnyNumberBelow(comparison) == false)
            {
                comparisonName ??= comparison.ToString();
                throw new ArgumentOutOfRangeException(
                    $"{vectorName} is all number above or equal to {comparisonName}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertIsAllNumberBelow(this Vector vector, Number comparison, string vectorName,
            string comparisonName = null)
        {
            if (vector.AllNumberBelow(comparison) == false)
            {
                comparisonName ??= comparison.ToString();
                throw new ArgumentOutOfRangeException($"{vectorName} is not all number below {comparisonName}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertIsAnyNumberAboveOrEqual(this Vector vector, Number comparison, string vectorName,
            string comparisonName = null)
        {
            if (vector.AnyNumberAboveOrEqual(comparison) == false)
            {
                comparisonName ??= comparison.ToString();
                throw new ArgumentOutOfRangeException(
                    $"{vectorName} is all number below {comparisonName}");
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertIsAllNumberAboveOrEqual(this Vector vector, Number comparison, string vectorName,
            string comparisonName = null)
        {
            if (vector.AllNumberAboveOrEqual(comparison) == false)
            {
                comparisonName ??= comparison.ToString();
                throw new ArgumentOutOfRangeException(
                    $"{vectorName} is not all number above or equal to {comparisonName}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertIsAnyNumberBelowOrEqual(this Vector vector, Number comparison, string vectorName,
            string comparisonName = null)
        {
            if (vector.AnyNumberBelowOrEqual(comparison) == false)
            {
                comparisonName ??= comparison.ToString();
                throw new ArgumentOutOfRangeException($"{vectorName} is all number above {comparisonName}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertIsAllNumberBelowOrEqual(this Vector vector, Number comparison, string vectorName,
            string comparisonName = null)
        {
            if (vector.AllNumberBelowOrEqual(comparison) == false)
            {
                comparisonName ??= comparison.ToString();
                throw new ArgumentOutOfRangeException(
                    $"{vectorName} is not all number below or equal to {comparisonName}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertIsAnyNumberAbove(this Vector vector, Vector comparison,
            string vectorName, string comparisonName = null)
        {
            if (vector.AnyNumberAbove(comparison) == false)
            {
                comparisonName ??= comparison.ToString();
                throw new ArgumentOutOfRangeException(
                    $"{vectorName} is all number below or equal to {comparisonName}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertIsAllNumberAbove(this Vector vector, Vector comparison,
            string vectorName, string comparisonName = null)
        {
            if (vector.AllNumberAbove(comparison) == false)
            {
                comparisonName ??= comparison.ToString();
                throw new ArgumentOutOfRangeException(
                    $"{vectorName} is not all number above {comparisonName}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertIsAnyNumberBelow(this Vector vector, Vector comparison,
            string vectorName, string comparisonName = null)
        {
            if (vector.AnyNumberBelow(comparison) == false)
            {
                comparisonName ??= comparison.ToString();
                throw new ArgumentOutOfRangeException(
                    $"{vectorName} is all number above or equal to {comparisonName}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertIsAllNumberBelow(this Vector vector, Vector comparison,
            string vectorName, string comparisonName = null)
        {
            if (vector.AllNumberBelow(comparison) == false)
            {
                comparisonName ??= comparison.ToString();
                throw new ArgumentOutOfRangeException(
                    $"{vectorName} is not all number below {comparisonName}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertIsAnyNumberAboveOrEqual(this Vector vector, Vector comparison,
            string vectorName, string comparisonName = null)
        {
            if (vector.AnyNumberAboveOrEqual(comparison) == false)
            {
                comparisonName ??= comparison.ToString();
                throw new ArgumentOutOfRangeException(
                    $"{vectorName} is all number below {comparisonName}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertIsAllNumberAboveOrEqual(this Vector vector, Vector comparison,
            string vectorName, string comparisonName = null)
        {
            if (vector.AllNumberAboveOrEqual(comparison) == false)
            {
                comparisonName ??= comparison.ToString();
                throw new ArgumentOutOfRangeException(
                    $"{vectorName} is not all number above or equal to {comparisonName}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertIsAnyNumberBelowOrEqual(this Vector vector, Vector comparison,
            string vectorName, string comparisonName = null)
        {
            if (vector.AnyNumberBelowOrEqual(comparison) == false)
            {
                comparisonName ??= comparison.ToString();
                throw new ArgumentOutOfRangeException(
                    $"{vectorName} is all number above {comparisonName}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertIsAllNumberBelowOrEqual(this Vector vector, Vector comparison,
            string vectorName, string comparisonName = null)
        {
            if (vector.AllNumberBelowOrEqual(comparison) == false)
            {
                comparisonName ??= comparison.ToString();
                throw new ArgumentOutOfRangeException(
                    $"{vectorName} is not all number below or equal to {comparisonName}");
            }
        }
    }
}