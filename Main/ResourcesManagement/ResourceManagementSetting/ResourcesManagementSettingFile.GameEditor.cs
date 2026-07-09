#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;

namespace VMFramework.GameLogicArchitecture
{
    public partial class ResourcesManagementSettingFile : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => EditorNames.RESOURCES_MANAGEMENT_SETTINGS;

        EditorIcon IEditorIconProvider.Icon => SdfIconType.Boxes;
    }
}
#endif