#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;

namespace VMFramework.GameLogicArchitecture 
{
    public partial class GameTagGeneralSetting : IGameEditorMenuTreeNode, IGameEditorMenuTreeNodesProvider
    {
        string INameOwner.Name => "Game Tag";

        EditorIcon IEditorIconProvider.Icon => new(SdfIconType.Collection);
        
        IEnumerable<object> IGameEditorMenuTreeNodesProvider.GetAllMenuTreeNodes()
        {
            return gameTagGroups;
        }
    }
}
#endif