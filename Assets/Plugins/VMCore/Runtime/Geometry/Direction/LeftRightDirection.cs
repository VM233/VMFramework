using System;

namespace VMFramework.Core
{
    /// <summary>
    /// Stands for the 1-dimensional direction of Left or Right.
    /// </summary>
    [Flags]
    public enum LeftRightDirection
    {
        None = 0,

        Left = 1,

        Right = 2,
        
        All = Left | Right
    }
}