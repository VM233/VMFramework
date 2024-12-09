using System.Collections.Generic;

namespace VMFramework.Core
{
    public interface IMinHeap<TItem> : IReadOnlyCollection<TItem>
    {
        public TItem MinItem { get; }
        
        public void Add(TItem item);
        
        public bool Contains(TItem item);
        
        public TItem ExtractMin();

        public void Clear();
    }
}