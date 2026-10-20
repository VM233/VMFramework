using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using VMFramework.GameEvents;
using VMFramework.GameLogicArchitecture;
using VMFramework.GameLogicArchitecture.Editor;
using FrameworkGameEditor = VMFramework.Editor.GameEditor.GameEditor;

namespace VMFramework.Editor.Tests
{
    public sealed class GamePrefabAuthoringLifecycleTests
    {
        private string directory, path, prefix;
        private GamePrefabSingleWrapper wrapper;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            prefix = "authoring_lifecycle_" + Guid.NewGuid().ToString("N");
            var initialized = typeof(GamePrefabWrapperInitializeUtility).Assembly
                .GetType("VMFramework.Procedure.Editor.EditorInitializer", true)
                .GetProperty("IsInitialized", BindingFlags.Static | BindingFlags.Public);
            double deadline = EditorApplication.timeSinceStartup + 20;
            while (!(bool)initialized.GetValue(null))
            {
                Assert.That(EditorApplication.timeSinceStartup, Is.LessThan(deadline),
                    "The actual Edit initializer did not publish its registry.");
                yield return null;
            }
            directory = "Assets/__" + prefix;
            path = directory + "/Wrapper.asset";
            yield return NativeChange(path, () =>
            {
                AssetDatabase.CreateFolder("Assets", Path.GetFileName(directory));
                wrapper = ScriptableObject.CreateInstance<GamePrefabSingleWrapper>();
                wrapper.InitGamePrefabs(new IGamePrefab[] { new GameEventConfig { id = prefix } });
                AssetDatabase.CreateAsset(wrapper, path);
            });
            Assert.That(GamePrefabManager.TryGetGamePrefab(prefix, out _), Is.True,
                "The real wrapper import did not register its configuration.");
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            if (directory != null && AssetDatabase.IsValidFolder(directory))
                yield return NativeChange(directory, () =>
                    Assert.That(AssetDatabase.DeleteAsset(directory), Is.True));
            Assert.That(GamePrefabManager.AllGamePrefabIDs.Where(id => id.StartsWith(prefix,
                StringComparison.Ordinal)), Is.Empty, "Owned test registrations survived native deletion.");
        }

        [UnityTest]
        public IEnumerator NativeImportRegistersTheSavedConfiguration()
        {
            Assert.That(GamePrefabManager.TryGetGamePrefab(prefix, out var actual), Is.True);
            var configurations = new List<IGamePrefab>();
            AssetDatabase.LoadAssetAtPath<GamePrefabSingleWrapper>(path).GetGamePrefabs(configurations);
            Assert.That(actual, Is.SameAs(configurations.Single()));
            yield break;
        }

        [UnityTest]
        public IEnumerator RepeatedRefreshKeepsExactlyOneRenameSubscription()
        {
            GamePrefabWrapperInitializeUtility.Refresh();
            GamePrefabWrapperInitializeUtility.Refresh();
            GamePrefabWrapperInitializeUtility.Refresh();
            Assert.That(GamePrefabManager.TryGetGamePrefab(prefix, out var actual), Is.True);
            int registrations = 0, retirements = 0;
            void Registered(IGamePrefab value) { if (ReferenceEquals(value, actual)) registrations++; }
            void Retired(IGamePrefab value) { if (ReferenceEquals(value, actual)) retirements++; }
            GamePrefabManager.OnGamePrefabRegisteredEvent += Registered;
            GamePrefabManager.OnGamePrefabUnregisteredEvent += Retired;
            try
            {
                actual.id = prefix + "_renamed";
                Assert.That(registrations, Is.EqualTo(1));
                Assert.That(retirements, Is.EqualTo(1));
                Assert.That(GamePrefabManager.TryGetGamePrefab(prefix, out _), Is.False);
                Assert.That(GamePrefabManager.TryGetGamePrefab(actual.id, out var renamed), Is.True);
                Assert.That(renamed, Is.SameAs(actual));
                yield return NativeChange(path, () =>
                {
                    EditorUtility.SetDirty(wrapper);
                    AssetDatabase.SaveAssetIfDirty(wrapper);
                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                });
                Assert.That(GamePrefabManager.TryGetGamePrefab(prefix + "_renamed", out _), Is.True);
            }
            finally
            {
                GamePrefabManager.OnGamePrefabRegisteredEvent -= Registered;
                GamePrefabManager.OnGamePrefabUnregisteredEvent -= Retired;
            }
        }

        [UnityTest]
        public IEnumerator ReplacedConfigurationCannotRegisterThroughItsRetiredIDCallback()
        {
            Assert.That(GamePrefabManager.TryGetGamePrefab(prefix, out var previous), Is.True);
            yield return NativeChange(path, () =>
            {
                LogAssert.Expect(LogType.Warning,
                    $"GamePrefabSingleWrapper({wrapper.name}) already has a gamePrefab.But it will be replaced with the new gamePrefab.");
                wrapper.InitGamePrefabs(new IGamePrefab[] { new GameEventConfig { id = prefix + "_replacement" } });
                EditorUtility.SetDirty(wrapper);
                AssetDatabase.SaveAssetIfDirty(wrapper);
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            });
            Assert.That(GamePrefabManager.TryGetGamePrefab(prefix, out _), Is.False);
            Assert.That(GamePrefabManager.TryGetGamePrefab(prefix + "_replacement", out _), Is.True);
            previous.id = prefix + "_retired";
            Assert.That(GamePrefabManager.TryGetGamePrefab(previous.id, out _), Is.False);
        }

        [UnityTest]
        public IEnumerator UnrelatedMeshKeepsRegistryAndExistingMenuTree()
        {
            var editor = ExistingEditor();
            var tree = editor.MenuTree;
            int refreshes = 0;
            void Refreshed() => refreshes++;
            GamePrefabWrapperInitializeUtility.OnGamePrefabWrappersRefresh += Refreshed;
            try
            {
                string meshPath = directory + "/Independent Mesh.asset";
                yield return NativeChange(meshPath, () => AssetDatabase.CreateAsset(new Mesh(), meshPath));
                Assert.That(refreshes, Is.Zero, "A Mesh import refreshed the GamePrefab registry.");
                Assert.That(editor.MenuTree, Is.SameAs(tree), "A Mesh import rebuilt an unrelated actual menu.");
                yield return NativeChange(meshPath, () => Assert.That(AssetDatabase.DeleteAsset(meshPath), Is.True));
                Assert.That(refreshes, Is.Zero, "A Mesh deletion refreshed the GamePrefab registry.");
                Assert.That(editor.MenuTree, Is.SameAs(tree));
            }
            finally { GamePrefabWrapperInitializeUtility.OnGamePrefabWrappersRefresh -= Refreshed; }
        }

        [UnityTest]
        public IEnumerator NativeMoveAndFolderDeletionRetireTheKnownSource()
        {
            string moved = directory + "/Moved Wrapper.asset";
            yield return NativeChange(moved, () => Assert.That(AssetDatabase.MoveAsset(path, moved), Is.Empty));
            path = moved;
            Assert.That(GamePrefabManager.TryGetGamePrefab(prefix, out var actual), Is.True);
            yield return NativeChange(directory, () => Assert.That(AssetDatabase.DeleteAsset(directory), Is.True));
            Assert.That(GamePrefabManager.TryGetGamePrefab(prefix, out _), Is.False);
            actual.id = prefix + "_after_delete";
            Assert.That(GamePrefabManager.TryGetGamePrefab(actual.id, out _), Is.False);
        }

        [UnityTest]
        public IEnumerator ActualMenuSourceImportRebuildsTheExistingHost()
        {
            var editor = ExistingEditor();
            var before = editor.MenuTree;
            string actualSource = before.EnumerateTree().Select(item => item.Value)
                .OfType<UnityEngine.Object>().Select(AssetDatabase.GetAssetPath)
                .First(value => !string.IsNullOrEmpty(value));
            yield return NativeChange(actualSource, () => AssetDatabase.ImportAsset(actualSource,
                ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport));
            Assert.That(editor.MenuTree, Is.Not.SameAs(before));
        }

        [UnityTest]
        public IEnumerator CancelRetiresTheActualQueuedNativeImport()
        {
            string secondPath = directory + "/Canceled Wrapper.asset";
            var cancel = typeof(GamePrefabWrapperInitializeUtility).Assembly
                .GetType("VMFramework.GameLogicArchitecture.Editor.GamePrefabWrapperPostProcessor", true)
                .GetMethod("Cancel", BindingFlags.Static | BindingFlags.NonPublic);
            bool canceled = false;
            void CancelPublishedImport(string[] imported, string[] deleted, string[] moved, string[] movedFrom)
            {
                if (!imported.Contains(secondPath)) return;
                cancel.Invoke(null, null);
                canceled = true;
            }
            NativeAuthoringChangeObserver.Changed += CancelPublishedImport;
            try
            {
                yield return NativeChange(secondPath, () =>
                {
                    var second = ScriptableObject.CreateInstance<GamePrefabSingleWrapper>();
                    second.InitGamePrefabs(new IGamePrefab[] { new GameEventConfig { id = prefix + "_canceled" } });
                    AssetDatabase.CreateAsset(second, secondPath);
                });
                Assert.That(canceled, Is.True, "The actual native import was not observed before cancellation.");
                Assert.That(GamePrefabManager.TryGetGamePrefab(prefix + "_canceled", out _), Is.False);
                Assert.That(GamePrefabManager.TryGetGamePrefab(prefix, out _), Is.True);
            }
            finally { NativeAuthoringChangeObserver.Changed -= CancelPublishedImport; }
        }

        private static FrameworkGameEditor ExistingEditor()
        {
            var editors = Resources.FindObjectsOfTypeAll<FrameworkGameEditor>();
            Assert.That(editors, Is.Not.Empty, "This native host regression requires an already-open Game Editor.");
            return editors.First();
        }

        private static IEnumerator NativeChange(string expectedPath, Action mutation)
        {
            bool published = false;
            void Complete() => published = true;
            void Changed(string[] imported, string[] deleted, string[] moved, string[] movedFrom)
            {
                bool Owns(string value) => value == expectedPath || value.StartsWith(expectedPath + "/", StringComparison.Ordinal);
                if (!imported.Concat(deleted).Concat(moved).Concat(movedFrom).Any(Owns)) return;
                EditorApplication.delayCall -= Complete;
                EditorApplication.delayCall += Complete;
            }
            NativeAuthoringChangeObserver.Changed += Changed;
            try
            {
                mutation();
                double deadline = EditorApplication.timeSinceStartup + 20;
                while (!published)
                {
                    Assert.That(EditorApplication.timeSinceStartup, Is.LessThan(deadline),
                        "No completed native import/deletion publication for " + expectedPath);
                    yield return null;
                }
            }
            finally
            {
                NativeAuthoringChangeObserver.Changed -= Changed;
                EditorApplication.delayCall -= Complete;
            }
        }
    }
}
