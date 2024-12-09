using System;
using MathNet.Numerics.Random;

namespace VMFramework.Core
{
    public static class GlobalRandom
    {
        // public static Random Default { get; } = new(RandomSeed.Robust());
        public static Random Default { get; } = new();
    }
}