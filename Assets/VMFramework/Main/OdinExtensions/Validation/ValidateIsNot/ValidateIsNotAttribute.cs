using System;
using System.Diagnostics;

namespace VMFramework.OdinExtensions
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public sealed class ValidateIsNotAttribute : SingleValidationAttribute
    {
        public object[] Contents;

        public string[] ContentGetters;

        public ValidateIsNotAttribute(params string[] contentGetters) : base()
        {
            ContentGetters = contentGetters;
        }

        public ValidateIsNotAttribute(params object[] contents) : base()
        {
            Contents = contents;
        }
    }
}
