using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class ObjectUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FindObject<T>(this string name) where T : Object
        {
#if UNITY_2023_1_OR_NEWER
            var results = Object.FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);     
#else
            var results = Object.FindObjectsOfType<T>(true);
#endif

            return results.FirstOrDefault(result => result.name == name);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsUnityNull(this object obj)
        {
            if (obj is Object unityObj)
            {
                return unityObj == null;
            }
            
            return obj == null;
        }
    }
}