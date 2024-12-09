using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class UniformlySpacedRangeFloatUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniformlySpacedRangeFloat UniformlySpaced(this float start, float end, int count)
        {
            return new UniformlySpacedRangeFloat(start, end, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniformlySpacedRangeFloat UniformlySpaced<TMinMaxOwner>(this TMinMaxOwner minMaxOwner,
            int count)
            where TMinMaxOwner : IMinMaxOwner<float>
        {
            return new UniformlySpacedRangeFloat(minMaxOwner.Min, minMaxOwner.Max, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniformlySpacedRangeFloat CircularUniformlySpaced<TMinMaxOwner>(this TMinMaxOwner minMaxOwner,
            int count, float? offset = null)
            where TMinMaxOwner : IMinMaxOwner<float>
        {
            var step = (minMaxOwner.Max - minMaxOwner.Min) / count;
            offset ??= step / 2;
            
            var newMin = minMaxOwner.Min + offset.Value;
            var newMax = minMaxOwner.Max + offset.Value - step;
            return new UniformlySpacedRangeFloat(newMin, newMax, count);
        }
    }
}