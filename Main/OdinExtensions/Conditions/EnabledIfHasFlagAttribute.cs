using System;
using System.Diagnostics;
using Sirenix.OdinInspector;

namespace VMFramework.OdinExtensions
{
    [DontApplyToListElements]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    [Conditional("UNITY_EDITOR")]
    public sealed class EnabledIfHasFlagAttribute : Attribute
    {
        public string Condition;

        public Enum[] Flags;

        public bool Animate;

        public EnabledIfHasFlagAttribute(string condition, params object[] flags)
        {
            Condition = condition;
            Flags = Array.ConvertAll(flags, x => (Enum)x);
            Animate = false;
        }
    }
}