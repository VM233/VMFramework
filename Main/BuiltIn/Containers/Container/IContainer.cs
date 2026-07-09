using System;
using System.Collections.Generic;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.Properties;

namespace VMFramework.Containers
{
    public delegate void ItemChangedHandler(IContainer container, int index, IContainerItem item);

    public delegate void ItemDirtyHandler(IContainer container, int index, IContainerItem item, bool local);

    public delegate void SizeChangedHandler(IContainer container, int currentSize);

    public partial interface IContainer : IJSONSerializableControllerGameItem, IReadOnlyCollection<IContainerItem>
    {
        public delegate void ItemAddedHandler(IContainer container, int index, IContainerItem item);

        public delegate void ItemRemovedHandler(IContainer container, int index, IContainerItem item);

        public delegate void QueryHandler(IContainer container, ContainerQueryArguments arguments,
            ICollection<IContainerItem> items);

        public delegate void FilterMatchHandler(IContainer container, int? index, IContainerItem item, string intention,
            ref bool isMatch);

        public delegate void AddPriorityRangeCollectHandler(IContainer container, IContainerItem item, string intention,
            ICollection<(RangeInteger range, int priority)> ranges);

        public delegate void SortableRangeCollectHandler(IContainer container, ICollection<RangeInteger> ranges);

        public IValueProperty<IContainerOwner> Owner { get; }

        public int? Capacity { get; }

        public int ValidCount { get; }

        public bool IsFull { get; }

        public IReadOnlyCollection<int> ValidSlotIndices { get; }

        public IReadOnlyCollection<IContainerItem> ValidItems { get; }

        public event ItemChangedHandler OnBeforeItemChangedEvent;
        public event ItemChangedHandler OnAfterItemChangedEvent;
        public event ItemDirtyHandler OnItemDirtyEvent;
        public event SizeChangedHandler OnSizeChangedEvent;

        public event ItemAddedHandler OnItemAddedEvent;

        public event ItemRemovedHandler OnItemRemovedEvent;

        public event QueryHandler OnQueryItems;

        public event FilterMatchHandler OnFilterMatch;
        public event AddPriorityRangeCollectHandler OnCollectAddPriorityRanges;
        public event SortableRangeCollectHandler OnCollectSortableRanges;

        /// <summary>
        /// 非强制性版本的检查索引是否越界
        /// 强制版本见<see cref="MandatoryCheckIndex"/>
        /// </summary>
        public bool IsIndexValid(int index);
        
        public void MandatoryCheckIndex(int index);

        /// <summary>
        /// 获取指定索引处的<see cref="IContainerItem"/>。
        /// 如果索引越界，则会报错。
        /// 可以使用<see cref="MandatoryCheckIndex"/>来检查索引是否越界。
        /// </summary>
        public IContainerItem GetItem(int index);

        public IReadOnlyList<IContainerItem> GetAllItems();

        public void QueryItems(ContainerQueryArguments arguments, ICollection<IContainerItem> items);

        public bool IsMatchFilters(int? slotIndex, IContainerItem item, string intention);

        public void SetItem(int index, IContainerItem item);

        public ItemMergeResult CheckItemAddable(IContainerItem item, ContainerAddArguments arguments,
            ContainerAddableCheckHint hint, ICollection<int> filledSlots, out int actualInsertCount);

        /// <summary>
        /// 添加物品到容器中
        /// </summary>
        public ItemMergeResult AddItem(IContainerItem item, ContainerAddArguments arguments, out int actualInsertCount);

        public void Sort(Comparison<IContainerItem> comparison);

        public void Sort(Comparison<IContainerItem> comparison, RangeInteger range);

        /// <summary>
        /// 压缩容器，去除物品间的Null
        /// </summary>
        public void Compress();

        /// <summary>
        /// 压缩容器，去除物品间的Null
        /// </summary>
        public void Compress(int startIndex, int endIndex);

        public void ExpandTo(int newCount);

        public void ClearAllItems();

        public void CopyAllItemsToArray(IContainerItem[] itemsArray);

        public void LoadFromItemsList(IReadOnlyList<IContainerItem> itemsList, bool autoReturn, int count);

        public void Shuffle();
    }
}