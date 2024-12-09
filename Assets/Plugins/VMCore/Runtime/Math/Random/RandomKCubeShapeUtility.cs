using System;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class RandomKCubeShapeUtility
    {
        /// <summary>
        /// 获得区间[from, to]内的一个随机的整数区间
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeInteger RandomRangeInteger(this Random random, int from, int to)
        {
            to.AssertIsAboveOrEqual(from, nameof(to), nameof(from));

            int start = random.Range(from, to);
            int length = random.Range(0, to - from + 1);

            int end = (start + length).Repeat(from, to);

            return start > end ? new(end, start) : new(start, end);
        }

        /// <summary>
        /// 获得区间[from, to]内的一个随机的浮点数区间
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeFloat RandomRangeFloat(this Random random, float from, float to)
        {
            to.AssertIsAboveOrEqual(from, nameof(to), nameof(from));

            float start = random.Range(from, to);
            float length = random.Range(0, to - from);

            float end = (start + length).Repeat(from, to);

            return start > end ? new(end, start) : new(start, end);
        }

        /// <summary>
        /// 获得区间[from, to]内的一个随机的整数区间
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeInteger RandomRangeInteger(this int from, int to) =>
            GlobalRandom.Default.RandomRangeInteger(from, to);

        /// <summary>
        /// 获得区间[from, to]内的一个随机的浮点数区间
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeFloat RandomRangeFloat(this float from, float to) =>
            GlobalRandom.Default.RandomRangeFloat(from, to);
    }
}