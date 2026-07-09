#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Maps 
{
    public partial class TileBaseConfigGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Tile Base";

        EditorIcon IEditorIconProvider.Icon => SdfIconType.Grid3x3;
    }
}
#endif