#if UNITY_EDITOR
using System.Collections.Generic;
using VMFramework.Core.Linq;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GamePrefabMultipleWrapper : IGameEditorMenuTreeNode
    {
        Icon IGameEditorMenuTreeNode.Icon
        {
            get
            {
                if (gamePrefabs.IsNullOrEmpty())
                {
                    return Icon.None;
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
                
                return Icon.None;
            }
        }

        protected override IEnumerable<ToolbarButtonConfig> GetToolbarButtons()
        {
            if (gamePrefabs.IsNullOrEmpty() == false)
            {
                foreach (var gamePrefab in gamePrefabs)
                {
                    if (gamePrefab is IGameEditorToolbarProvider provider)
                    {
                        foreach (var config in provider.GetToolbarButtons())
                        {
                            yield return config;
                        }
                    }
                }
            }
            
            foreach (var config in base.GetToolbarButtons())
            {
                yield return config;
            }
        }
    }
}
#endif