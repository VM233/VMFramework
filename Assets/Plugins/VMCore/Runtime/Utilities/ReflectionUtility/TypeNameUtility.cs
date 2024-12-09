using System;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class TypeNameUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetTypeNameWithoutGenerics(this Type type)
        {
            if (type == null)
            {
                return null;
            }
            
            var typeName = type.Name;
            if (type.IsGenericType)
            {
                typeName = typeName[..typeName.IndexOf('`')];
            }

            return typeName;
        }
    }
}