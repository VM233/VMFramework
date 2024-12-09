using System;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class RandomBoolUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool NextBool(this Random random) => random.Next(2) == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool NextBool(this Random random, float trueProbability) => random.NextDouble() < trueProbability;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool RandomBool(this float trueProbability) => GlobalRandom.Default.NextBool(trueProbability);
    }
}