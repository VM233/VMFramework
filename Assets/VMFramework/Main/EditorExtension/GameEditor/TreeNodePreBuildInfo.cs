using System.Collections.Generic;
using VMFramework.Core;

#if UNITY_EDITOR
namespace VMFramework.Editor.GameEditor
{
    internal sealed class TreeNodePreBuildInfo : IResettable, ITreeNode<TreeNodePreBuildInfo>
    {
        public IGameEditorMenuTreeNode node;
        
        public IGameEditorMenuTreeNode parent;

        public IGameEditorMenuTreeNodesProvider provider;
        
        public TreeNodePreBuildInfo parentInfo;
        
        public readonly List<TreeNodePreBuildInfo> childrenInfos = new();
        
        public string name;

        public bool TryReset()
        {
            node = null;
            provider = null;
            parent = null;
            parentInfo = null;
            childrenInfos.Clear();
            name = null;
            return true;
        }


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