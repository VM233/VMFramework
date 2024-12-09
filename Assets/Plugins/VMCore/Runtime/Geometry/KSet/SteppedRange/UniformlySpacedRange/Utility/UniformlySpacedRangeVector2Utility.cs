using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class UniformlySpacedRangeVector2Utility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniformlySpacedRangeVector2 GetUniformlySpacedRange(this Vector2 start, Vector2 end, int count)
        {
            return new UniformlySpacedRangeVector2(start, end, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniformlySpacedRangeVector2 GetUniformlySpacedRange<TMinMaxOwner>(this TMinMaxOwner minMaxOwner,
            int count)
            where TMinMaxOwner : IMinMaxOwner<Vector2>
        {
            return new UniformlySpacedRangeVector2(minMaxOwner.Min, minMaxOwner.Max, count);
        }
    }
}