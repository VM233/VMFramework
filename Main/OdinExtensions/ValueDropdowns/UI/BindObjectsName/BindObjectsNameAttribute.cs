using System;
using System.Diagnostics;

namespace VMFramework.OdinExtensions
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    [Conditional("UNITY_EDITOR")]
    public class BindObjectsNameAttribute : GeneralValueDropdownAttribute
    {
        public enum SingleModeLimitType
        {
            None,
            Single,
            NotSingle
        }

        public SingleModeLimitType SingleModeLimit { get; set; } = SingleModeLimitType.None;
        
        public bool AutoFirstThisGameObject { get; set; } = false;
        
        public bool AutoFirstParentGameObject { get; set; } = false;
    }
}