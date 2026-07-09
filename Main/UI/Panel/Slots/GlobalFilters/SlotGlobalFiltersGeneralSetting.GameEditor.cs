#if UNITY_EDITOR
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI 
{
    public partial class SlotGlobalFiltersGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Slot Global Filters";

        EditorIcon IEditorIconProvider.Icon => EditorIcon.None;
    }
}
#endif