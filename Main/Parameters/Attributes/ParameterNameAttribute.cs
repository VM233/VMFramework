using System;
using System.Diagnostics;
using VMFramework.OdinExtensions;

namespace VMFramework.Parameters
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    [Conditional("UNITY_EDITOR")]
    public class ParameterNameAttribute : GeneralValueDropdownAttribute
    {
        public string TypeGetter { get; set; }
        
        public bool IsCollection { get; set; }

        public ParameterNameAttribute(string typeGetter, bool isCollection)
        {
            TypeGetter = typeGetter;
            IsCollection = isCollection;
        }
    }
}