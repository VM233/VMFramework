using System;
using System.Diagnostics;
using UnityEngine.UIElements;
using VMFramework.Core;
using Debug = UnityEngine.Debug;

namespace VMFramework.OdinExtensions
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    [Conditional("UNITY_EDITOR")]
    public sealed class VisualElementNameAttribute : GeneralValueDropdownAttribute
    {
        public bool IsProvider { get; set; } = false;
        
        public Type[] VisualElementTypes { get; }

        public VisualElementNameAttribute(params Type[] visualElementTypes)
        {
            VisualElementTypes = visualElementTypes;
            
            Check();
        }

        public VisualElementNameAttribute()
        {
            VisualElementTypes = new[] { typeof(VisualElement) };
            
            Check();
        }

        private void Check()
        {
            foreach (var type in VisualElementTypes)
            {
                if (type.IsInterface)
                {
                    continue;
                }
                
                if (type.IsDerivedFrom<VisualElement>(true) == false)
                {
                    Debug.LogError($"Type {type.Name} is not a derived class of VisualElement.");
                }
            }
        }
    }
}