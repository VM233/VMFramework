using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using VMFramework.Configuration;
using VMFramework.UI;

namespace VMFramework.Editor.Tests
{
    public sealed class ConfigurationScriptReloadTests
    {
        private const string TestFolder = "Assets/__VMFrameworkConfigurationReloadTests";
        private const string AssetPath = TestFolder + "/Procedures.asset";
        private const string ProcedureID = "reload_procedure";

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            Assert.That(AssetDatabase.IsValidFolder(TestFolder), Is.False);
            AssetDatabase.CreateFolder("Assets", "__VMFrameworkConfigurationReloadTests");
            yield break;
        }

        [TearDown]
        public void TearDown() => AssetDatabase.DeleteAsset(TestFolder);

        [UnityTest]
        public IEnumerator InitializedDictionary_ScriptReloadReturnsToAuthoringState()
        {
            var setting = ScriptableObject.CreateInstance<UIPanelProcedureGeneralSetting>();
            var config = new UIPanelProcedureConfig
            {
                procedureID = ProcedureID,
                uniqueUIPanelAutoOpenOnEnter = new List<string> { "saved_panel" }
            };
            setting.procedureConfigs.TryAddConfigEditor(config);
            AssetDatabase.CreateAsset(setting, AssetPath);
            AssetDatabase.SaveAssets();
            setting.procedureConfigs.Init();
            Assert.That(setting.procedureConfigs.InitDone, Is.True);
            Assert.That(setting.procedureConfigs.GetConfigRuntime(ProcedureID), Is.SameAs(config));

            // An unsaved authoring edit proves the object survives hot reload rather than a disk reload.
            config.uniqueUIPanelAutoOpenOnEnter[0] = "unsaved_panel";
            Assert.That(File.ReadAllText(AssetPath), Does.Contain("saved_panel"));
            Assert.That(File.ReadAllText(AssetPath), Does.Not.Contain("unsaved_panel"));

            EditorUtility.RequestScriptReload();
            yield return new WaitForDomainReload();

            var reloaded = AssetDatabase.LoadAssetAtPath<UIPanelProcedureGeneralSetting>(AssetPath);
            Assert.That(reloaded.procedureConfigs.InitDone, Is.False);
            Assert.That(reloaded.procedureConfigs.TryGetConfig(ProcedureID, out var authoringConfig), Is.True);
            Assert.That(authoringConfig.uniqueUIPanelAutoOpenOnEnter, Is.EqualTo(new[] { "unsaved_panel" }));
            reloaded.procedureConfigs.Init();
            Assert.That(reloaded.procedureConfigs.GetConfigRuntime(ProcedureID), Is.SameAs(authoringConfig));
            reloaded.procedureConfigs.Init();
            Assert.That(reloaded.procedureConfigs.GetRuntimeDictionary().Count, Is.EqualTo(1));
        }
    }
}
