using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
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
        public IEnumerator AuthoringList_ScriptReloadPreservesUnsavedEditsAndImmediateQueries()
        {
            var setting = ScriptableObject.CreateInstance<UIPanelProcedureGeneralSetting>();
            var config = new UIPanelProcedureConfig
            {
                procedureID = ProcedureID,
                uniqueUIPanelAutoOpenOnEnter = new List<string> { "saved_panel" }
            };
            setting.procedureConfigs.Add(config);
            AssetDatabase.CreateAsset(setting, AssetPath);
            AssetDatabase.SaveAssets();
            Assert.That(setting.TryGetProcedureConfig(ProcedureID, out var initializedConfig), Is.True);
            Assert.That(initializedConfig, Is.SameAs(config));

            // An unsaved authoring edit proves the object survives hot reload rather than a disk reload.
            config.uniqueUIPanelAutoOpenOnEnter[0] = "unsaved_panel";
            Assert.That(File.ReadAllText(AssetPath), Does.Contain("saved_panel"));
            Assert.That(File.ReadAllText(AssetPath), Does.Not.Contain("unsaved_panel"));

            EditorUtility.RequestScriptReload();
            yield return new WaitForDomainReload();

            var reloaded = AssetDatabase.LoadAssetAtPath<UIPanelProcedureGeneralSetting>(AssetPath);
            Assert.That(reloaded.TryGetProcedureConfig(ProcedureID, out var authoringConfig), Is.True);
            Assert.That(authoringConfig.uniqueUIPanelAutoOpenOnEnter, Is.EqualTo(new[] { "unsaved_panel" }));
            Assert.That(reloaded.procedureConfigs.Count, Is.EqualTo(1));
        }
    }
}
