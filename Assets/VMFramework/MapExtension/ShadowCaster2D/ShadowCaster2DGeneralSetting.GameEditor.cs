#if UNITY_EDITOR
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Maps 
{
    public partial class ShadowCaster2DGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Shadow Caster 2D";

        Icon IGameEditorMenuTreeNode.Icon => Icon.None;
    }
}
#endif