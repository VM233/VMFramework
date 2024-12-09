#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Maps
{
    public partial class ExtendedRuleTileGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Ext Rule Tile";

        public Icon Icon => Icon.None;
    }
}
#endif