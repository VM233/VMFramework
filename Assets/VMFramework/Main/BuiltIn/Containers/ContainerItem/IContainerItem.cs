using System;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Containers
{
    public interface IContainerItem : IGameItem
    {
        public IContainer SourceContainer { get; set; }

        public int MaxStackCount { get; }

        public int Count { get; set; }
        
        public int SlotIndex { get; }
        
        public delegate void CountChangedEventHandler(IContainerItem containerItem, int oldCount, int newCount);

        public event Action<IContainerItem, int, int> OnCountChangedEvent;

        public bool IsMergeableWith(IContainerItem other)
        {
            {
                if (other == null) return false;
                if (Count >= MaxStackCount) return false;

                return other.id == id;
            }
        }

        /// <summary>
        /// 将此物品与另一个物品合并。
        /// </summary>
        /// <param name="other"></param>
        /// <param name="preferredCount">希望合并的最大数量</param>
        /// <returns>实际合并的数量</returns>
        public int MergeWith(IContainerItem other, int preferredCount = int.MaxValue)
        {
            if (other.Count == 0) return 0;

            int maxIncrease = MaxStackCount - Count;
            maxIncrease = maxIncrease.Min(preferredCount);

            if (maxIncrease > other.Count)
            {
                var otherCount = other.Count;
                Count += otherCount;
                other.Count = 0;
                return otherCount;
            }

            Count += maxIncrease;
            other.Count -= maxIncrease;
            return maxIncrease;
        }

        public bool IsSplittable(int targetCount)
        {
            return Count >= targetCount;
        }

        public IContainerItem Split(int targetCount)
        {
            var clone = this.GetClone();

            clone.Count = targetCount;

            Count -= targetCount;

            return clone;
        }

        public void OnAddedToContainer(IContainer container, int slotIndex);

        public void OnRemovedFromContainer(IContainer container);
    }
}
