using System;

namespace VMFramework.Parameters
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ParameterDefineAttribute : Attribute
    {
        public string Name { get; set; }

        public Type Type { get; set; }

        public bool IsCollection { get; set; }

        public ParameterDefineAttribute(string name, Type type, bool isCollection = false)
        {
            Name = name;
            Type = type;
            IsCollection = isCollection;
        }
    }
}