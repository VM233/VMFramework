#if UNITY_EDITOR
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using VMFramework.Core.Editor;

namespace VMFramework.Editor.BatchProcessor
{
    public sealed class BatchProcessorWindow : OdinEditorWindow
    {
        private BatchProcessorContainer container;

        private static BatchProcessorWindow GetWindow()
        {
            bool hasOpenedWindow = HasOpenInstances<BatchProcessorWindow>();
            var window = GetWindow<BatchProcessorWindow>(BatchProcessorNames.BATCH_PROCESSOR_NAME);

            if (hasOpenedWindow == false)
            {
                window.position = GUIHelper.GetEditorWindowRect().AlignCenter(900, 600);
            }

            return window;
        }

        [MenuItem(UnityMenuItemNames.VMFRAMEWORK + BatchProcessorNames.BATCH_PROCESSOR_NAME + " #B", false, 200)]
        public static void OpenWindow()
        {
            GetWindow();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OpenWindow(IEnumerable<object> selectedObjects)
        {
            GetWindow().container.SetSelectedObjects(selectedObjects);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OpenWindow(params object[] selectedObjects)
        {
            OpenWindow((IEnumerable<object>)selectedObjects);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddToWindow(IEnumerable<object> additionalObjects)
        {
            GetWindow().container.AddSelectedObjects(additionalObjects);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddToWindow(params object[] additionalObjects)
        {
            AddToWindow((IEnumerable<object>)additionalObjects);
        }

        [MenuItem("Assets/" + BatchProcessorNames.BATCH_PROCESSOR_NAME)]
        [MenuItem("GameObject/" + BatchProcessorNames.BATCH_PROCESSOR_NAME)]
        public static void OpenWindowFromContextMenu()
        {
            var selectedObjects = Selection.objects;

            if (selectedObjects.Length == 1 && selectedObjects[0].IsFolder())
            {
                OpenWindow(selectedObjects[0].GetAllAssetsInFolder());
                return;
            }

            OpenWindow(Selection.objects);
        }

        [MenuItem("Assets/" + BatchProcessorNames.ADD_TO_BATCH_PROCESSOR_NAME)]
        [MenuItem("GameObject/" + BatchProcessorNames.ADD_TO_BATCH_PROCESSOR_NAME)]
        public static void AddToWindowFromContextMenu()
        {
            var selectedObjects = Selection.objects;

            if (selectedObjects.Length == 1 && selectedObjects[0].IsFolder())
            {
                AddToWindow(selectedObjects[0].GetAllAssetsInFolder());
                return;
            }

            AddToWindow(Selection.objects);
        }

        protected override void Initialize()
        {
            base.Initialize();

            if (container == null)
            {
                container = CreateInstance<BatchProcessorContainer>();
            }

            container.Init();
        }

        protected override object GetTarget()
        {
            return container;
        }

        protected override void OnImGUI()
        {
            if (Event.current.type == EventType.MouseDown)
            {
                container.UpdateValidUnits();
            }

            base.OnImGUI();
        }
    }
}
#endif