using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class AssemblyUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyCollection<string> GetNamespaces(this Assembly assembly)
        {
            var namespaces = new HashSet<string>();

            foreach (var type in assembly.GetTypes())
            {
                namespaces.Add(type.Namespace);
            }
            
            return namespaces;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Assembly GetAssembly(this string assemblyName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return assemblies.FirstOrDefault(assembly =>
                assembly.GetName().Name == assemblyName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Assembly GetAssemblyStrictly(this string assemblyName)
        {
            var result = assemblyName.GetAssembly();

            if (result == null)
            {
                throw new InvalidOperationException($"未找到Assembly:{assemblyName}");
            }

            return result;
        }
    }
}