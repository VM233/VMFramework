using System;

namespace VMFramework.GameEvents
{
    [Flags]
    public enum MouseEventType
    {
        None = 0,

        PointerEnter = 1 << 0,
        PointerExit = 1 << 1,
        PointerStay = 1 << 2,

        AnyMouseButtonDown = 1 << 3,
        AnyMouseButtonUp = 1 << 4,
        AnyMouseButtonStay = 1 << 5,

        LeftMouseButtonDown = 1 << 6,
        LeftMouseButtonUp = 1 << 7,
        LeftMouseButtonClick = 1 << 8,
        LeftMouseButtonStay = 1 << 9,

        RightMouseButtonDown = 1 << 10,
        RightMouseButtonUp = 1 << 11,
        RightMouseButtonClick = 1 << 12,
        RightMouseButtonStay = 1 << 13,

        MiddleMouseButtonDown = 1 << 14,
        MiddleMouseButtonUp = 1 << 15,
        MiddleMouseButtonClick = 1 << 16,
        MiddleMouseButtonStay = 1 << 17,

        DragBegin = 1 << 18,
        DragStay = 1 << 19,
        DragEnd = 1 << 20,
        
        PointerEnterMultiple = 1 << 21,
        PointerExitMultiple = 1 << 22,
        PointerStayMultiple = 1 << 23,
    }
}