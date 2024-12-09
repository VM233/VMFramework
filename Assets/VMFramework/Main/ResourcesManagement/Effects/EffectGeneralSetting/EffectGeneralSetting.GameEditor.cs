#if UNITY_EDITOR
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.ResourcesManagement 
{
    public partial class EffectGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Effect";

        Icon IGameEditorMenuTreeNode.Icon => Icon.None;
    }
}
#endif