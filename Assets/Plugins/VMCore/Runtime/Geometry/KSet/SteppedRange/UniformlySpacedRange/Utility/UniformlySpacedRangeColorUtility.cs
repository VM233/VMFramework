using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class UniformlySpacedRangeColorUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniformlySpacedRangeColor GetUniformlySpacedRange(this Color start, Color end, int count)
        {
            return new UniformlySpacedRangeColor(start, end, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniformlySpacedRangeColor GetUniformlySpacedRange<TMinMaxOwner>(this TMinMaxOwner minMaxOwner,
            int count)
            where TMinMaxOwner : IMinMaxOwner<Color>
        {
            return new UniformlySpacedRangeColor(minMaxOwner.Min, minMaxOwner.Max, count);
        }
    }
}