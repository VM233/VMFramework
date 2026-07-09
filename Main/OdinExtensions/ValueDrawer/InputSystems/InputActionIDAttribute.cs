#if ENABLE_INPUT_SYSTEM
using System;
using System.Diagnostics;

namespace VMFramework.OdinExtensions
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public sealed class InputActionIDAttribute : GeneralValueDropdownAttribute
    {
        
    }
}
#endif