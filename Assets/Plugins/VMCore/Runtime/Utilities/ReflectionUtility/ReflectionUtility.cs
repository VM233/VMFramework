using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VMFramework.Core.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VMFramework.Core
{
    public static partial class ReflectionUtility
    {
        public const BindingFlags ALL_FIELDS_FLAGS = BindingFlags.Public | BindingFlags.NonPublic |
                                                             BindingFlags.Instance | BindingFlags.Static;
        
        public const BindingFlags ALL_STATIC_FIELDS_FLAGS = BindingFlags.Public | BindingFlags.NonPublic |
                                                             BindingFlags.Static;
        
        public const BindingFlags ALL_INSTANCE_FIELDS_FLAGS = BindingFlags.Public | BindingFlags.NonPublic |
                                                             BindingFlags.Instance;

        public const BindingFlags ALL_PROPERTIES_FLAGS = BindingFlags.Public |
                                                                 BindingFlags.NonPublic |
                                                                 BindingFlags.Instance | BindingFlags.Static;
        
        #region GetCopy

        

        #endregion

        #region Other

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSystemType(this Type type)
        {
            if (type?.Namespace == null)
            {
                return false;
            }

            return type.Namespace == "System" ||
                   type.Namespace.StartsWith("System.");
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsUnityType(this Type type)
        {
            if (type == null)
            {
                return false;
            }

            return type.Namespace == "UnityEngine" ||
                   type.Namespace.StartsWith("UnityEngine.");
        }

        #endregion
    }
}
