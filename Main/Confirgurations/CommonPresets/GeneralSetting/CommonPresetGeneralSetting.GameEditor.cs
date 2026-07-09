#if UNITY_EDITOR
using System.Collections.Generic;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Configuration 
{
    public partial class CommonPresetGeneralSetting : IGameEditorMenuTreeNode, IGameEditorMenuTreeNodesProvider
    {
        string INameOwner.Name => "Common Priority";

        EditorIcon IEditorIconProvider.Icon => EditorIcon.None;

        IEnumerable<object> IGameEditorMenuTreeNodesProvider.GetAllMenuTreeNodes()
        {
            foreach (var preset in presets.Values)
            {
                yield return preset;
            }
        }
    }
}
#endif