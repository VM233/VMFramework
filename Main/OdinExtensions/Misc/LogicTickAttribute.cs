using System;
using System.Diagnostics;

namespace VMFramework.OdinExtensions
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    [Conditional("UNITY_EDITOR")]
    public class LogicTickAttribute : Attribute
    {
        
    }
}