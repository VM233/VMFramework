using System;

namespace VMFramework.UI
{
    [Flags]
    public enum VisualElementSelectionMode
    {
        None = 0,
        Self = 1,
        DirectChildren = 2,
        IndirectChildren = 4,
    }
}