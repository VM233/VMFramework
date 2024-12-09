using System;
using System.Diagnostics;

namespace VMFramework.OdinExtensions
{
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public sealed class CollectionNotContainsAttribute : SingleValidationAttribute
    {
        public bool IsContentValid { get; }

        public object Content { get; }

        public string ContentGetter { get; }

        public CollectionNotContainsAttribute(string contentGetter)
        {
            IsContentValid = false;
            ContentGetter = contentGetter;
        }

        public CollectionNotContainsAttribute(object content)
        {
            IsContentValid = true;
            Content = content;
        }
    }
}
