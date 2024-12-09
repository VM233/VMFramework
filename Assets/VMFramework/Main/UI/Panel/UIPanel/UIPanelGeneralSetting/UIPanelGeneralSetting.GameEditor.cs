#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI
{
    public partial class UIPanelGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "UI Panel";

        Icon IGameEditorMenuTreeNode.Icon => SdfIconType.LayoutWtf;
    }
}
#endif