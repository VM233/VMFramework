using System;

namespace VMFramework.GameLogicArchitecture
{
    [UnityEngine.Scripting.RequireAttributeUsages]
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class GlobalSettingFileConfigAttribute : Attribute
    {
        public string FileName { get; init; }
        
        public Type LoaderType { get; init; }
    }
}
