using System.Linq;
using NUnit.Framework;
using UnityEngine;
using VMCommonPreset.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Tests
{
    public sealed class CommonPresetGameEditorMenuTests
    {
        [Test]
        public void CoreRuntimeMenuDisplaysConfiguredCommonPresetAssetsInOrder()
        {
            var coreSettingFile = ScriptableObject.CreateInstance<CoreSettingFile>();
            try
            {
                var commonPresetMenu = ((IGameEditorMenuTreeNodesProvider)coreSettingFile)
                    .GetAllMenuTreeNodes()
                    .OfType<IGameEditorMenuTreeNodesProvider>()
                    .Single(node => node.Name == "Common Presets");
                var menuItems = commonPresetMenu.GetAllMenuTreeNodes().ToArray();
                var configuredPresets = CommonPresetProjectSettings.Presets
                    .Where(entry => entry?.Preset != null)
                    .ToArray();

                Assert.That(menuItems, Has.Length.EqualTo(configuredPresets.Length));
                for (var index = 0; index < configuredPresets.Length; index++)
                {
                    Assert.That(menuItems[index], Is.InstanceOf<IGameEditorMenuTreeNode>());
                    Assert.That(menuItems[index], Is.InstanceOf<INameOwner>());
                    Assert.That(((INameOwner)menuItems[index]).Name, Is.EqualTo(configuredPresets[index].Key));
                    Assert.That(((IGameEditorMenuTreeNode)menuItems[index]).VisualNode,
                        Is.SameAs(configuredPresets[index].Preset));
                }
            }
            finally
            {
                Object.DestroyImmediate(coreSettingFile);
            }
        }
    }
}
