using System;

namespace VMFramework.GameEvents
{
    [Flags]
    public enum MouseButtonType {
        LeftButton = 1 << 0,
        RightButton = 1 << 1,
        MiddleButton = 1 << 2,
        AnyButton = LeftButton | RightButton | MiddleButton,
    }
}