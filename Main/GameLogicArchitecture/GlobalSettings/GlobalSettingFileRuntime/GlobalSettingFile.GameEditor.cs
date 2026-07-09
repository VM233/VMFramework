#if UNITY_EDITOR
using System.Collections.Generic;
using VMFramework.Editor.GameEditor;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GlobalSettingFile : IGameEditorMenuTreeNodesProvider
    {
        string INameOwner.Name => name;

        protected virtual IEnumerable<IGameEditorMenuTreeNode> GetAllMenuTreeNodes()
        {
            foreach (var generalSetting in GetAllGeneralSettings())
            {
                if (generalSetting is IGameEditorMenuTreeNode menuTreeNode)
                {
                    yield return menuTreeNode;
                }
            }
        }
        
        IEnumerable<object> IGameEditorMenuTreeNodesProvider.GetAllMenuTreeNodes()
        {
            return GetAllMenuTreeNodes();
        }
    }
}
#endif