using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VMFramework.Configuration
{
    /// <summary>
    /// Stores a <see cref="Type"/> through Unity's native string serialization.
    /// </summary>
    [Serializable]
    public sealed class SerializableType
    {
        [SerializeField]
        [HideInInspector]
        private string assemblyQualifiedName;

        [NonSerialized]
        private bool hasResolvedValue;

        [NonSerialized]
        private string resolvedAssemblyQualifiedName;

        [NonSerialized]
        private Type resolvedValue;

        [ShowInInspector]
        public Type Value
        {
            get
            {
                if (hasResolvedValue == false ||
                    string.Equals(resolvedAssemblyQualifiedName,
                        assemblyQualifiedName, StringComparison.Ordinal) == false)
                {
                    resolvedValue = Resolve(assemblyQualifiedName);
                    resolvedAssemblyQualifiedName = assemblyQualifiedName;
                    hasResolvedValue = true;
                }

                return resolvedValue;
            }
            set
            {
                string identifier = value?.AssemblyQualifiedName;
                if (value != null && string.IsNullOrEmpty(identifier))
                {
                    throw new ArgumentException(
                        $"Type '{value}' has no assembly-qualified name and " +
                        "cannot be serialized by Unity.", nameof(value));
                }

                assemblyQualifiedName = identifier;
                resolvedAssemblyQualifiedName = identifier;
                resolvedValue = value;
                hasResolvedValue = true;
            }
        }

        public SerializableType()
        {
        }

        public SerializableType(Type value)
        {
            Value = value;
        }

        public static implicit operator Type(SerializableType serializableType)
        {
            return serializableType?.Value;
        }

        public static implicit operator SerializableType(Type type)
        {
            return new SerializableType(type);
        }

        private static Type Resolve(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                return null;
            }

            Type type = Type.GetType(identifier, false);
            if (type == null)
            {
                throw new TypeLoadException(
                    $"Serialized type '{identifier}' could not be resolved.");
            }

            return type;
        }
    }
}
