using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.GameEvents;
using VMFramework.GameLogicArchitecture;
using VMFramework.Maps;
using VMFramework.UI;
using Assert = NUnit.Framework.Assert;

namespace VMFramework.Editor.Tests
{
    public sealed class SettingsNativeSerializationTests
    {
        private string directory;

        [SetUp]
        public void SetUp()
        {
            directory = "Assets/__NativeSettings_" + Guid.NewGuid().ToString("N");
            AssetDatabase.CreateFolder("Assets", Path.GetFileName(directory));
        }

        [TearDown]
        public void TearDown() => AssetDatabase.DeleteAsset(directory);

        [Test]
        public void ProvidersAndDependencyGraph_RoundTripAndRemainEditable()
        {
            var wrapper = ScriptableObject.CreateInstance<GamePrefabSingleWrapper>();
            wrapper.InitGamePrefabs(new IGamePrefab[] { new GameEventConfig { id = "native_event" } });
            AssetDatabase.CreateAsset(wrapper, directory + "/Provider.asset");
            var setting = ScriptableObject.CreateInstance<GameEventGeneralSetting>();
            AssetDatabase.CreateAsset(setting, directory + "/Events.asset");
            setting.AddToInitialGamePrefabProviders(wrapper);
            var child = new GameEventDependencyNode { gameEventID = "child" };
            setting.dependencyNodes.Add(new GameEventDependencyNode
            {
                gameEventID = "root", children = new List<GameEventDependencyNode> { child }
            });
            setting.dependencyNodes.Add(child);
            setting.autoEnableActions = false;

            setting = SaveAndReload(setting);
            Assert.That(setting.initialGamePrefabProviders.Single(), Is.EqualTo(wrapper));
            Assert.That(setting.dependencyNodes[0].children[0], Is.SameAs(setting.dependencyNodes[1]));
            var prefabs = new List<IGamePrefab>();
            setting.GetGamePrefabs(prefabs);
            Assert.That(prefabs.Single().id, Is.EqualTo("native_event"));

            setting.dependencyNodes[1].gameEventID = "changed_child";
            setting = SaveAndReload(setting);
            Assert.That(setting.dependencyNodes[0].children[0].gameEventID, Is.EqualTo("changed_child"));
            Assert.That(setting.autoEnableActions, Is.False);
        }

        [Test]
        public void DictionaryConfigs_RoundTripThenInitializeFromAuthoringState()
        {
            var setting = ScriptableObject.CreateInstance<UIPanelProcedureGeneralSetting>();
            AssetDatabase.CreateAsset(setting, directory + "/Procedures.asset");
            setting.procedureConfigs.TryAddConfigEditor(new UIPanelProcedureConfig
            {
                procedureID = "native_procedure", uniqueUIPanelAutoOpenOnEnter = new List<string> { "panel_a" }
            });
            setting = SaveAndReload(setting);
            Assert.That(setting.procedureConfigs.InitDone, Is.False);
            setting.procedureConfigs.Init();
            Assert.That(setting.procedureConfigs.GetConfigRuntime("native_procedure").uniqueUIPanelAutoOpenOnEnter,
                Is.EqualTo(new[] { "panel_a" }));
            setting.procedureConfigs.Init();
            Assert.That(setting.procedureConfigs.GetRuntimeDictionary().Count, Is.EqualTo(1));
        }

        [Test]
        public void TypeFilters_RoundTripConfiguredTypes()
        {
            var filter = ScriptableObject.CreateInstance<GeneralObjectFilter>();
            AssetDatabase.CreateAsset(filter, directory + "/Filter.asset");
            filter.typeFilter = new TypeFilter
            {
                type = typeof(string), isMultiple = true,
                types = new SerializableType[] { typeof(string), typeof(int) }
            };
            filter.componentTypeFilter = new ComponentTypeFilter { type = typeof(Transform) };
            filter = SaveAndReload(filter);
            Assert.That(filter.typeFilter.type.Value, Is.EqualTo(typeof(string)));
            Assert.That(filter.typeFilter.types.Select(value => value.Value), Is.EqualTo(new[] { typeof(string), typeof(int) }));
            Assert.That(filter.componentTypeFilter.type.Value, Is.EqualTo(typeof(Transform)));
            Assert.That(filter.typeFilter.IsMatch("matches"), Is.True);
            Assert.That(filter.typeFilter.IsMatch(12.5f), Is.False);
        }

        [Test]
        public void GridMapConfig_RoundTripsBoundsAndAxes()
        {
            var setting = ScriptableObject.CreateInstance<GridMapGeneralSetting>();
            AssetDatabase.CreateAsset(setting, directory + "/Grid.asset");
            setting.defaultDynamicGridMapConfig = new DynamicGridMapConfig(new Vector3Int(16, 8, 4),
                new CubeInteger(new Vector3Int(-3, -2, -1), new Vector3Int(5, 6, 7))
                { inverseX = true, inverseZ = true });
            setting = SaveAndReload(setting);
            Assert.That(setting.defaultDynamicGridMapConfig.chunkSize, Is.EqualTo(new Vector3Int(16, 8, 4)));
            Assert.That(setting.defaultDynamicGridMapConfig.chunkBounds.min, Is.EqualTo(new Vector3Int(-3, -2, -1)));
            Assert.That(setting.defaultDynamicGridMapConfig.chunkBounds.max, Is.EqualTo(new Vector3Int(5, 6, 7)));
            Assert.That(setting.defaultDynamicGridMapConfig.chunkBounds.inverseX, Is.True);
            Assert.That(setting.defaultDynamicGridMapConfig.chunkBounds.inverseY, Is.False);
            Assert.That(setting.defaultDynamicGridMapConfig.chunkBounds.inverseZ, Is.True);
        }

        private static T SaveAndReload<T>(T asset) where T : ScriptableObject
        {
            string path = AssetDatabase.GetAssetPath(asset);
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssetIfDirty(asset);
            Resources.UnloadAsset(asset);
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
            Assert.That(File.ReadAllText(path), Does.Not.Contain("serializationData:"));
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }
    }
}
