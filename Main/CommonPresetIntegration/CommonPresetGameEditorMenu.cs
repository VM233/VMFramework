#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMCommonPreset.Editor;
using VMFramework.Configuration;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;

namespace VMFramework.GameLogicArchitecture
{
    public sealed partial class CoreSettingFile
    {
        protected override IEnumerable<IGameEditorMenuTreeNode> GetAllMenuTreeNodes()
        {
            foreach (var node in base.GetAllMenuTreeNodes())
            {
                yield return node;
            }

            yield return new CommonPresetGameEditorMenu();
        }
    }

    internal sealed class CommonPresetGameEditorMenu : IGameEditorMenuTreeNode, IGameEditorMenuTreeNodesProvider
    {
        string INameOwner.Name => "Common Presets";

        EditorIcon IEditorIconProvider.Icon => SdfIconType.Collection;

        IEnumerable<object> IGameEditorMenuTreeNodesProvider.GetAllMenuTreeNodes()
        {
            foreach (var entry in CommonPresetProjectSettings.Presets)
            {
                if (entry?.Preset == null)
                {
                    continue;
                }

                yield return new CommonPresetGameEditorMenuItem(entry.Key, entry.Preset);
            }
        }
    }

    internal sealed class CommonPresetGameEditorMenuItem : IGameEditorMenuTreeNode
    {
        private readonly string key;
        private readonly CommonPreset preset;

        public CommonPresetGameEditorMenuItem(string key, CommonPreset preset)
        {
            this.key = key;
            this.preset = preset;
        }

        string INameOwner.Name => key;

        object IGameEditorMenuTreeNode.VisualNode => preset;
    }
}
#endif
