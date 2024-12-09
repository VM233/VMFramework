using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class SteppedRangeUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ForwardSteppedRangeInteger GetForwardSteppedRange(this int start, int end, int step)
        {
            return new ForwardSteppedRangeInteger(start, end, step);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ForwardSteppedRangeInteger GetForwardSteppedRange<TMinMaxOwner>(this TMinMaxOwner minMaxOwner,
            int step)
            where TMinMaxOwner : IMinMaxOwner<int>
        {
            return new ForwardSteppedRangeInteger(minMaxOwner.Min, minMaxOwner.Max, step);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ForwardSteppedRangeFloat GetForwardSteppedRange(this float start, float end, float step)
        {
            return new ForwardSteppedRangeFloat(start, end, step);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ForwardSteppedRangeFloat GetForwardSteppedRange<TMinMaxOwner>(this TMinMaxOwner minMaxOwner,
            float step)
            where TMinMaxOwner : IMinMaxOwner<float>
        {
            return new ForwardSteppedRangeFloat(minMaxOwner.Min, minMaxOwner.Max, step);
        }
    }
}