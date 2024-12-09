#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.GameLogicArchitecture 
{
    public partial class GameTagGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Game Tag";

        Icon IGameEditorMenuTreeNode.Icon => new(SdfIconType.Collection);
    }
}
#endif