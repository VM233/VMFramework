#if UNITY_EDITOR
using VMFramework.Editor.GameEditor;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GameTagGroupBase : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => name;
    }
}
#endif