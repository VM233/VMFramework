using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using VMFramework.Core.Linq;

namespace VMFramework.Core.Editor
{
    public static class ScriptOperationUtility
    {
        #region Select Script

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SelectScriptOfType(this Type type)
        {
            if (type == null)
            {
                return;
            }
            
            var typeName = type.GetTypeNameWithoutGenerics();

            if (typeName.TryGetMonoScript(out var mono))
            {
                Selection.activeObject = mono;
            }
            else
            {
                Debug.LogWarning($"Failed to select script of type {type.Name}, " +
                                 $"because no script file named {typeName}.cs exists in the project.");
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SelectScriptOfObject(this object obj)
        {
            if (obj.IsUnityNull())
            {
                Debug.LogWarning($"{nameof(obj)} is null! Can't select script.");
                return;
            }
            
            obj.GetType().SelectScriptOfType();
        }

        #endregion
        
        #region Open Script

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OpenScriptOfType(this Type type, bool openAllScripts = false)
        {
            if (type == null)
            {
                return false;
            }
            
            var typeName = type.GetTypeNameWithoutGenerics();

            if (openAllScripts)
            {
                if (typeName.TryGetMonoScripts(out var scripts))
                {
                    scripts.OpenAssets();
                    return true;
                }
            }
            else
            {
                if (typeName.TryGetMonoScript(out var mono))
                {
                    mono.OpenAsset();
                    return true;
                }
            }

            Debug.LogWarning($"Failed to open script of type {type.Name}, " +
                             $"because no script file named {typeName}.cs exists in the project.");
            return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OpenScriptOfObject(this object obj)
        {
            if (obj.IsUnityNull())
            {
                Debug.LogWarning($"{nameof(obj)} is null! Can't open script.");
                return;
            }
            
            obj.GetType().OpenScriptOfType();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OpenScriptOfObjects<TObject>(this IEnumerable<TObject> objects)
        {
            if (objects == null)
            {
                Debug.LogWarning($"{nameof(objects)} is null! Can't open script.");
                return;
            }
            
            objects.Examine(obj => obj.OpenScriptOfObject());
        }

        #endregion
    }
}