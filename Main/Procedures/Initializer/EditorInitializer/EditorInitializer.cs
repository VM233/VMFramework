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
        
        public static IReadOnlyInitializerManager InitializerManager => initializerManager;

        public static bool IsInitialized => initializerManager is { IsInitialized: true }; 
        
        [InitializeOnLoadMethod]
        private static void InitializationEntry()
        {
            EditorApplication.delayCall += Initialize;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode)
            {
                EditorApplication.delayCall += Initialize;
            }
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
