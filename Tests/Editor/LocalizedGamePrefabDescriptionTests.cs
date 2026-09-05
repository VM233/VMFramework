using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Editor.Tests
{
    public sealed class LocalizedGamePrefabDescriptionTests
    {
        private const string TestFolder = "Assets/__VMFrameworkLocalizedDescriptionTests";
        private const string DescriptionType = "Default";

        [Serializable]
        private sealed class TestConfig : LocalizedGamePrefab
        {
        }

        private sealed class TestItem : GameItem
        {
        }

        private GameObject host;
        private string registeredID;

        [SetUp]
        public void SetUp()
        {
            Assert.That(AssetDatabase.IsValidFolder(TestFolder), Is.False);
            AssetDatabase.CreateFolder("Assets", "__VMFrameworkLocalizedDescriptionTests");
        }

        [TearDown]
        public void TearDown()
        {
            if (host != null)
            {
                UnityEngine.Object.DestroyImmediate(host);
            }

            if (registeredID != null)
            {
                GamePrefabManager.UnregisterGamePrefab(registeredID);
                registeredID = null;
            }

            AssetDatabase.DeleteAsset(TestFolder);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void DisabledDescription_NativeEmptyObjectIsNotPublished(bool copyDescription)
        {
            TestConfig config = SaveAndReload(false, null);
            Assert.That(config.description, Is.Not.Null,
                "Native inline serialization materializes the originally null LocalizedString.");
            Assert.That(config.description.IsEmpty, Is.True);

            DescriptionManager manager = RegisterDescription(config, copyDescription);

            Assert.That(manager.GetLocalizedStrings(), Is.Empty);
            Assert.That(manager.TryGetDescription(DescriptionType, out string description), Is.False);
            Assert.That(description, Is.Null);
            Assert.That(((IDescriptionOwner)config).Description, Is.Null);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void DisabledDescription_RetainedAuthoringReferenceIsNotPublished(bool copyDescription)
        {
            TestConfig config = SaveAndReload(false, CreateDescription());
            Assert.That(config.description.IsEmpty, Is.False);

            DescriptionManager manager = RegisterDescription(config, copyDescription);

            Assert.That(manager.GetLocalizedStrings(), Is.Empty);
            Assert.That(manager.TryGetDescription(DescriptionType, out string description), Is.False);
            Assert.That(description, Is.Null);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void EnabledDescription_PreservesReferenceAndCopyOwnership(bool copyDescription)
        {
            TestConfig config = SaveAndReload(true, CreateDescription());
            DescriptionManager manager = RegisterDescription(config, copyDescription);
            LocalizedString published = manager.GetLocalizedStrings().Single();

            Assert.That(published.TableReference, Is.EqualTo(config.description.TableReference));
            Assert.That(published.TableEntryReference, Is.EqualTo(config.description.TableEntryReference));
            Assert.That(ReferenceEquals(published, config.description), Is.EqualTo(!copyDescription));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void EnabledDescription_EmptyReferenceStillExposesInvalidConfiguration(bool copyDescription)
        {
            TestConfig config = SaveAndReload(true, null);
            DescriptionManager manager = RegisterDescription(config, copyDescription);

            Assert.That(manager.GetLocalizedStrings().Count, Is.EqualTo(1));
            Assert.Throws<ArgumentException>(() =>
                manager.TryGetDescription(DescriptionType, out _));
        }

        private static LocalizedString CreateDescription()
        {
            return new LocalizedString("Description Test Table", "DescriptionTestEntry");
        }

        private static TestConfig SaveAndReload(bool hasDescription, LocalizedString description)
        {
            const string path = TestFolder + "/Description.asset";
            var wrapper = ScriptableObject.CreateInstance<GamePrefabSingleWrapper>();
            wrapper.InitGamePrefabs(new IGamePrefab[]
            {
                new TestConfig
                {
                    id = "description_test_" + Guid.NewGuid().ToString("N"),
                    hasDescription = hasDescription,
                    description = description
                }
            });

            AssetDatabase.CreateAsset(wrapper, path);
            AssetDatabase.SaveAssets();
            Resources.UnloadAsset(wrapper);
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

            var reloaded = AssetDatabase.LoadAssetAtPath<GamePrefabSingleWrapper>(path);
            var configs = new List<IGamePrefab>();
            reloaded.GetGamePrefabs(configs);
            return (TestConfig)configs.Single();
        }

        private DescriptionManager RegisterDescription(TestConfig config, bool copyDescription)
        {
            Assert.That(GamePrefabManager.RegisterGamePrefab(config), Is.True);
            registeredID = config.id;
            var item = new TestItem();
            ((ICreatablePoolItem<string>)item).OnCreate(config.id);

            host = new GameObject("Localized Description Test");
            host.SetActive(false);
            var manager = host.AddComponent<DescriptionManager>();
            var register = host.AddComponent<GamePrefabDescriptionRegister>();
            register.type = DescriptionType;
            register.copyDescription = copyDescription;

            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            Type registerType = typeof(GamePrefabDescriptionRegister);
            registerType.GetField("gameItem", flags).SetValue(register, item);
            registerType.GetField("descriptionManager", flags).SetValue(register, manager);
            var generate = (DescriptionManager.GenerateHandler)Delegate.CreateDelegate(
                typeof(DescriptionManager.GenerateHandler), register,
                registerType.GetMethod("Generate", flags));
            manager.Register(DescriptionType, generate);
            var onGet = (Action<IPoolEventProvider>)Delegate.CreateDelegate(
                typeof(Action<IPoolEventProvider>), register,
                registerType.GetMethod("OnGet", flags));

            onGet(null);
            onGet(null);
            return manager;
        }
    }
}
