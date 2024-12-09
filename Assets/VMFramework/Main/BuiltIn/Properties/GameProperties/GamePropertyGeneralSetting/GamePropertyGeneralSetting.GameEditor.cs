#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Properties
{
    public partial class GamePropertyGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Game Property";

        Icon IGameEditorMenuTreeNode.Icon => SdfIconType.JournalText;
    }
}
#endif