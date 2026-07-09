using System;
using System.Diagnostics;
using UnityEngine.UIElements;
using VMFramework.Core;

namespace VMFramework.OdinExtensions
{
    /// <summary>
    /// Restricts which VisualElement types are shown in VisualElementPath selections and optionally
    /// specifies the VisualTreeAsset field/property name to use.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public sealed class VisualElementPathSettingsAttribute : Attribute
    {
        public Type[] AllowedTypes { get; }

        /// <summary>
        /// Optional field or property name on the target object that provides the VisualTreeAsset.
        /// </summary>
        public string VisualTreeFieldName { get; set; }

        /// <summary>
        /// When true, the drawer will search IVisualTreeAssetProvider on the target or its parents.
        /// </summary>
        public bool IsFromLocalProvider { get; set; } = false;
        
        public bool MustFromParent { get; set; } = false;

        public VisualElementPathSettingsAttribute(params Type[] allowedTypes)
        {
            AllowedTypes = allowedTypes is { Length: > 0 } ? allowedTypes : new[] { typeof(VisualElement) };

            foreach (var type in AllowedTypes)
            {
                if (type.IsDerivedFrom<VisualElement>(true) == false)
                {
                    throw new ArgumentException($"{type.Name} is not a VisualElement.");
                }
            }
        }
    }
}
