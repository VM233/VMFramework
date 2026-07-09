using System.Collections.Generic;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.Properties;

namespace VMFramework.Containers
{
    public interface IContainerItem : IJSONSerializableControllerGameItem, IDirtyable, IPropertyManagerOwner
    {
        public delegate void IsMergeableCheckHandler(IContainerItem item, IContainerItem other,
            ref bool isMergeable);
        
        public delegate void InsertableCheckHandler(IContainerItem item,
            IReadOnlyCollection<ContainerItemIndexPair> others, InsertEmptyRangeInfo emptyRangeInfo,
            ICollection<int> filledSlots, int preferredCount, ref ItemMergeResult? mergeResult,
            ref int actualInsertCount);

        public delegate void InsertHandler(IContainerItem item, IReadOnlyCollection<ContainerItemIndexPair> others,
            InsertEmptyRangeInfo emptyRangeInfo, ICollection<ContainerItemIndexPair> splitItems, int preferredCount,
            ref ItemMergeResult? mergeResult, ref int actualInsertCount);

        public delegate void SplitHandler(IContainerItem item, int targetCount, ICollection<IContainerItem> splitItems,
            ref int actualSplitCount);

        public delegate void RemovableCheckHandler(IContainerItem item, int targetRemoveCount,
            ref int actualRemoveCount);

        public delegate void RemoveHandler(IContainerItem item, int targetRemoveCount, ref int actualRemoveCount);

        public IValueProperty<IContainerItemOwner> Owner { get; }

        public IContainer SourceContainer { get; set; }

        public int MaxStackCount { get; }
        
        public IValueProperty<int> Count { get; }

        public int SlotIndex { get; }

        public event ContainerItemSourceChangedHandler OnAddedToContainerEvent;
        public event ContainerItemSourceChangedHandler OnRemovedFromContainerEvent;

        public event IsMergeableCheckHandler OnCheckIsMergeable;
        public event InsertableCheckHandler OnCheckInsertable;
        public event InsertHandler OnInsert;
        public event SplitHandler OnSplit;
        public event RemovableCheckHandler OnCheckRemovable;
        public event RemoveHandler OnRemove;

        public bool IsMergeableWith(IContainerItem other);

        public ItemMergeResult CheckInsertable(IReadOnlyCollection<ContainerItemIndexPair> others,
            InsertEmptyRangeInfo emptyRangeInfo, int preferredCount, ICollection<int> filledSlots,
            out int actualInsertCount);

        public ItemMergeResult Insert(IReadOnlyCollection<ContainerItemIndexPair> others,
            InsertEmptyRangeInfo emptyRangeInfo, int preferredCount, ICollection<ContainerItemIndexPair> splitItems,
            out int actualInsertCount);

        public void Split(int targetCount, ICollection<IContainerItem> splitItems, out int actualSplitCount);

        public void CheckRemovable(int targetRemoveCount, out int actualRemoveCount);

        public void Remove(int targetRemoveCount, out int actualRemoveCount);

        public void OnAddedToContainer(IContainer container, int slotIndex);

        public void OnRemovedFromContainer(IContainer container);
    }
}