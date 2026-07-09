#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VMFramework.Core.Editor
{
    public static class InspectorWindowEditor
    {
        private static readonly Type inspectorWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.InspectorWindow");

        /// <summary>
        /// Opens a new inspector window for the specified object.
        /// </summary>
        /// <param name="unityObj">The unity object.</param>
        /// <exception cref="T:System.ArgumentNullException">unityObj</exception>
        public static void OpenInNewInspector(this Object unityObj)
        {
            if (unityObj == null)
            {
                UnityEngine.Debug.LogWarning($"Cannot open inspector window for null object.");
                return;
            }
            
            Vector2 size = new Vector2(450f, 600f);
            Rect rect = new Rect(Vector2.zero, size);
            EditorWindow instance = ScriptableObject.CreateInstance(inspectorWindowType) as EditorWindow;
            instance.Show();
            Object[] prevSelection = Selection.objects;
            Selection.activeObject = unityObj;
            inspectorWindowType
                .GetProperty("isLocked",
                    BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                    BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).GetSetMethod().Invoke(
                    instance, new object[1]
                    {
                        true
                    });
            Selection.objects = prevSelection;
            instance.position = rect;
            
            if (unityObj.GetType().IsDerivedFrom(typeof(Texture2D), true) == false)
            {
                return;
            }
            
            EditorApplication.delayCall += () =>
            {
                EditorApplication.delayCall += () =>
                {
                    Selection.objects = prevSelection;
                };
            };
        }
    }
}
#endif