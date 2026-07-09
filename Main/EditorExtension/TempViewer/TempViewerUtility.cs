#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;

namespace VMFramework.Editor
{
    public static class TempViewerUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ShowTempViewer<TObject>(this TObject obj)
        {
            var container = ScriptableObject.CreateInstance<TempViewerContainer>();
            container.obj = obj;
            container.OpenInNewInspector();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ShowTempViewer<TObject>(this IEnumerable<TObject> objects)
        {
            var container = ScriptableObject.CreateInstance<TempListViewerContainer>();
            container.objects = objects.Cast<object>().ToList();
            container.OpenInNewInspector();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ShowConfirmViewer<TObject>(this TObject obj, Action<TObject> onConfirm)
        {
            var container = ScriptableObject.CreateInstance<TempConfirmContainer>();
            var window = OdinEditorWindow.InspectObject(container);

            EditorApplication.delayCall += () =>
            {
                if (window.position.width < 600)
                {
                    window.position = window.position.AlignCenterX(600);
                }
            };
            
            container.Init(obj, () =>
            {
                onConfirm((TObject)container.obj);
            }, window);
        }
    }
}
#endif