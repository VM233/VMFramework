using System;
using System.Diagnostics;

namespace VMFramework.OdinExtensions
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    [Conditional("UNITY_EDITOR")]
    public sealed class GamePrefabGeneralSettingAttribute : GeneralValueDropdownAttribute
    {
        
    }
}