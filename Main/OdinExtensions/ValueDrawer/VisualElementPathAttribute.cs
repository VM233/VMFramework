using System;
using System.Diagnostics;

namespace VMFramework.OdinExtensions
{
    /// <summary>
    /// Stores a path of VisualElement names that can be resolved inside a VisualTreeAsset.
    /// The path is expected to be stored in a VisualElementPath instance.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public sealed class VisualElementPathAttribute : Attribute
    {
        /// <summary>
        /// Optional field or property name that provides the VisualTreeAsset to inspect.
        /// If not set, the drawer will look for an IVisualTreeAssetProvider or UIDocument on the owner.
        /// </summary>
        public string VisualTreeAssetMemberName { get; }

        /// <summary>
        /// When true, the drawer searches parent components that implement IVisualTreeAssetProvider.
        /// </summary>
        public bool SearchInParents { get; set; } = true;

        /// <summary>
        /// When true, the drawer will also try to read the VisualTreeAsset from a UIDocument.
        /// </summary>
        public bool AllowUIDocumentFallback { get; set; } = true;

        public VisualElementPathAttribute(string visualTreeAssetMemberName = null)
        {
            VisualTreeAssetMemberName = visualTreeAssetMemberName;
        }
    }
}
