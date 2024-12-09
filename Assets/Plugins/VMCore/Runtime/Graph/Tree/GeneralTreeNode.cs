using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public sealed class GeneralTreeNode<TData> : ITreeNode<GeneralTreeNode<TData>>
    {
        public TData data { get; private set; }

        private GeneralTreeNode<TData> parent;

        private readonly HashSet<GeneralTreeNode<TData>> children = new();
        
        public GeneralTreeNode(TData data)
        {
            SetData(data);
        }

        public GeneralTreeNode(TData data, GeneralTreeNode<TData> parent)
        {
            SetData(data);
            parent.AddChild(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetData(TData data)
        {
            this.data = data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddChild(GeneralTreeNode<TData> child)
        {
            children.Add(child);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GeneralTreeNode<TData> GetParent()
        {
            return parent;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<GeneralTreeNode<TData>> GetChildren()
        {
            return children;
        }
    }
}