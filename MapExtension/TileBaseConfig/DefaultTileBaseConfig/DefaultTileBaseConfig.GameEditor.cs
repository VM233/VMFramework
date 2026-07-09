#if UNITY_EDITOR
using VMFramework.Core.Editor;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;

namespace VMFramework.Maps
{
    public partial class DefaultTileBaseConfig : IGameEditorMenuTreeNode
    {
        EditorIcon IEditorIconProvider.Icon => tile.GetAssetPreview();
    }
}
#endif