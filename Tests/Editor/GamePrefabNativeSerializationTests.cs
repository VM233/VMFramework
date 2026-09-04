using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using VMFramework.GameEvents;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Editor.Tests
{
    public sealed class GamePrefabNativeSerializationTests
    {
        private const string TestFolder =
            "Assets/__VMFrameworkGamePrefabNativeSerializationTests";

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

        private static GamePrefabWrapper SaveAndReload(
            GamePrefabWrapper wrapper, string path)
        {
            AssetDatabase.CreateAsset(wrapper, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.ForceReserializeAssets(new[] { path });
            Resources.UnloadAsset(wrapper);
            AssetDatabase.ImportAsset(path,
                ImportAssetOptions.ForceSynchronousImport |
                ImportAssetOptions.ForceUpdate);
            return AssetDatabase.LoadAssetAtPath<GamePrefabWrapper>(path);
        }

        private static void AssertNativeYaml(string path)
        {
            string yaml = File.ReadAllText(path);
            Assert.That(yaml, Does.Contain("  references:"));
            Assert.That(yaml, Does.Not.Contain("    RefIds: []"));
            Assert.That(yaml, Does.Not.Contain("serializationData:"));
        }
    }
}
