#if UNITY_EDITOR
using UnityEngine;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GamePrefabSingleWrapper : IGameEditorMenuTreeNode
    {
        EditorIcon IEditorIconProvider.Icon
        {
            get
            {
                if (gamePrefab is IGameEditorMenuTreeNode node)
                {
                    return node.Icon;
                }
                
                return EditorIcon.None;
            }
        }

        object IGameEditorMenuTreeNode.VisualNode
        {
            get
            {
                if (gamePrefab is Object and IGameEditorMenuTreeNode node)
                {
                    return node;
                }
                
                return null;
            }
        }
    }
}
#endif