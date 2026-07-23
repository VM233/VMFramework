#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Editor;

namespace VMFramework.Procedure.Editor
{
    internal static class EditorInitializer
    {
        private static InitializerManager initializerManager;
        private static CancellationTokenSource initializationCancellation;

        internal static event Action InitializationCompleted;

        public static IReadOnlyInitializerManager InitializerManager => initializerManager;

        public static bool IsInitialized => initializerManager is { IsInitialized: true };

        [InitializeOnLoadMethod]
        private static void InitializationEntry()
        {
            ScheduleInitialize();
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            AssemblyReloadEvents.beforeAssemblyReload += CancelInitialization;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                CancelInitialization();
            }
            else if (state == PlayModeStateChange.EnteredEditMode)
            {
                ScheduleInitialize();
            }
        }

        internal static void ScheduleInitialize()
        {
            if (initializerManager is { IsInitializing: true })
            {
                return;
            }

            EditorApplication.update -= RunScheduledInitialization;
            EditorApplication.update += RunScheduledInitialization;
        }

        private static void RunScheduledInitialization()
        {
            if (Application.isPlaying || EditorApplication.isCompiling || EditorApplication.isUpdating ||
                EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            EditorApplication.update -= RunScheduledInitialization;
            StartInitialization();
        }

        [MenuItem(UnityMenuItemNames.EDITOR_INITIALIZATION + "Editor Initialize")]
        private static void InitializeFromMenu()
        {
            StartInitialization();
        }

        private static void StartInitialization()
        {
            if (Application.isPlaying || initializerManager is { IsInitializing: true })
            {
                return;
            }

            CancelInitialization();

            var cancellation = new CancellationTokenSource();
            initializationCancellation = cancellation;
            RunInitialization(cancellation).Forget(LogInitializationException);
        }

        private static async UniTask RunInitialization(CancellationTokenSource cancellation)
        {
            try
            {
                await Initialize(cancellation.Token);
            }
            finally
            {
                if (ReferenceEquals(initializationCancellation, cancellation))
                {
                    initializationCancellation = null;
                }

                cancellation.Dispose();
            }

            InitializationCompleted?.Invoke();
        }

        internal static async UniTask Initialize(CancellationToken cancellationToken = default)
        {
            if (Application.isPlaying)
            {
                return;
            }

            EditorApplication.update -= RunScheduledInitialization;

            initializerManager = new InitializerManager();

            var initializers = new List<IEditorInitializer>();
            foreach (var derivedClass in typeof(IEditorInitializer).GetDerivedInstantiableClasses(false))
            {
                var initializer = (IEditorInitializer)derivedClass.CreateInstance();
                initializers.Add(initializer);
            }

            initializerManager.Set(initializers);
            await initializerManager.Initialize(cancellationToken);

            UnityEngine.Debug.Log("Editor Initialization Done!");
        }

        private static void CancelInitialization()
        {
            initializationCancellation?.Cancel();
        }

        private static void LogInitializationException(Exception exception)
        {
            if (exception is OperationCanceledException)
            {
                return;
            }

            UnityEngine.Debug.LogException(exception);
        }
    }
}
#endif
