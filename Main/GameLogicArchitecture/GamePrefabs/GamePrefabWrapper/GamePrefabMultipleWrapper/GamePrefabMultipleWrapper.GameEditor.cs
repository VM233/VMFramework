#if UNITY_EDITOR
using VMFramework.Core.Linq;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GamePrefabMultipleWrapper : IGameEditorMenuTreeNode
    {
        EditorIcon IEditorIconProvider.Icon
        {
            get
            {
                if (gamePrefabs.IsNullOrEmpty())
                {
                    return EditorIcon.None;
                }
                
                foreach (var gamePrefab in gamePrefabs)
                {
                    if (gamePrefab is IGameEditorMenuTreeNode node)
                    {
                        if (node.Icon.IsNone() == false)
                        {
                            return node.Icon;
                        }
                    }
                }
                
                return EditorIcon.None;
            }
        }
    }
}
#endif