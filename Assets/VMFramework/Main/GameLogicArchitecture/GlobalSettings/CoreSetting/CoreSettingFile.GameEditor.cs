#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;

namespace VMFramework.GameLogicArchitecture
{
    public partial class CoreSettingFile : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => EditorNames.CORE_SETTINGS;

        Icon IGameEditorMenuTreeNode.Icon => SdfIconType.Asterisk;
    }
}
#endif