using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using VMFramework.GameEvents;
using VMFramework.GameLogicArchitecture;
using VMFramework.GameLogicArchitecture.Editor;

namespace VMFramework.Editor.Tests
{
    public sealed class GamePrefabNativeSerializationTests
    {
        private const string TestFolder =
            "Assets/__VMFrameworkGamePrefabNativeSerializationTests";

        [Serializable]
        private abstract class TestPayload
        {
            public string label;
        }

        [Serializable]
        private sealed class TestPayloadImplementation : TestPayload
        {
            public Vector3 offset;
        }

        [Serializable]
        private sealed class TestGamePrefab : GamePrefab
        {
            public GameObject prefabReference;

            public List<int> values = new();

            [SerializeReference]
            public TestPayload payload;
        }

        [SetUp]
        public void SetUp()
        {
            AssetDatabase.DeleteAsset(TestFolder);
            AssetDatabase.CreateFolder("Assets",
                "__VMFrameworkGamePrefabNativeSerializationTests");
        }

        [TearDown]
        public void TearDown()
        {
            AssetDatabase.DeleteAsset(TestFolder);
        }

        [Test]
        public void WrapperContract_UsesUnityManagedReferenceStorage()
        {
            Assert.That(typeof(GamePrefabWrapper).BaseType,
                Is.EqualTo(typeof(ScriptableObject)));
            AssertSerializeReferenceField(typeof(GamePrefabSingleWrapper),
                "gamePrefab", typeof(IGamePrefab));
            AssertSerializeReferenceField(typeof(GamePrefabMultipleWrapper),
                "gamePrefabs", typeof(List<IGamePrefab>));
        }

        [Test]
        public void AllLoadedGamePrefabConfigTypes_AreSerializable()
        {
            Type[] nonSerializableTypes = TypeCache
                .GetTypesDerivedFrom<GamePrefab>()
                .Append(typeof(GamePrefab))
                .Where(type => type.IsDefined(typeof(SerializableAttribute),
                    false) == false)
                .OrderBy(type => type.FullName, StringComparer.Ordinal)
                .ToArray();

            Assert.That(nonSerializableTypes, Is.Empty,
                "Every GamePrefab config type in the loaded project must " +
                "declare [Serializable].");
        }

        [Test]
        public void SingleWrapper_RoundTripsManagedReferenceWithoutOdinPayload()
        {
            const string path = TestFolder + "/Single.asset";
            var wrapper = ScriptableObject.CreateInstance<GamePrefabSingleWrapper>();
            wrapper.InitGamePrefabs(new IGamePrefab[]
            {
                new GameEventConfig
                {
                    id = "native_single_event",
                    gameTags = new List<string> { "alpha", "beta" }
                }
            });

            GamePrefabWrapper reloaded = SaveAndReload(wrapper, path);
            var gamePrefabs = new List<IGamePrefab>();
            reloaded.GetGamePrefabs(gamePrefabs);

            GameEventConfig config = gamePrefabs.OfType<GameEventConfig>().Single();
            Assert.That(config.id, Is.EqualTo("native_single_event"));
            Assert.That(config.gameTags,
                Is.EqualTo(new[] { "alpha", "beta" }));
            AssertNativeYaml(path);
        }

        [Test]
        public void MultipleWrapper_RoundTripsOrderTagsAndInputActionId()
        {
            const string path = TestFolder + "/Multiple.asset";
            Guid inputActionID = Guid.NewGuid();
            var wrapper = ScriptableObject.CreateInstance<GamePrefabMultipleWrapper>();
            wrapper.InitGamePrefabs(new IGamePrefab[]
            {
                new GameEventConfig
                {
                    id = "native_first_event",
                    gameTags = new List<string> { "first" }
                },
                new InputSystemGameEventConfig
                {
                    id = "native_input_event",
                    gameTags = new List<string> { "input" },
                    inputActionID = inputActionID
                }
            });

            GamePrefabWrapper reloaded = SaveAndReload(wrapper, path);
            var gamePrefabs = new List<IGamePrefab>();
            reloaded.GetGamePrefabs(gamePrefabs);

            Assert.That(gamePrefabs.Select(gamePrefab => gamePrefab.id),
                Is.EqualTo(new[] { "native_first_event", "native_input_event" }));
            var inputConfig =
                (InputSystemGameEventConfig)gamePrefabs[1];
            Assert.That(inputConfig.inputActionID, Is.EqualTo(inputActionID));
            Assert.That(inputConfig.gameTags,
                Is.EqualTo(new[] { "input" }));
            AssertNativeYaml(path);
        }

        [Test]
        public void NestedGraph_RoundTripsUnityObjectAndManagedReferences()
        {
            const string prefabPath = TestFolder + "/Reference.prefab";
            const string wrapperPath = TestFolder + "/Nested.asset";
            var root = new GameObject("Native Reference");
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root,
                prefabPath);
            UnityEngine.Object.DestroyImmediate(root);

            var wrapper = ScriptableObject
                .CreateInstance<GamePrefabSingleWrapper>();
            wrapper.InitGamePrefabs(new IGamePrefab[]
            {
                new TestGamePrefab
                {
                    id = "native_nested_event",
                    gameTags = new List<string> { "nested", "reference" },
                    prefabReference = prefab,
                    values = new List<int> { 3, 5, 8 },
                    payload = new TestPayloadImplementation
                    {
                        label = "payload",
                        offset = new Vector3(1.25f, -2.5f, 4.75f)
                    }
                }
            });

            GamePrefabWrapper reloaded = SaveAndReload(wrapper, wrapperPath);
            TestGamePrefab config = GetSingleGamePrefab<TestGamePrefab>(
                reloaded);

            Assert.That(config.gameTags,
                Is.EqualTo(new[] { "nested", "reference" }));
            Assert.That(config.values, Is.EqualTo(new[] { 3, 5, 8 }));
            Assert.That(AssetDatabase.GetAssetPath(config.prefabReference),
                Is.EqualTo(prefabPath));
            Assert.That(config.payload,
                Is.TypeOf<TestPayloadImplementation>());
            Assert.That(config.payload.label, Is.EqualTo("payload"));
            Assert.That(((TestPayloadImplementation)config.payload).offset,
                Is.EqualTo(new Vector3(1.25f, -2.5f, 4.75f)));
            AssertNativeYaml(wrapperPath);
        }

        [Test]
        public void CreatorAndSecondSave_PreserveNativeGraphMutations()
        {
            const string path = TestFolder + "/Created.asset";
            Guid initialActionID = Guid.NewGuid();
            GamePrefabWrapperCreator.CreateGamePrefabWrapper(path,
                GamePrefabWrapperType.Single,
                new InputSystemGameEventConfig
                {
                    id = "native_created_event",
                    gameTags = new List<string> { "created" },
                    inputActionID = initialActionID
                });

            GamePrefabWrapper reloaded = Reload(path);
            InputSystemGameEventConfig config =
                GetSingleGamePrefab<InputSystemGameEventConfig>(reloaded);
            Assert.That(config.inputActionID, Is.EqualTo(initialActionID));

            Guid updatedActionID = Guid.NewGuid();
            config.gameTags.Add("updated");
            config.inputActionID = updatedActionID;
            EditorUtility.SetDirty(reloaded);
            AssetDatabase.SaveAssets();

            reloaded = Reload(path);
            config = GetSingleGamePrefab<InputSystemGameEventConfig>(reloaded);
            Assert.That(config.inputActionID, Is.EqualTo(updatedActionID));
            Assert.That(config.gameTags,
                Is.EqualTo(new[] { "created", "updated" }));
            AssertNativeYaml(path);
        }

        [Test]
        public void DiscoveredProjectWrappers_HaveValidNativeGraphs()
        {
            string[] wrapperPaths = AssetDatabase
                .FindAssets($"t:{nameof(GamePrefabSingleWrapper)}")
                .Concat(AssetDatabase.FindAssets(
                    $"t:{nameof(GamePrefabMultipleWrapper)}"))
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(path => path.StartsWith(TestFolder,
                    StringComparison.Ordinal) == false)
                .Distinct(StringComparer.Ordinal)
                .OrderBy(path => path, StringComparer.Ordinal)
                .ToArray();
            HashSet<Guid> inputActionIDs = GetInputActionIDs();
            var gamePrefabIDs = new HashSet<string>(StringComparer.Ordinal);
            var issues = new List<string>();
            int gamePrefabCount = 0;
            int tagCount = 0;
            int inputEventCount = 0;

            foreach (string path in wrapperPaths)
            {
                GamePrefabWrapper wrapper =
                    AssetDatabase.LoadAssetAtPath<GamePrefabWrapper>(path);
                if (wrapper == null)
                {
                    issues.Add($"{path}: failed to load wrapper.");
                    continue;
                }

                var gamePrefabs = new List<IGamePrefab>();
                wrapper.GetGamePrefabs(gamePrefabs);
                if (gamePrefabs.Count == 0)
                {
                    issues.Add($"{path}: wrapper has no GamePrefab graph.");
                }

                foreach (IGamePrefab gamePrefab in gamePrefabs)
                {
                    gamePrefabCount++;
                    ValidateGamePrefab(path, gamePrefab, inputActionIDs,
                        gamePrefabIDs, issues, ref tagCount,
                        ref inputEventCount);
                }

                ValidateNativeYaml(path, issues);
            }

            TestContext.Progress.WriteLine(
                $"Validated {wrapperPaths.Length} wrappers, " +
                $"{gamePrefabCount} GamePrefabs, {tagCount} tags, and " +
                $"{inputEventCount} Input System events.");
            Assert.That(issues, Is.Empty,
                string.Join(Environment.NewLine, issues));
        }

        private static GamePrefabWrapper SaveAndReload(
            GamePrefabWrapper wrapper, string path)
        {
            AssetDatabase.CreateAsset(wrapper, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.ForceReserializeAssets(new[] { path });
            return Reload(path);
        }

        private static GamePrefabWrapper Reload(string path)
        {
            GamePrefabWrapper loaded =
                AssetDatabase.LoadAssetAtPath<GamePrefabWrapper>(path);
            if (loaded != null)
            {
                Resources.UnloadAsset(loaded);
            }

            AssetDatabase.ImportAsset(path,
                ImportAssetOptions.ForceSynchronousImport |
                ImportAssetOptions.ForceUpdate);
            return AssetDatabase.LoadAssetAtPath<GamePrefabWrapper>(path);
        }

        private static TGamePrefab GetSingleGamePrefab<TGamePrefab>(
            GamePrefabWrapper wrapper)
            where TGamePrefab : class, IGamePrefab
        {
            var gamePrefabs = new List<IGamePrefab>();
            wrapper.GetGamePrefabs(gamePrefabs);
            return gamePrefabs.OfType<TGamePrefab>().Single();
        }

        private static void AssertSerializeReferenceField(Type ownerType,
            string fieldName, Type expectedFieldType)
        {
            FieldInfo field = ownerType.GetField(fieldName,
                BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(field, Is.Not.Null);
            Assert.That(field.FieldType, Is.EqualTo(expectedFieldType));
            Assert.That(field.IsDefined(typeof(SerializeReference), false),
                Is.True);
        }

        private static HashSet<Guid> GetInputActionIDs()
        {
            var result = new HashSet<Guid>();
            foreach (string guid in AssetDatabase.FindAssets(
                         $"t:{nameof(InputActionAsset)}"))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                InputActionAsset asset =
                    AssetDatabase.LoadAssetAtPath<InputActionAsset>(path);
                if (asset == null)
                {
                    continue;
                }

                foreach (InputActionMap actionMap in asset.actionMaps)
                {
                    foreach (InputAction action in actionMap.actions)
                    {
                        result.Add(action.id);
                    }
                }
            }

            return result;
        }

        private static void ValidateGamePrefab(string path,
            IGamePrefab gamePrefab, ISet<Guid> inputActionIDs,
            ISet<string> gamePrefabIDs, ICollection<string> issues,
            ref int tagCount, ref int inputEventCount)
        {
            if (gamePrefab == null)
            {
                issues.Add($"{path}: graph contains a null GamePrefab.");
                return;
            }

            Type type = gamePrefab.GetType();
            if (type.IsDefined(typeof(SerializableAttribute), false) == false)
            {
                issues.Add($"{path}: {type.FullName} is not [Serializable].");
            }

            if (string.IsNullOrWhiteSpace(gamePrefab.id))
            {
                issues.Add($"{path}: {type.FullName} has an empty ID.");
            }
            else if (gamePrefabIDs.Add(gamePrefab.id) == false)
            {
                issues.Add($"{path}: duplicate GamePrefab ID " +
                           $"'{gamePrefab.id}'.");
            }

            if (gamePrefab.GameTags == null)
            {
                issues.Add($"{path}: {gamePrefab.id} has null GameTags.");
            }
            else
            {
                tagCount += gamePrefab.GameTags.Count;
                if (gamePrefab.GameTags.Any(string.IsNullOrWhiteSpace))
                {
                    issues.Add($"{path}: {gamePrefab.id} has an empty tag.");
                }

                if (gamePrefab.GameTags.Distinct(StringComparer.Ordinal)
                        .Count() != gamePrefab.GameTags.Count)
                {
                    issues.Add($"{path}: {gamePrefab.id} has duplicate tags.");
                }
            }

            if (gamePrefab is not InputSystemGameEventConfig inputConfig)
            {
                return;
            }

            inputEventCount++;
            if (inputConfig.inputActionID == Guid.Empty)
            {
                issues.Add($"{path}: {gamePrefab.id} has an empty " +
                           "Input Action ID.");
            }
            else if (inputActionIDs.Contains(inputConfig.inputActionID) ==
                     false)
            {
                issues.Add($"{path}: {gamePrefab.id} references missing " +
                           $"Input Action {inputConfig.inputActionID}.");
            }
        }

        private static void ValidateNativeYaml(string path,
            ICollection<string> issues)
        {
            string yaml = File.ReadAllText(path);
            if (yaml.Contains("  references:") == false)
            {
                issues.Add($"{path}: managed-reference section is missing.");
            }

            if (yaml.Contains("    RefIds: []"))
            {
                issues.Add($"{path}: managed-reference graph is empty.");
            }

            if (yaml.Contains("serializationData:"))
            {
                issues.Add($"{path}: legacy Odin payload is still present.");
            }
        }

        private static void AssertNativeYaml(string path)
        {
            var issues = new List<string>();
            ValidateNativeYaml(path, issues);
            Assert.That(issues, Is.Empty,
                string.Join(Environment.NewLine, issues));
        }
    }
}
