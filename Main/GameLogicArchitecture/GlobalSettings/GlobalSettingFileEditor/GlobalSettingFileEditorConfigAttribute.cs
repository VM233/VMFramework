using System;
using System.Diagnostics;

namespace VMFramework.GameLogicArchitecture.Editor
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    [Conditional("UNITY_EDITOR")]
    public class GlobalSettingFileEditorConfigAttribute : Attribute
    {
        public string FolderPath { get; init; }
    }
}