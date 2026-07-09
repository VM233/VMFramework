using System.Collections.Generic;
using VMFramework.Core;

#if UNITY_EDITOR
namespace VMFramework.Editor.GameEditor
{
    internal sealed class TreeNodePreBuildInfo : ITreeNode<TreeNodePreBuildInfo>
    {
        public object node;
        
        public object parent;

        public IGameEditorMenuTreeNodesProvider provider;
        
        public TreeNodePreBuildInfo parentInfo;
        
        public readonly List<TreeNodePreBuildInfo> childrenInfos = new();
        
        public string name;

        TreeNodePreBuildInfo IParentProvider<TreeNodePreBuildInfo>.GetParent()
        {
            return parentInfo;
        }

        IEnumerable<TreeNodePreBuildInfo> IChildrenProvider<TreeNodePreBuildInfo>.GetChildren()
        {
            return childrenInfos;
        }
    }
}
#endif