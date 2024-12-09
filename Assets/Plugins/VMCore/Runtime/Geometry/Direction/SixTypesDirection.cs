using System;

namespace VMFramework.Core
{
    /// <summary>
    /// Stands for the 3-dimensional directions of Left, Right, Up, Down, Forward, and Back.
    /// </summary>
    [Flags]
    public enum SixTypesDirection
    {
        None = 0,

        Left = 1,

        Right = 2,

        Up = 4,

        Down = 8,

        Forward = 16,

        Back = 32,

        All = Left | Right | Up | Down | Forward | Back
    }
}