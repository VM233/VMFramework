using System;

namespace VMFramework.Editor.BatchProcessor
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class UnitSettingsAttribute : Attribute
    {
        public int Priority { get; init; }
        
        public UnitPriority UnitPriority
        {
            init => Priority = (int)value;
        }

        public UnitSettingsAttribute(int priority)
        {
            Priority = priority;
        }

        public UnitSettingsAttribute(UnitPriority unitPriority)
        {
            UnitPriority = unitPriority;
        }
    }
}