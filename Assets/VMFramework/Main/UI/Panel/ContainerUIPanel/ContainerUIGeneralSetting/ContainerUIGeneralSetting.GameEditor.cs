#if UNITY_EDITOR
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI 
{
    public partial class ContainerUIGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Container UI";

        Icon IGameEditorMenuTreeNode.Icon => Icon.None;
    }
}
#endif