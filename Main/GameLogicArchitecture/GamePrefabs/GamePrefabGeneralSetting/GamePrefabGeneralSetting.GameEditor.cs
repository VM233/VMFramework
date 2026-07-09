#if UNITY_EDITOR
using System.Collections.Generic;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GamePrefabGeneralSetting : IGameEditorMenuTreeNodesProvider, IGameEditorMenuTreeNode
    {
        string INameOwner.Name => name;

        IEnumerable<object> IGameEditorMenuTreeNodesProvider.GetAllMenuTreeNodes()
        {
            return initialGamePrefabProviders;
        }

        #region Icon

        private IGameEditorMenuTreeNode iconGamePrefab;

        EditorIcon IEditorIconProvider.Icon
        {
            get
            {
                foreach (var gamePrefab in GamePrefabManager.GetAllGamePrefabs(BaseGamePrefabType))
                {
                    if (gamePrefab is not IGameEditorMenuTreeNode node)
                    {
                        continue;
                    }

                    if (node.Icon.IsNull() == false)
                    {
                        iconGamePrefab = node;
                        return node.Icon;
                    }
                }
                
                return EditorIcon.None;
            }
        }

        #endregion
    }
}
#endif