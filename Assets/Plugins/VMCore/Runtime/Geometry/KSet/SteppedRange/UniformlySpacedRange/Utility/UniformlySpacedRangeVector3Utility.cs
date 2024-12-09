using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class UniformlySpacedRangeVector3Utility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniformlySpacedRangeVector3 GetUniformlySpacedRange(this Vector3 start, Vector3 end, int count)
        {
            return new UniformlySpacedRangeVector3(start, end, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniformlySpacedRangeVector3 GetUniformlySpacedRange<TMinMaxOwner>(this TMinMaxOwner minMaxOwner,
            int count)
            where TMinMaxOwner : IMinMaxOwner<Vector3>
        {
            return new UniformlySpacedRangeVector3(minMaxOwner.Min, minMaxOwner.Max, count);
        }
    }
}