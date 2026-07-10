#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Editor;

namespace VMFramework.Procedure.Editor
{ 
    internal static class EditorInitializer
    {
        private static InitializerManager initializerManager;
        private static bool isInitializationScheduled;
        
        public static IReadOnlyInitializerManager InitializerManager => initializerManager;

        public static bool IsInitialized => initializerManager is { IsInitialized: true }; 
        
        [InitializeOnLoadMethod]
        private static void InitializationEntry()
        {
            ScheduleInitialize();
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode)
            {
                ScheduleInitialize();
            }
        }

        internal static void ScheduleInitialize()
        {
            if (isInitializationScheduled || initializerManager is { IsInitializing: true })
            {
                return;
            }

            isInitializationScheduled = true;
            EditorApplication.delayCall += RunScheduledInitialization;
        }

        private static void RunScheduledInitialization()
        {
            isInitializationScheduled = false;
            Initialize();
        }
        
        [MenuItem(UnityMenuItemNames.EDITOR_INITIALIZATION + "Editor Initialize")]
        public static async void Initialize()
        {
            if (Application.isPlaying)
            {
                return;
            }
            
            initializerManager = new();
            
            var initializers = new List<IEditorInitializer>();
            
            foreach (var derivedClass in typeof(IEditorInitializer).GetDerivedInstantiableClasses(false))
            {
                var initializer = (IEditorInitializer)derivedClass.CreateInstance();
                
                initializers.Add(initializer);
            }
            
            initializerManager.Set(initializers);

            await initializerManager.Initialize();
            
            UnityEngine.Debug.Log("Editor Initialization Done!");
        }
    }
}
#endif
