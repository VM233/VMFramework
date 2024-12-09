using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMFramework.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core.Linq;
using VMFramework.Core.Pools;
using VMFramework.GameEvents;
using VMFramework.GameLogicArchitecture;
using VMFramework.Network;

namespace VMFramework.Containers
{
    public partial class Container : UUIDGameItem, IContainer
    {
        protected IContainerConfig ContainerConfig => (IContainerConfig)GamePrefab;
        
        [ShowInInspector]
        public IContainerOwner Owner { get; private set; }

        public bool IsOpen { get; private set; } = false;

        public int Count => items.Count;

        public int? Capacity { get; private set; }

        public int ValidCount => validItemsLookup.Count;

        [ShowInInspector]
        public virtual bool IsFull => Capacity.HasValue && ValidCount >= Capacity;

        [ShowInInspector]
        public virtual bool IsEmpty => ValidCount == 0;

        private readonly SortedDictionary<int, IContainerItem> validItemsLookup = new();

        public event ItemChangedHandler OnBeforeItemChangedEvent;
        public event ItemChangedHandler OnAfterItemChangedEvent;

        public IReadOnlyParameterizedGameEvent<ContainerItemChangedParameter> ItemAddedEvent => itemAddedEvent;

        public IReadOnlyParameterizedGameEvent<ContainerItemChangedParameter> ItemRemovedEvent => itemRemovedEvent;

        [ShowInInspector]
        private ContainerItemChangedEvent itemAddedEvent;

        [ShowInInspector]
        private ContainerItemChangedEvent itemRemovedEvent;

        public event Action<IContainer> OnOpenEvent;
        public event Action<IContainer> OnCloseEvent;

        public event ItemCountChangedHandler OnItemCountChangedEvent;

        public event SizeChangedHandler OnSizeChangedEvent;
        
        private Action<IContainerItem, int, int> itemCountChangedFunc;
        
        [ShowInInspector]
        private List<IContainerItem> items = new();

        public IReadOnlyCollection<int> ValidSlotIndices => validItemsLookup.Keys;

        public IReadOnlyCollection<IContainerItem> ValidItems => validItemsLookup.Values;

        #region Pool Events

        protected override void OnCreate()
        {
            base.OnCreate();
            
            itemCountChangedFunc = OnItemCountChanged;

            Capacity = ContainerConfig.Capacity;

            if (Capacity.HasValue)
            {
                for (var i = items.Count; i < Capacity.Value; i++)
                {
                    items.Add(null);
                }
            }
            
            itemAddedEvent =
                GameItemManager.Get<ContainerItemChangedEvent>(ContainerItemChangedEventConfig.ADDED_EVENT_ID);
            itemRemovedEvent =
                GameItemManager.Get<ContainerItemChangedEvent>(ContainerItemChangedEventConfig.REMOVED_EVENT_ID);
        }

        protected override void OnGet()
        {
            base.OnGet();

            using var containerCreateEvent = ContainerCreateEvent.Get();
            containerCreateEvent.SetContainer(this);
            containerCreateEvent.Propagate();
        }

        protected override void OnReturn()
        {
            base.OnReturn();

            if (IsDebugging)
            {
                Debugger.LogWarning($"{this} is Destroyed");
            }

            using var containerDestroyEvent = ContainerDestroyEvent.Get();
            containerDestroyEvent.SetContainer(this);
            containerDestroyEvent.Propagate();
            
            itemAddedEvent.Reset();
            itemRemovedEvent.Reset();
        }

        protected override void OnClear()
        {
            base.OnClear();
            
            GameItemManager.Return(itemAddedEvent);
            GameItemManager.Return(itemRemovedEvent);

            itemAddedEvent = null;
            itemRemovedEvent = null;
        }

        #endregion

        #region Owner

        public bool SetOwner(IContainerOwner newOwner)
        {
            Owner = this.SetClassValue(Owner, newOwner, out bool result, nameof(Owner));
            return result;
        }

        #endregion

        #region Open & Close

        public void Open()
        {
            if (IsDebugging)
            {
                Debug.LogWarning($"Opening：{this}");
            }

            IsOpen = true;

            OnOpenEvent?.Invoke(this);
        }

        public void Close()
        {
            if (IsDebugging)
            {
                Debug.LogWarning($"Closing：{this}");
            }

            IsOpen = false;

            OnCloseEvent?.Invoke(this);
        }

        #endregion

        #region IContainerItem Event

        private void OnItemCountChanged(IContainerItem item, int previousCount, int currentCount)
        {
            if (currentCount <= 0)
            {
                SetItem(item.SlotIndex, null);

                GameItemManager.Return(item);
            }
            else
            {
                OnItemCountChangedEvent?.Invoke(this, item.SlotIndex, item, previousCount, currentCount);
            }
        }

        protected void OnItemAdded(int slotIndex, IContainerItem item)
        {
            item.SourceContainer = this;

            validItemsLookup.Add(slotIndex, item);

            item.OnCountChangedEvent += itemCountChangedFunc;

            item.OnAddedToContainer(this, slotIndex);

            itemAddedEvent.Propagate(new(this, slotIndex, item));
        }

        protected void OnItemRemoved(int slotIndex, IContainerItem item)
        {
            item.OnCountChangedEvent -= itemCountChangedFunc;

            validItemsLookup.Remove(slotIndex);

            if (item.SourceContainer == this)
            {
                item.SourceContainer = null;
            }

            item.OnRemovedFromContainer(this);

            itemRemovedEvent.Propagate(new(this, slotIndex, item));
        }

        protected void OnBeforeItemChanged(int slotIndex, IContainerItem item)
        {
            OnBeforeItemChangedEvent?.Invoke(this, slotIndex, item);
        }

        protected void OnAfterItemChanged(int slotIndex, IContainerItem item)
        {
            OnAfterItemChangedEvent?.Invoke(this, slotIndex, item);
        }

        #endregion

        #region Size Event

        protected void OnCountChanged()
        {
            OnSizeChangedEvent?.Invoke(this, Count);

            if (IsDebugging)
            {
                Debugger.LogWarning($"This size of {this} has changed to {Count}");
            }
        }

        #endregion

        #region Query Items

        public void CheckIndex(int index)
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

        #endregion

        #region Try Merge
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryMergeItem(int slotIndex, IContainerItem newItem, int preferredCount, out int mergedCount)
        {
            var itemInContainer = GetItem(slotIndex);

            itemInContainer.AssertIsNotNull(nameof(itemInContainer));

            if (itemInContainer.IsMergeableWith(newItem) == false)
            {
                mergedCount = 0;
                return false;
            }

            OnBeforeItemChanged(slotIndex, itemInContainer);

            mergedCount = itemInContainer.MergeWith(newItem, preferredCount);

            OnAfterItemChanged(slotIndex, itemInContainer);

            return true;
        }

        #endregion

        #region Set Item
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetItem(int index, IContainerItem item)
        {
            var targetItem = item;

            if (targetItem is { Count: <= 0 })
            {
                targetItem = null;
            }

            if (item is { SourceContainer: not null })
            {
                item.SourceContainer.SetItem(item.SlotIndex, null);
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

        public virtual bool TryAddItem(IContainerItem item, int preferredCount, out int addedCount)
        {
            return TryAddItem(item, int.MinValue, int.MaxValue, preferredCount, out addedCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryAddItem(IContainerItem item, int startIndex, int endIndex, int preferredCount,
            out int addedCount)
        {
            if (IsDebugging)
            {
                Debugger.LogWarning($"Trying to add {item} to {this} in slots range: [{startIndex}, {endIndex}]" +
                                    $"with preferred count: {preferredCount}");
            }

            addedCount = 0;
            
            if (item == null)
            {
                return true;
            }

            if (item.IsDestroyed)
            {
                throw new InvalidOperationException($"Cannot add destroyed item {item} to {this}.");
            }

            if (item.Count <= 0)
            {
                return true;
            }

            if (preferredCount <= 0)
            {
                return true;
            }

            if (startIndex > endIndex)
            {
                return false;
            }

            var clampedStartIndex = startIndex.ClampMin(0);
            var clampedEndIndex = endIndex.ClampMax(items.Count - 1);

            int leftCount = preferredCount.Min(item.Count);

            for (var slotIndex = clampedStartIndex; slotIndex <= clampedEndIndex; slotIndex++)
            {
                var itemInContainer = items[slotIndex];

                if (itemInContainer == null)
                {
                    continue;
                }

                if (TryMergeItem(slotIndex, item, leftCount, out var mergedCount))
                {
                    leftCount -= mergedCount;
                    addedCount += mergedCount;

                    if (leftCount <= 0)
                    {
                        return true;
                    }
                }
            }

            for (var slotIndex = clampedStartIndex; slotIndex <= clampedEndIndex; slotIndex++)
            {
                var itemInContainer = items[slotIndex];
                if (itemInContainer != null)
                {
                    continue;
                }

                bool shouldReturn;

                if (item.Count <= item.MaxStackCount)
                {
                    AddCountLessThanMaxStack(slotIndex, out var addedCountOnce);
                    addedCount += addedCountOnce;

                    shouldReturn = true;
                }
                else
                {
                    AddCountGreaterThanMaxStack(slotIndex, out shouldReturn, out var addedCountOnce);
                    addedCount += addedCountOnce;
                }

                if (shouldReturn)
                {
                    return true;
                }
            }

            var addTimes = 0;
            while (true)
            {
                if (items.Count >= endIndex || (Capacity.HasValue && items.Count >= Capacity))
                {
                    return false;
                }

                if (item.Count <= item.MaxStackCount)
                {
                    AddNull(1);

                    AddCountLessThanMaxStack(items.Count - 1, out var addedCountOnce);

                    addedCount += addedCountOnce;

                    break;
                }
                else
                {
                    AddNull(1);

                    AddCountGreaterThanMaxStack(items.Count - 1, out var shouldReturn, out var addedCountOnce);
                    addedCount += addedCountOnce;

                    if (shouldReturn)
                    {
                        break;
                    }

                    addTimes++;

                    if (addTimes >= 1000)
                    {
                        throw new NotSupportedException(
                            $"Failed to add item {item} to {this}, exceeded maximum add times: {1000}");
                    }
                }
            }

            return true;

            void AddCountLessThanMaxStack(int slotIndex, out int addedCountOnce)
            {
                if (item.IsSplittable(leftCount) == false)
                {
                    addedCountOnce = item.Count;
                    SetItem(slotIndex, item);
                }
                else
                {
                    addedCountOnce = leftCount;

                    var cloneItem = item.Split(leftCount);

                    SetItem(slotIndex, cloneItem);
                }
            }

            void AddCountGreaterThanMaxStack(int slotIndex, out bool shouldReturn, out int addedCountOnce)
            {
                if (item.MaxStackCount < leftCount)
                {
                    var cloneItem = item.Split(item.MaxStackCount);

                    SetItem(slotIndex, cloneItem);

                    leftCount -= item.MaxStackCount;
                    addedCountOnce = item.MaxStackCount;

                    shouldReturn = false;
                }
                else
                {
                    var cloneItem = item.Split(leftCount);

                    SetItem(slotIndex, cloneItem);
                    addedCountOnce = leftCount;

                    shouldReturn = true;
                }
            }
        }

        #endregion

        #region Stack Items

        public virtual void StackItems()
        {
            this.StackItems(int.MinValue, int.MaxValue);
        }

        #endregion

        #region Compress Items

        public void Compress()
        {
            Compress(int.MinValue, int.MaxValue);
        }

        /// <summary>
        /// 压缩容器，去除物品间的Null
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        public void Compress(int startIndex, int endIndex)
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

        #region Sort

        public virtual void Sort(Comparison<IContainerItem> comparison)
        {
            Sort(comparison, int.MinValue, int.MaxValue);
        }

        public void Sort(Comparison<IContainerItem> comparison, int startIndex, int endIndex)
        {
            startIndex = startIndex.ClampMin(0);
            endIndex = endIndex.ClampMax(Count - 1);
            
            this.StackItems(startIndex, endIndex);

            var itemList = ListPool<IContainerItem>.Default.Get();
            itemList.Clear();
            itemList.AddRange(this.GetRangeItems(startIndex, endIndex));

            itemList.RemoveAllNull();

            itemList.Sort(comparison);

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
                Debugger.LogError($"Cannot expand container {this} to {newCount} because it has a capacity of {Capacity}");
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
                SetItem(i, null);
            }

            items.Clear();
            OnCountChanged();
        }

        #region Array Operations

        public void LoadFromItemsArray<TItem>(TItem[] itemsArray)
            where TItem : IContainerItem
        {
            ClearAllItems();

            for (var i = 0; i < itemsArray.Length; i++)
            {
                items.Add(null);
            }

            OnCountChanged();

            for (var i = 0; i < itemsArray.Length; i++)
            {
                SetItem(i, itemsArray[i]);
            }
        }

        public void CopyAllItemsToArray<TItem>(TItem[] itemsArray)
            where TItem : IContainerItem
        {
            for (var i = 0; i < items.Count; i++)
            {
                itemsArray[i] = (TItem)items[i];
            }
        }

        #endregion

        public void Shuffle()
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
