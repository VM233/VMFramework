#if ODIN_INSPECTOR
using System;
using System.Diagnostics;
using UnityEngine;
using VMFramework.Core;
using Debug = UnityEngine.Debug;

namespace VMFramework.OdinExtensions
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    [Conditional("UNITY_EDITOR")]
    public sealed class UGUINameAttribute : GeneralValueDropdownAttribute
    {
        public Type[] UGUITypes { get; }

        public UGUINameAttribute(params Type[] uguiTypes)
        {
            UGUITypes = uguiTypes;
            
            Check();
        }

        public UGUINameAttribute()
        {
            UGUITypes = new[] { typeof(RectTransform) };
            
            Check();
        }

        private void Check()
        {
            foreach (var type in UGUITypes)
            {
                if (type.IsInterface)
                {
                    continue;
                }
                
                if (type.IsDerivedFrom<Component>(true) == false)
                {
                    Debug.LogError($"Type {type.Name} is not a derived class of Component.");
                }
            }
        }
    }
}
#endif