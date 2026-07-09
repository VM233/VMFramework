using System;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class WarnUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WarnIfNull<T>(this T obj, string name)
        {
            if (obj == null)
            {
                UnityEngine.Debug.LogWarning($"{name} is null.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WarnIfNullOrEmpty(this string str, string name)
        {
            if (string.IsNullOrEmpty(str))
            {
                UnityEngine.Debug.LogWarning($"{name} is null or empty.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WarnIfAbove<TComparable>(this TComparable comparable, TComparable threshold, string name,
            string thresholdName = null)
            where TComparable : IComparable<TComparable>
        {
            if (comparable.Above(threshold))
            {
                UnityEngine.Debug.LogWarning($"{name}: {comparable} is above {thresholdName.FormatDebugNameValue(threshold)}.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WarnIfBelow<TComparable>(this TComparable comparable, TComparable threshold, string name,
            string thresholdName = null)
            where TComparable : IComparable<TComparable>
        {
            if (comparable.Below(threshold))
            {
                UnityEngine.Debug.LogWarning($"{name}: {comparable} is below {thresholdName.FormatDebugNameValue(threshold)}.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WarnIfAboveOrEqual<TComparable>(this TComparable comparable, TComparable threshold,
            string name, string thresholdName = null)
            where TComparable : IComparable<TComparable>
        {
            if (comparable.AboveOrEqual(threshold))
            {
                UnityEngine.Debug.LogWarning(
                    $"{name}: {comparable} is above or equal to {thresholdName.FormatDebugNameValue(threshold)}.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WarnIfBelowOrEqual<TComparable>(this TComparable comparable, TComparable threshold,
            string name, string thresholdName = null)
            where TComparable : IComparable<TComparable>
        {
            if (comparable.BelowOrEqual(threshold))
            {
                UnityEngine.Debug.LogWarning(
                    $"{name}: {comparable} is below or equal to {thresholdName.FormatDebugNameValue(threshold)}.");
            }
        }
    }
}