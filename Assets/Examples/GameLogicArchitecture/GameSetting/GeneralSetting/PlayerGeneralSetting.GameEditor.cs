#if UNITY_EDITOR
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Examples
{
    public partial class PlayerGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Player Setting";
    }
}
#endif