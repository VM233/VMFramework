using System;

namespace VMFramework.Core
{
    [Flags]
    public enum FaceType
    {
        /// <summary>
        /// pos with offset (1, 0, 0)
        /// </summary>
        Right = 1 << 1,

        /// <summary>
        /// pos with offset (-1, 0, 0)
        /// </summary>
        Left = 1 << 2,

        /// <summary>
        /// pos with offset (0, 1, 0)
        /// </summary>
        Up = 1 << 3,

        /// <summary>
        /// pos with offset (0, -1, 0)
        /// </summary>
        Down = 1 << 4,

        /// <summary>
        /// pos with offset (0, 0, 1)
        /// </summary>
        Forward = 1 << 5,

        /// <summary>
        /// pos with offset (0, 0, -1)
        /// </summary>
        Back = 1 << 6,

        /// <summary>
        /// AllDirectionFace
        /// </summary>
        All = Right | Left | Up | Down | Forward | Back,
    }
}