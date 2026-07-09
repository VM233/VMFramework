using System;
using System.Diagnostics;
using VMFramework.Core;

namespace VMFramework.OdinExtensions
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    [Conditional("UNITY_EDITOR")]
    public sealed class CommonPresetAttribute : GeneralValueDropdownAttribute
    {
        public string[] Keys { get; set; }

        public CommonPresetAttribute()
        {
            
        }
        
        public CommonPresetAttribute(params string[] keys)
        {
            Keys = keys;

            if (keys.IsNullOrEmpty())
            {
                throw new ArgumentException("Key cannot be null or empty.");
            }
        }
    }
}