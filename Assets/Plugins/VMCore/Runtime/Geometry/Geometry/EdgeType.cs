using System;

namespace VMFramework.Core
{
    [Flags]
    public enum EdgeType
    {
        /// <summary>
        /// Edge between Right and Up faces
        /// </summary>
        RightUp = 1 << 0,

        /// <summary>
        /// Edge between Right and Down faces
        /// </summary>
        RightDown = 1 << 1,

        /// <summary>
        /// Edge between Right and Forward faces
        /// </summary>
        RightForward = 1 << 2,

        /// <summary>
        /// Edge between Right and Back faces
        /// </summary>
        RightBack = 1 << 3,

        /// <summary>
        /// Edge between Left and Up faces
        /// </summary>
        LeftUp = 1 << 4,

        /// <summary>
        /// Edge between Left and Down faces
        /// </summary>
        LeftDown = 1 << 5,

        /// <summary>
        /// Edge between Left and Forward faces
        /// </summary>
        LeftForward = 1 << 6,

        /// <summary>
        /// Edge between Left and Back faces
        /// </summary>
        LeftBack = 1 << 7,

        /// <summary>
        /// Edge between Up and Forward faces
        /// </summary>
        UpForward = 1 << 8,

        /// <summary>
        /// Edge between Up and Back faces
        /// </summary>
        UpBack = 1 << 9,

        /// <summary>
        /// Edge between Down and Forward faces
        /// </summary>
        DownForward = 1 << 10,

        /// <summary>
        /// Edge between Down and Back faces
        /// </summary>
        DownBack = 1 << 11,
    }
}