#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;

namespace VMFramework.GameLogicArchitecture
{
    public partial class ResourcesManagementSettingFile : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => EditorNames.RESOURCES_MANAGEMENT_SETTINGS;

        Icon IGameEditorMenuTreeNode.Icon => SdfIconType.Boxes;
    }
}
#endif