using System;
using System.Collections.Generic;
using VMFramework.GameEvents;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Containers
{
    public delegate void ItemChangedHandler(IContainer container, int index, IContainerItem item);

    public delegate void ItemCountChangedHandler(IContainer container, int index, IContainerItem item, int oldCount,
        int newCount);

    public delegate void SizeChangedHandler(IContainer container, int currentSize);

    public partial interface IContainer : IGameItem, IReadOnlyCollection<IContainerItem>
    {
        public IContainerOwner Owner { get; }

        public bool IsOpen { get; }

        public int? Capacity { get; }

        public int ValidCount { get; }

        public bool IsFull { get; }

        public IReadOnlyCollection<int> ValidSlotIndices { get; }
        
        public IReadOnlyCollection<IContainerItem> ValidItems { get; }

        public event ItemChangedHandler OnBeforeItemChangedEvent;
        public event ItemChangedHandler OnAfterItemChangedEvent;
        public event ItemCountChangedHandler OnItemCountChangedEvent;
        public event SizeChangedHandler OnSizeChangedEvent;

        public IReadOnlyParameterizedGameEvent<ContainerItemChangedParameter> ItemAddedEvent { get; }

        public IReadOnlyParameterizedGameEvent<ContainerItemChangedParameter> ItemRemovedEvent { get; }

        public event Action<IContainer> OnOpenEvent;
        public event Action<IContainer> OnCloseEvent;

        public bool SetOwner(IContainerOwner newOwner);

        public void CheckIndex(int index);

        /// <summary>
        /// 获取指定索引处的<see cref="IContainerItem"/>。
        /// 如果索引越界，则会报错。
        /// 可以使用<see cref="CheckIndex"/>来检查索引是否越界。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IContainerItem GetItem(int index);

        public IReadOnlyList<IContainerItem> GetAllItems();

        /// <summary>
        /// 尝试合并物品到容器中
        /// </summary>
        /// <param name="slotIndex"></param>
        /// <param name="newItem"></param>
        /// <param name="preferredCount">期望合并的数量</param>
        /// <param name="mergedCount">实际合并的数量</param>
        /// <returns>是否发生了合并</returns>
        public bool TryMergeItem(int slotIndex, IContainerItem newItem, int preferredCount, out int mergedCount);

        public void SetItem(int index, IContainerItem item);

        /// <summary>
        /// 如果item完全添加到容器中，则返回true，否则返回false
        /// </summary>
        /// <param name="item"></param>
        /// <param name="preferredCount">期望添加的数量</param>
        /// <param name="addedCount"></param>
        /// <returns></returns>
        public bool TryAddItem(IContainerItem item, int preferredCount, out int addedCount);

        /// <summary>
        /// 如果将数量不超过<paramref name="preferredCount"/>的item完全添加到容器中，则返回true，否则返回false
        /// </summary>
        /// <param name="item"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="preferredCount">期望添加的数量</param>
        /// <param name="addedCount"></param>
        /// <returns></returns>
        public bool TryAddItem(IContainerItem item, int startIndex, int endIndex, int preferredCount,
            out int addedCount);

        public void Sort(Comparison<IContainerItem> comparison);

        public void Sort(Comparison<IContainerItem> comparison, int startIndex, int endIndex);

        /// <summary>
        /// 堆叠物品，将相同物品的数量合并到一起
        /// </summary>
        public void StackItems();

        /// <summary>
        /// 压缩容器，去除物品间的Null
        /// </summary>
        public void Compress();

        /// <summary>
        /// 压缩容器，去除物品间的Null
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        public void Compress(int startIndex, int endIndex);

        public void ExpandTo(int newCount);

        public void ClearAllItems();

        public void CopyAllItemsToArray<TItem>(TItem[] itemsArray)
            where TItem : IContainerItem;

        public void LoadFromItemsArray<TItem>(TItem[] itemsArray)
            where TItem : IContainerItem;

        public void Open();

        public void Close();

        public void Shuffle();
    }
}