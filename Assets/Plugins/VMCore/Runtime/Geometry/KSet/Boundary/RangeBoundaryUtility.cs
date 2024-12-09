using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class RangeBoundaryUtility
    {
        #region Get Boundary

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeBoundary GetBoundary<TMinMaxOwner>(this TMinMaxOwner minMaxOwner)
            where TMinMaxOwner : IMinMaxOwner<int> =>
            new(minMaxOwner.Min, minMaxOwner.Max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeBoundary GetBoundary(this int min, int max) => new(min, max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeBoundary GetBoundary(this int size) => new(0, size - 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeBoundary GetInnerBoundary<TMinMaxOwner>(this TMinMaxOwner minMaxOwner)
            where TMinMaxOwner : IMinMaxOwner<int> =>
            new(minMaxOwner.Min + 1, minMaxOwner.Max - 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeBoundary GetInnerBoundary(this int min, int max) => new(min + 1, max - 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeBoundary GetInnerBoundary(this int size) => new(1, size - 2);

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int MinDistanceToBoundary(this int value, RangeBoundary boundary)
        {
            return value.MinDistanceToBoundary(boundary.min, boundary.max);
        }
    }
}