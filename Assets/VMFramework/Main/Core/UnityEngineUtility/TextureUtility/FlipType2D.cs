using System;

namespace VMFramework.Core
{
    [Flags]
    public enum FlipType2D
    {
        NonFlipped = 1,
        X = 2,
        Y = 4,
        XY = 8,
        ALL_FLIP = X | Y | XY
    }
}