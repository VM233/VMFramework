#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.GameEvents
{
    public partial class GameEventGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Game Event";

        EditorIcon IEditorIconProvider.Icon => new(SdfIconType.Dpad);
    }
}
#endif