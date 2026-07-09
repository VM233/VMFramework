#if UNITY_EDITOR
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Configuration 
{
    public partial class GeneralConfigGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "General Config";

        EditorIcon IEditorIconProvider.Icon => EditorIcon.None;
    }
}
#endif