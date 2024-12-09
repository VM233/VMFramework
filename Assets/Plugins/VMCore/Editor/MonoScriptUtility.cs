using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;

namespace VMFramework.Core.Editor
{
    public static class MonoScriptUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyList<MonoScript> GetMonoScripts(this string scriptName)
        {
            var results = new List<MonoScript>();

            foreach (var monoScript in scriptName.FindAssetsOfName<MonoScript>())
            {
                if (monoScript.name == scriptName)
                {
                    results.Prepend(monoScript);
                }
                else if (monoScript.name.StartsWith(scriptName + "."))
                {
                    results.Add(monoScript);
                }
            }
            
            return results;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetMonoScripts(this string scriptName, out IReadOnlyList<MonoScript> scripts)
        {
            scripts = GetMonoScripts(scriptName);
            return scripts.Count > 0;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MonoScript GetMonoScript(this string scriptName)
        {
            return GetMonoScripts(scriptName).FirstOrDefault();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetMonoScript(this string scriptName, out MonoScript monoScript)
        {
            monoScript = GetMonoScript(scriptName);
            return monoScript!= null;
        }
    }
}