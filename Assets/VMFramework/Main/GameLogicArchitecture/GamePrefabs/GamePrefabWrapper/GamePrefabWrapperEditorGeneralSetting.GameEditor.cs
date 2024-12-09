#if UNITY_EDITOR
using VMFramework.Editor.GameEditor;

namespace VMFramework.GameLogicArchitecture.Editor
{
    public partial class GamePrefabWrapperEditorGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Game Prefab Wrapper";
    }
}
#endif