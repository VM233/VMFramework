#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;

namespace VMFramework.GameLogicArchitecture
{
    public partial class BuiltInModulesSettingFile : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => EditorNames.BUILT_IN_MODULES_SETTINGS;

        EditorIcon IEditorIconProvider.Icon => SdfIconType.Inboxes;
    }
}
#endif