using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using VMFramework.Core.Linq;

namespace VMFramework.Core.Editor
{
    public static class AssetSaveUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EnforceSave<TEnumerable>(this TEnumerable objects) where TEnumerable : IEnumerable<Object>
        {
            if (objects.IsNullOrEmpty())
            {
                Debug.LogWarning($"Failed to save {typeof(TEnumerable)}, because it is null or empty");
                return;
            }

            foreach (var obj in objects)
            {
                if (obj == null)
                {
                    continue;
                }
                
                obj.SetEditorDirty();
            }
            
            if (EditorApplication.isUpdating || EditorApplication.isCompiling)
            {
                return;
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EnforceSave(this Object obj)
        {
            if (obj == null)
            {
                Debug.LogWarning($"Failed to save Object, because it is null");
                return;
            }
            
            obj.SetEditorDirty();
            
            if (EditorApplication.isUpdating || EditorApplication.isCompiling)
            {
                return;
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetEditorDirty(this Object obj)
        {
            if (obj == null)
            {
                Debug.LogWarning("Object is null, cannot set dirty");
                return;
            }
            
            EditorUtility.SetDirty(obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetEditorDirty<TEnumerable>(this TEnumerable objects)
            where TEnumerable : IEnumerable<Object>
        {
            foreach (var obj in objects)
            {
                if (obj == null)
                {
                    continue;
                }
                
                obj.SetEditorDirty();
            }
        }
    }
}