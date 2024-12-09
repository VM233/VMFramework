using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class UniformlySpacedRangeVector4Utility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniformlySpacedRangeVector4 GetUniformlySpacedRange(this Vector4 start, Vector4 end, int count)
        {
            return new UniformlySpacedRangeVector4(start, end, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniformlySpacedRangeVector4 GetUniformlySpacedRange<TMinMaxOwner>(this TMinMaxOwner minMaxOwner,
            int count)
            where TMinMaxOwner : IMinMaxOwner<Vector4>
        {
            return new UniformlySpacedRangeVector4(minMaxOwner.Min, minMaxOwner.Max, count);
        }
    }
}