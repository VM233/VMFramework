using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine.TestTools;
using VMFramework.Procedure;
using VMFramework.Procedure.Editor;

namespace VMFramework.Tests
{
    public sealed class EditorInitializationBuildBoundaryTests
    {
        private static readonly Type Owner = typeof(InitializerManager).Assembly.GetType(
            "VMFramework.Procedure.Editor.EditorInitializer", true);
        private static readonly BindingFlags Flags = BindingFlags.Static | BindingFlags.NonPublic;
        private static InitializerManager Current => (InitializerManager)Owner.GetField("initializerManager", Flags).GetValue(null);
        private static void Invoke(string name) => Owner.GetMethod(name, Flags).Invoke(null, null);

        [UnityTest]
        public IEnumerator NativeBuildBoundaryCancelsCurrentWorkAndAdmitsOneFreshInitialization()
        {
            yield return Until(() => Current is { IsInitializing: false });
            BuildBoundaryPendingInitializer.Pending = new UniTaskCompletionSource();
            BuildBoundaryPendingInitializer.Entered = false;
            BuildBoundaryPendingInitializer.Enabled = true;
            try
            {
                Invoke("StartInitialization");
                yield return Until(() => BuildBoundaryPendingInitializer.Entered);
                var prior = Current;
                var cancellation = (CancellationTokenSource)Owner.GetField("initializationCancellation", Flags).GetValue(null);
                var token = cancellation.Token;
                var boundaryType = Owner.Assembly.GetType(
                    "VMFramework.Procedure.Editor.EditorInitializationBuildBoundary", true);
                var boundary = (IPreprocessBuildWithReport)Activator.CreateInstance(boundaryType, true);
                BuildBoundaryPendingInitializer.Enabled = false;
                boundary.OnPreprocessBuild(null);
                Assert.That(token.IsCancellationRequested, Is.True);
                yield return Until(() => !prior.IsInitializing);
                Assert.That(prior.LastException, Is.InstanceOf<OperationCanceledException>());
                Assert.That(prior.IsInitialized, Is.False);
                yield return Until(() => Current is { IsInitialized: true });
                Assert.That(Current, Is.Not.SameAs(prior));
            }
            finally
            {
                BuildBoundaryPendingInitializer.Enabled = false;
                BuildBoundaryPendingInitializer.Pending.TrySetResult();
                Invoke("CancelInitialization");
                if (Current is not { IsInitialized: true }) Invoke("ScheduleInitialize");
            }
        }

        [UnityTearDown]
        public IEnumerator JoinOwnerRetirement()
        {
            yield return Until(() => Current is { IsInitialized: true, IsInitializing: false });
            BuildBoundaryPendingInitializer.Pending = null;
        }

        private static IEnumerator Until(Func<bool> completed)
        {
            var started = EditorApplication.timeSinceStartup;
            while (!completed())
            {
                Assert.That(EditorApplication.timeSinceStartup - started, Is.LessThan(30),
                    "The actual Editor initialization lifetime did not retire.");
                yield return null;
            }
        }
    }
}
