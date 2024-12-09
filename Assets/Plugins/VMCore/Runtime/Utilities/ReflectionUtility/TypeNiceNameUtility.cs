using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace VMFramework.Core
{
    public static class TypeNiceNameUtility
    {
        private static readonly Dictionary<Type, string> cachedNiceNames = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetNiceFullName(this Type type)
        {
            if (type.IsNested && !type.IsGenericParameter)
            {
                return type.DeclaringType.GetNiceFullName() + "." + type.GetNiceName();
            }
            
            string niceFullName = type.GetNiceName();
            
            if (type.Namespace != null)
            {
                niceFullName = type.Namespace + "." + niceFullName;
            }
            
            return niceFullName;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetNiceName(this Type type)
        {
            if (cachedNiceNames.TryGetValue(type, out string result))
            {
                return result;
            }

            result = CreateNiceName(type);
            cachedNiceNames[type] = result;
            return result;
        }

        public static string CreateNiceName(Type type)
        {
            if (type.IsArray)
            {
                int arrayRank = type.GetArrayRank();
                
                string suffix;
                if (arrayRank == 1)
                {
                    suffix = "[]";
                }
                else
                {
                    suffix = "[" + new string(',', arrayRank - 1) + "]";
                }
                 
                return type.GetElementType().GetNiceName() + suffix;
            }

            if (type.IsDerivedFrom(typeof(Nullable<>), true, true))
            {
                return type.GetGenericArguments()[0].GetNiceName() + "?";
            }

            if (type.IsByRef)
            {
                return "ref " + type.GetElementType().GetNiceName();
            }

            if (type.IsGenericParameter || type.IsGenericType == false)
            {
                return type.Name;
            }

            StringBuilder stringBuilder = new StringBuilder();
            string name = type.Name;
            int num = name.IndexOf("`", StringComparison.Ordinal);
            stringBuilder.Append(num != -1 ? name[..num] : name);

            stringBuilder.Append('<');
            var genericArguments = type.GetGenericArguments();
            for (int i = 0; i < genericArguments.Length; i++)
            {
                Type genericArgument = genericArguments[i];
                if (i != 0)
                {
                    stringBuilder.Append(", ");
                }

                stringBuilder.Append(genericArgument.GetNiceName());
            }

            stringBuilder.Append('>');
            return stringBuilder.ToString();
        }
    }
}