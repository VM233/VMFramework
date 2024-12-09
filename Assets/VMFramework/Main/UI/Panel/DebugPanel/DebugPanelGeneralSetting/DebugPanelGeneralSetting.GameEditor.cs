#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI
{
    public partial class DebugPanelGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Debug Entry";

        Icon IGameEditorMenuTreeNode.Icon => SdfIconType.Bug;
    }
}
#endif