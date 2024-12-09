using System;
using System.Collections.Generic;

namespace VMFramework.Core
{
    public interface IAStarNode<TNode> : IHeapItem<TNode> where TNode : IAStarNode<TNode>
    {
        /// <summary>
        /// The real cost of reaching this node from the start node.
        /// </summary>
        public float GCost { get; set; }
        
        /// <summary>
        /// The estimated cost of reaching the goal node from this node.
        /// </summary>
        public float HCost { get; set; }
        
        public TNode Parent { get; set; }

        public void GetNeighbors(ICollection<TNode> neighbors);
    }
}