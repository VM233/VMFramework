using System;

namespace VMFramework.Core
{
    /// <summary>
    /// Stands for the eight types of directions in a 2D plane.
    /// </summary>
    [Flags]
    public enum EightTypesDirection
    {
        None = 0,
        
        Left = 1,
        
        Right = 2,
        
        Up = 4,
        
        Down = 8,
        
        UpLeft = 16,

        UpRight = 32,

        DownLeft = 64,

        DownRight = 128,
        
        Horizontal = Left | Right,
        
        Vertical = Up | Down,
        
        Cardinal = Up | Down | Left | Right,
        
        Corner = UpLeft | UpRight | DownLeft | DownRight,
        
        All = Up | Down | Left | Right | UpLeft | UpRight | DownLeft | DownRight
    }
}