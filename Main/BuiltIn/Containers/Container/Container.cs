using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMFramework.Core;
using Sirenix.OdinInspector;
using VMFramework.Core.JSON;
using VMFramework.Core.Linq;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;
using VMFramework.Properties;
using VMFramework.Tools;

namespace VMFramework.Containers
{
    public partial class Container : ControllerGameItem, IContainer
    {
        [TitleGroup(ComponentNames.CONFIG)]
        public int initialSize = 0;

        protected IContainerConfig ContainerConfig => (IContainerConfig)GamePrefab;

        public IValueProperty<IContainerOwner> Owner => ownerProperty;

        public int Count => items.Count;

        public int? Capacity { get; private set; }

        public int ValidCount => validItemsLookup.Count;

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        public virtual bool IsFull => Capacity.HasValue && ValidCount >= Capacity;

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        public virtual bool IsEmpty => ValidCount == 0;

        public IReadOnlyCollection<int> ValidSlotIndices => validItemsLookup.Keys;

        public IReadOnlyCollection<IContainerItem> ValidItems => validItemsLookup.Values;

        public event ItemChangedHandler OnBeforeItemChangedEvent;
        public event ItemChangedHandler OnAfterItemChangedEvent;

        public event IContainer.ItemAddedHandler OnItemAddedEvent;
        public event IContainer.ItemRemovedHandler OnItemRemovedEvent;

        public event ItemDirtyHandler OnItemDirtyEvent;

        public event SizeChangedHandler OnSizeChangedEvent;

        public event IContainer.QueryHandler OnQueryItems;

        public event IContainer.FilterMatchHandler OnFilterMatch;
        public event IContainer.AddPriorityRangeCollectHandler OnCollectAddPriorityRanges;
        public event IContainer.SortableRangeCollectHandler OnCollectSortableRanges;

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected readonly SimpleProperty<IContainerOwner> ownerProperty = new();

        protected PropertyChangedHandler<int> itemCountChangedFunc;
        protected IDirtyable.DirtyHandler itemDirtyFunc;
        protected PropertyChangedHandler<IContainerItemOwner> itemOwnerChangedFunc;

        protected readonly SortedDictionary<int, IContainerItem> validItemsLookup = new();

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected List<IContainerItem> items = new();

        public ReadOnlySpan<IJSONSerializationReceiver> JSONSerializationReceivers => jsonSerializationReceivers;

        protected IJSONSerializationReceiver[] jsonSerializationReceivers;
        
        protected Func<int?, IContainerItem, string, bool> filterMatchFunc;

        #region Pool Events

        protected override void OnCreate()
        {
            base.OnCreate();

            ownerProperty.SetOwner(this);
            ownerProperty.OnChanged += OnOwnerChanged;

            itemCountChangedFunc = OnItemCountChanged;
            itemDirtyFunc = OnItemDirty;
            itemOwnerChangedFunc = OnItemOwnerChanged;

            Capacity = ContainerConfig.Capacity;

            if (Capacity.HasValue)
            {
                for (var i = items.Count; i < Capacity.Value; i++)
                {
                    items.Add(null);
                }
            }

            jsonSerializationReceivers = GetComponentsInChildren<IJSONSerializationReceiver>();
            Array.Sort(jsonSerializationReceivers, (a, b) => a.Priority.CompareTo(b.Priority));

            filterMatchFunc = IsMatchFilters;
        }

        protected override void OnGet()
        {
            base.OnGet();

            if (Capacity.HasValue == false)
            {
                if (initialSize > 0)
                {
                    AddNull(initialSize);
                }
            }
        }

        protected override void OnReturn()
        {
            base.OnReturn();

            if (IsDebugging)
            {
                UnityEngine.Debug.LogWarning($"{this} is Destroyed");
            }

            ClearAllItems();

            var transformParent =
                ContainerTransform.Get(BuiltInModulesSetting.ContainerGeneralSetting.transformContainerName);
            transform.SetParent(transformParent);

            ownerProperty.SetValue(null, initial: false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ReturnItems()
        {
            foreach (var item in items)
            {
                if (item == null)
                {
                    continue;
                }

                GameItemManager.Instance.Return(item);
            }
        }

        #endregion

        #region Internal Events

        protected virtual void OnOwnerChanged(IReadOnlyProperty property, IContainerOwner previous,
            IContainerOwner current, bool initial)
        {
            var ownerChangingItems = items.ToListDefaultPooled();
            try
            {
                foreach (var item in ownerChangingItems)
                {
                    if (item == null || item.IsDestroyed || ReferenceEquals(item.SourceContainer, this) == false)
                    {
                        continue;
                    }

                    item.Owner.SetValue(current, initial);
                }
            }
            finally
            {
                ownerChangingItems.ReturnToDefaultPool();
            }
        }

        private void OnItemCountChanged(IReadOnlyProperty property, int previousCount, int currentCount, bool isInitial)
        {
            var item = (IContainerItem)property.Owner;
            if (currentCount <= 0)
            {
                SetItem(item.SlotIndex, null);

                GameItemManager.Instance.Return(item);
            }
        }

        private void OnItemDirty(IDirtyable dirtyable, bool local)
        {
            var containerItem = (IContainerItem)dirtyable;
            OnItemDirtyEvent?.Invoke(this, containerItem.SlotIndex, containerItem, local);
        }

        protected virtual void OnItemOwnerChanged(IReadOnlyProperty property, IContainerItemOwner previous,
            IContainerItemOwner current, bool initial)
        {
            if (previous == current)
            {
                return;
            }

            if (current != ownerProperty.GetValue())
            {
                var item = (IContainerItem)property.Owner;
                SetItem(item.SlotIndex, null);
            }
        }

        protected virtual void OnItemAdded(int slotIndex, IContainerItem item)
        {
            item.SourceContainer = this;

            validItemsLookup.Add(slotIndex, item);

            item.Count.OnChanged += itemCountChangedFunc;
            item.OnDirty += itemDirtyFunc;
            item.Owner.OnChanged += itemOwnerChangedFunc;

            item.Owner.SetValue(ownerProperty.GetValue(), initial: false);

            item.OnAddedToContainer(this, slotIndex);

            OnItemAddedEvent?.Invoke(this, slotIndex, item);
        }

        protected virtual void OnItemRemoved(int slotIndex, IContainerItem item)
        {
            item.Count.OnChanged -= itemCountChangedFunc;
            item.OnDirty -= itemDirtyFunc;
            item.Owner.OnChanged -= itemOwnerChangedFunc;

            validItemsLookup.Remove(slotIndex);

            if (ReferenceEquals(item.SourceContainer, this))
            {
                item.SourceContainer = null;
            }

            if (item.Owner.GetValue() == ownerProperty.GetValue())
            {
                item.Owner.SetValue(null, initial: false);
            }

            item.OnRemovedFromContainer(this);

            OnItemRemovedEvent?.Invoke(this, slotIndex, item);
        }

        protected void OnBeforeItemChanged(int slotIndex, IContainerItem item)
        {
            OnBeforeItemChangedEvent?.Invoke(this, slotIndex, item);
        }

        protected void OnAfterItemChanged(int slotIndex, IContainerItem item)
        {
            OnAfterItemChangedEvent?.Invoke(this, slotIndex, item);
        }

        protected void OnCountChanged()
        {
            OnSizeChangedEvent?.Invoke(this, Count);

            if (IsDebugging)
            {
                UnityEngine.Debug.LogWarning($"This size of {this} has changed to {Count}");
            }
        }

        #endregion

        #region Query Items

        public bool IsIndexValid(int index)
        {
            return index >= 0 && index < items.Count;
        }

        public void MandatoryCheckIndex(int index)
        {
            if (index < 0 || index >= items.Count)
            {
                throw new IndexOutOfRangeException(
                    $"Index {index} is out of range [{0}, {items.Count - 1}] for container {this}.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IContainerItem GetItem(int index) => items[index];

        public IReadOnlyList<IContainerItem> GetAllItems() => items;

        public virtual void QueryItems(ContainerQueryArguments arguments, ICollection<IContainerItem> items)
        {
            OnQueryItems?.Invoke(this, arguments, items);
        }

        #endregion

        #region Match Filters

        public virtual bool IsMatchFilters(int? slotIndex, IContainerItem item, string intention)
        {
            var isMatch = true;
            OnFilterMatch?.Invoke(this, slotIndex, item, intention, ref isMatch);
            return isMatch;
        }

        #endregion

        #region Set Item

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetNullAndReturn(int index)
        {
            var item = items[index];

            if (item == null)
            {
                return;
            }

            SetItem(index, null);
            GameItemManager.Instance.Return(item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetItem(int index, IContainerItem item)
        {
            var targetItem = item;

            if (targetItem != null)
            {
                if (targetItem.IsDestroyed)
                {
                    targetItem = null;
                }
                else if (targetItem.Count.GetValue() <= 0)
                {
                    GameItemManager.Instance.Return(targetItem);
                    targetItem = null;
                }
                else
                {
                    targetItem.SourceContainer?.SetItem(item.SlotIndex, null);
                }
            }

            var oldItem = items[index];

            items[index] = targetItem;

            if (oldItem != null)
            {
                OnItemRemoved(index, oldItem);
            }

            OnBeforeItemChanged(index, oldItem);

            if (targetItem != null)
            {
                OnItemAdded(index, targetItem);
            }

            OnAfterItemChanged(index, targetItem);
        }

        #endregion

        #region Add Item

        public virtual ItemMergeResult CheckItemAddable(IContainerItem item, ContainerAddArguments arguments,
            ContainerAddableCheckHint hint, ICollection<int> filledSlots, out int actualInsertCount)
        {
            if (item == null || item.IsDestroyed)
            {
                actualInsertCount = 0;
                return ItemMergeResult.None;
            }

            if (IsMatchFilters(null, item, arguments.intention) == false)
            {
                actualInsertCount = 0;
                return ItemMergeResult.None;
            }

            var (startIndex, endIndex) = arguments.slotRange ?? RangeInteger.Infinite;

            if (IsDebugging)
            {
                UnityEngine.Debug.LogWarning($"Trying to add {item} to {this} in slots range: [{startIndex}, {endIndex}]");
            }

            startIndex = startIndex.ClampMin(0);

            actualInsertCount = 0;

            if (startIndex > endIndex)
            {
                return ItemMergeResult.None;
            }

            var priorityRanges = ListPool<(RangeInteger Range, int priority)>.Default.Get();
            priorityRanges.Clear();

            OnCollectAddPriorityRanges?.Invoke(this, item, arguments.intention, priorityRanges);

            if (priorityRanges.Count <= 0)
            {
                priorityRanges.ReturnToDefaultPool();
                return ItemMergeResult.None;
            }

            var checkedIndices = HashSetPool<int>.Default.Get();
            checkedIndices.Clear();
            
            var unmatchableIndices = HashSetPool<int>.Default.Get();
            unmatchableIndices.Clear();

            var itemsToInsert = ListPool<ContainerItemIndexPair>.Default.Get();

            priorityRanges.Sort((a, b) => a.priority.CompareTo(b.priority));

            ItemMergeResult result = ItemMergeResult.None;

            int preferredCount = arguments.preferredCount ?? int.MaxValue;

            foreach (var (range, _) in priorityRanges)
            {
                itemsToInsert.Clear();

                var min = range.min.ClampMin(startIndex);
                var max = range.max.ClampMax(endIndex).ClampMax(items.Count - 1);

                if (min > max)
                {
                    continue;
                }
                
                unmatchableIndices.Clear();

                for (var index = min; index <= max; index++)
                {
                    if (checkedIndices.Add(index) == false)
                    {
                        continue;
                    }

                    if (IsMatchFilters(index, item, arguments.intention) == false)
                    {
                        unmatchableIndices.Add(index);
                        continue;
                    }

                    if (hint.ignoreExistingItems)
                    {
                        itemsToInsert.Add(new(index, null));
                    }
                    else
                    {
                        var itemInContainer = GetItem(index);
                        itemsToInsert.Add(new(index, itemInContainer));
                    }
                }

                RangeInteger? emptyRange = null;
                if (Capacity is null)
                {
                    var emptyRangeValue = range;
                    emptyRangeValue.min = emptyRangeValue.min.ClampMin(startIndex);
                    emptyRangeValue.max = emptyRangeValue.max.ClampMax(endIndex);

                    emptyRange = emptyRangeValue;
                }

                var emptyRangeInfo = new InsertEmptyRangeInfo(emptyRange, unmatchableIndices,
                    new ContainerFilterMatch(arguments.intention, filterMatchFunc));

                var currentResult = item.CheckInsertable(itemsToInsert, emptyRangeInfo, preferredCount, filledSlots,
                    out var currentActualInsertCount);
                preferredCount -= currentActualInsertCount;
                actualInsertCount += currentActualInsertCount;

                if (currentResult == ItemMergeResult.FullMerge)
                {
                    result = ItemMergeResult.FullMerge;
                    break;
                }

                if (currentResult == ItemMergeResult.PartialMerge)
                {
                    result = ItemMergeResult.PartialMerge;
                }
            }

            priorityRanges.ReturnToDefaultPool();
            checkedIndices.ReturnToDefaultPool();
            unmatchableIndices.ReturnToDefaultPool();
            itemsToInsert.ReturnToDefaultPool();
            return result;
        }

        public virtual ItemMergeResult AddItem(IContainerItem item, ContainerAddArguments arguments,
            out int actualInsertCount)
        {
            if (item == null || item.IsDestroyed)
            {
                actualInsertCount = 0;
                return ItemMergeResult.None;
            }
            
            if (IsMatchFilters(null, item, arguments.intention) == false)
            {
                actualInsertCount = 0;
                return ItemMergeResult.None;
            }

            var (startIndex, endIndex) = arguments.slotRange ?? RangeInteger.Infinite;

            if (IsDebugging)
            {
                UnityEngine.Debug.LogWarning($"Trying to add {item} to {this} in slots range: [{startIndex}, {endIndex}]");
            }

            startIndex = startIndex.ClampMin(0);

            actualInsertCount = 0;

            if (startIndex > endIndex)
            {
                return ItemMergeResult.None;
            }

            var priorityRanges = ListPool<(RangeInteger Range, int priority)>.Default.Get();
            priorityRanges.Clear();

            OnCollectAddPriorityRanges?.Invoke(this, item, arguments.intention, priorityRanges);

            if (priorityRanges.Count <= 0)
            {
                priorityRanges.ReturnToDefaultPool();
                return ItemMergeResult.None;
            }

            var checkedIndices = HashSetPool<int>.Default.Get();
            checkedIndices.Clear();
            
            var unmatchableIndices = HashSetPool<int>.Default.Get();
            unmatchableIndices.Clear();

            var itemsToInsert = ListPool<ContainerItemIndexPair>.Default.Get();
            var splitItems = ListPool<ContainerItemIndexPair>.Default.Get();

            if (priorityRanges.Count > 1)
            {
                priorityRanges.Sort((a, b) => a.priority.CompareTo(b.priority));
            }

            ItemMergeResult result = ItemMergeResult.None;

            int preferredCount = arguments.preferredCount ?? int.MaxValue;

            foreach (var (range, _) in priorityRanges)
            {
                itemsToInsert.Clear();
                splitItems.Clear();

                var min = range.min.ClampMin(startIndex);
                var max = range.max.ClampMax(endIndex).ClampMax(items.Count - 1);

                if (min <= max)
                {
                    for (var index = min; index <= max; index++)
                    {
                        if (checkedIndices.Add(index) == false)
                        {
                            continue;
                        }

                        if (IsMatchFilters(index, item, arguments.intention) == false)
                        {
                            unmatchableIndices.Add(index);
                            continue;
                        }

                        var itemInContainer = GetItem(index);
                        itemsToInsert.Add(new(index, itemInContainer));
                    }
                }

                RangeInteger? emptyRange = null;
                if (Capacity is null)
                {
                    var emptyRangeValue = range;
                    emptyRangeValue.min = emptyRangeValue.min.ClampMin(startIndex);
                    emptyRangeValue.max = emptyRangeValue.max.ClampMax(endIndex);

                    emptyRange = emptyRangeValue;
                }

                if (itemsToInsert.Count <= 0 && emptyRange == null)
                {
                    continue;
                }

                var emptyRangeInfo = new InsertEmptyRangeInfo(emptyRange, unmatchableIndices,
                    new ContainerFilterMatch(arguments.intention, filterMatchFunc));

                var currentResult = item.Insert(itemsToInsert, emptyRangeInfo, preferredCount, splitItems,
                    out var currentActualInsertCount);
                preferredCount -= currentActualInsertCount;
                actualInsertCount += currentActualInsertCount;

                if (Capacity is null)
                {
                    foreach (var (slotIndex, splitItem) in splitItems)
                    {
                        if (slotIndex >= Count)
                        {
                            var nullCount = slotIndex - Count + 1;
                            AddNull(nullCount);
                        }

                        SetItem(slotIndex, splitItem);
                    }
                }
                else
                {
                    foreach (var (slotIndex, splitItem) in splitItems)
                    {
                        SetItem(slotIndex, splitItem);
                    }
                }

                if (currentResult == ItemMergeResult.FullMerge)
                {
                    result = ItemMergeResult.FullMerge;
                    break;
                }

                if (currentResult == ItemMergeResult.PartialMerge)
                {
                    result = ItemMergeResult.PartialMerge;
                }
            }

            priorityRanges.ReturnToDefaultPool();
            checkedIndices.ReturnToDefaultPool();
            unmatchableIndices.ReturnToDefaultPool();
            itemsToInsert.ReturnToDefaultPool();
            splitItems.ReturnToDefaultPool();
            return result;
        }

        #endregion

        #region Sort

        public virtual void Sort(Comparison<IContainerItem> comparison)
        {
            var ranges = ListPool<RangeInteger>.Default.Get();
            ranges.Clear();
            OnCollectSortableRanges?.Invoke(this, ranges);

            foreach (var range in ranges)
            {
                Sort(comparison, range);
            }

            ranges.ReturnToDefaultPool();
        }

        public virtual void Sort(Comparison<IContainerItem> comparison, RangeInteger range)
        {
            range = this.ClampSlotRange(range);

            this.StackItems(range);

            var itemList = ListPool<IContainerItem>.Default.Get();
            itemList.Clear();
            this.GetRangeItems(range, itemList);

            itemList.RemoveAllNull();

            itemList.Sort(comparison);

            for (var i = 0; i < itemList.Count; i++)
            {
                SetItem(range.min + i, itemList[i]);
            }

            for (var i = range.max; i >= range.min + itemList.Count; i--)
            {
                SetItem(i, null);
            }

            itemList.ReturnToDefaultPool();
        }

        #endregion

        #region Compress Items

        public virtual void Compress()
        {
            Compress(int.MinValue, int.MaxValue);
        }

        public virtual void Compress(int startIndex, int endIndex)
        {
            startIndex = startIndex.ClampMin(0);
            endIndex = endIndex.ClampMax(Count - 1);

            var itemList = ListPool<IContainerItem>.Default.Get();
            itemList.Clear();
            itemList.AddRange(this.GetRangeItems(startIndex, endIndex));

            itemList.RemoveAllNull();

            for (var i = 0; i < itemList.Count; i++)
            {
                SetItem(startIndex + i, itemList[i]);
            }

            for (var i = endIndex; i >= startIndex + itemList.Count; i--)
            {
                items.RemoveAt(i);
            }

            itemList.ReturnToDefaultPool();
            OnCountChanged();
        }

        #endregion

        #region String

        protected override void OnGetStringProperties(
            ICollection<(string propertyID, string propertyContent)> collection)
        {
            base.OnGetStringProperties(collection);

            collection.Add((nameof(ValidCount), ValidCount.ToString()));
        }

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void AddNull(int count)
        {
            for (var i = 0; i < count; i++)
            {
                items.Add(null);
            }

            OnCountChanged();
        }

        public void ExpandTo(int newCount)
        {
            if (newCount > Capacity)
            {
                UnityEngine.Debug.LogError(
                    $"Cannot expand container {this} to {newCount} because it has a capacity of {Capacity}");
                newCount = Capacity.Value;
            }

            if (newCount <= items.Count)
            {
                return;
            }

            for (var i = items.Count; i < newCount; i++)
            {
                items.Add(null);
            }

            OnCountChanged();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearAllItems()
        {
            for (var i = 0; i < items.Count; i++)
            {
                SetNullAndReturn(i);
            }

            if (Capacity.HasValue == false)
            {
                items.Clear();
                OnCountChanged();
            }
        }

        #region Array Operations

        public void LoadFromItemsList(IReadOnlyList<IContainerItem> itemsList, bool autoReturn, int count)
        {
            ClearAllItems();

            var sourceCount = itemsList.Count.Min(count);
            if (Capacity.HasValue)
            {
                var minLength = items.Count.Min(sourceCount);

                if (items.Count != sourceCount)
                {
                    UnityEngine.Debug.LogWarning($"Container {this} has a capacity of {Capacity}, " +
                                        $"but the source list has {sourceCount} items.");
                }

                for (int i = 0; i < minLength; i++)
                {
                    var otherItem = itemsList[i];
                    if (otherItem == null)
                    {
                        continue;
                    }

                    SetItem(i, itemsList[i]);
                }

                if (autoReturn)
                {
                    for (int i = minLength; i < sourceCount; i++)
                    {
                        var otherItem = itemsList[i];

                        if (otherItem == null)
                        {
                            continue;
                        }

                        GameItemManager.Instance.Return(otherItem);
                    }
                }
            }
            else
            {
                ExpandTo(sourceCount);
                OnCountChanged();

                for (var i = 0; i < sourceCount; i++)
                {
                    SetItem(i, itemsList[i]);
                }
            }
        }

        public void CopyAllItemsToArray(IContainerItem[] itemsArray)
        {
            for (var i = 0; i < items.Count; i++)
            {
                itemsArray[i] = items[i];
            }
        }

        #endregion

        public virtual void Shuffle()
        {
            var itemList = items.ToListDefaultPooled();

            itemList.Shuffle();

            foreach (var (slotIndex, item) in itemList.Enumerate())
            {
                SetItem(slotIndex, item);
            }

            itemList.ReturnToDefaultPool();
        }
    }
}
