using System;

namespace VMFramework.Core
{
    /// <summary>
    /// Stands for the 2d directions of Left, Right, Up, and Down.
    /// </summary>
    [Flags]
    public enum FourTypesDirection
    {
        None = 0,

        Left = 1,
        
        Right = 2,
        
        Up = 4,
        
        Down = 8,
        
        Horizontal = Left | Right,
        
        Vertical = Up | Down,

        All = Up | Down | Left | Right
    }
}