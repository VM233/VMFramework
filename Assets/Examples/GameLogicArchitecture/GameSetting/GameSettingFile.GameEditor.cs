#if UNITY_EDITOR
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Examples 
{
    public partial class GameSettingFile : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Game Setting";
    }
}
#endif