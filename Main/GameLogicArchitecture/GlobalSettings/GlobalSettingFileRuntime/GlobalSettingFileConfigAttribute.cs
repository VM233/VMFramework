using System;

namespace VMFramework.GameLogicArchitecture
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class GlobalSettingFileConfigAttribute : Attribute
    {
        public string FileName { get; init; }
        
        public Type LoaderType { get; init; }
    }
}