using System;

namespace VMFramework.Configuration
{
    [Flags]
    public enum GeneralObjectFilterType
    {
        None = 0,
        Type = 1,
        ID = 2,
        GameTag = 4,
        ComponentType = 8,
    }
}