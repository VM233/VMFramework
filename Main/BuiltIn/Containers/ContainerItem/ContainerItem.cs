using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.Core.JSON;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;
using VMFramework.Properties;

namespace VMFramework.Containers
{
    public delegate void ContainerItemSourceChangedHandler(IContainer container, int slotIndex, IContainerItem item);

    public abstract partial class ContainerItem : ControllerGameItem, IContainerItem, IDescriptionManagerProvider
    {
        public abstract string CountPropertyName { get; }

        #region Fields and Properties

        public IValueProperty<IContainerItemOwner> Owner => ownerProperty;

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        public IContainer SourceContainer { get; private set; }

        public abstract int MaxStackCount { get; }

        public int SlotIndex { get; private set; }

        public ReadOnlySpan<IJSONSerializationReceiver> JSONSerializationReceivers => jsonSerializationReceivers;

        public PropertyManager PropertyManager { get; protected set; }

        public DescriptionManager DescriptionManager { get; protected set; }

        public event IDirtyable.DirtyHandler OnDirty;

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected IJSONSerializationReceiver[] jsonSerializationReceivers;

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        [DisableInEditorMode]
        protected readonly SimpleProperty<IContainerItemOwner> ownerProperty = new();

        IContainer IContainerItem.SourceContainer
        {
            get => SourceContainer;
            set => SourceContainer = value;
        }

        public IValueProperty<int> Count => countProperty;

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        [DisableInEditorMode]
        protected IValueProperty<int> countProperty;

        #endregion

        #region Add and Remove Events

        public event ContainerItemSourceChangedHandler OnAddedToContainerEvent;
        public event ContainerItemSourceChangedHandler OnRemovedFromContainerEvent;

        protected virtual void OnAddedToContainer(IContainer container, int slotIndex)
        {
            transform.SetParent(container.transform);
        }

        protected virtual void OnRemovedFromContainer(IContainer container)
        {

        }

        void IContainerItem.OnAddedToContainer(IContainer container, int slotIndex)
        {
            SlotIndex = slotIndex;
            OnAddedToContainer(container, slotIndex);
            OnAddedToContainerEvent?.Invoke(container, slotIndex, this);
        }

        void IContainerItem.OnRemovedFromContainer(IContainer container)
        {
            OnRemovedFromContainer(container);
            OnRemovedFromContainerEvent?.Invoke(container, SlotIndex, this);
        }

        #endregion

        #region Pool Events

        protected virtual void Awake()
        {
            PropertyManager = GetComponentInChildren<PropertyManager>();
            PropertyManager.GetPropertyStrictly(CountPropertyName, out countProperty);

            DescriptionManager = GetComponentInChildren<DescriptionManager>();
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            ownerProperty.SetOwner(this);

            foreach (var dirtyable in GetComponentsInChildren<IDirtyable>())
            {
                if (ReferenceEquals(dirtyable, this))
                {
                    continue;
                }

                dirtyable.OnDirty += OnComponentDirty;
            }

            jsonSerializationReceivers = GetComponentsInChildren<IJSONSerializationReceiver>();
            Array.Sort(jsonSerializationReceivers, (a, b) => a.Priority.CompareTo(b.Priority));
        }

        protected override void OnReturn()
        {
            base.OnReturn();

            ownerProperty.SetValue(null, initial: false);
        }

        #endregion

        #region Insert and Merge

        public event IContainerItem.IsMergeableCheckHandler OnCheckIsMergeable;
        public event IContainerItem.InsertableCheckHandler OnCheckInsertable;
        public event IContainerItem.InsertHandler OnInsert;

        public virtual bool IsMergeableWith(IContainerItem other)
        {
            if (other == null)
            {
                return false;
            }

            if (countProperty.GetValue() >= MaxStackCount)
            {
                return false;
            }

            if (other.id != id)
            {
                return false;
            }

            bool isMergeable = true;
            OnCheckIsMergeable?.Invoke(this, other, ref isMergeable);
            return isMergeable;
        }

        public virtual ItemMergeResult CheckInsertable(IReadOnlyCollection<ContainerItemIndexPair> others,
            InsertEmptyRangeInfo emptyRangeInfo, int preferredCount, ICollection<int> filledSlots,
            out int actualInsertCount)
        {
            actualInsertCount = 0;
            ItemMergeResult? result = null;
            OnCheckInsertable?.Invoke(this, others, emptyRangeInfo, filledSlots, preferredCount, ref result,
                ref actualInsertCount);
            result ??= ItemMergeResult.None;
            return result.Value;
        }

        public virtual ItemMergeResult Insert(IReadOnlyCollection<ContainerItemIndexPair> others,
            InsertEmptyRangeInfo emptyRangeInfo, int preferredCount, ICollection<ContainerItemIndexPair> splitItems,
            out int actualInsertCount)
        {
            actualInsertCount = 0;
            ItemMergeResult? result = null;
            OnInsert?.Invoke(this, others, emptyRangeInfo, splitItems, preferredCount, ref result,
                ref actualInsertCount);
            result ??= ItemMergeResult.None;
            return result.Value;
        }

        #endregion

        #region Split

        public event IContainerItem.SplitHandler OnSplit;

        public virtual void Split(int targetCount, ICollection<IContainerItem> splitItems, out int actualSplitCount)
        {
            actualSplitCount = 0;
            OnSplit?.Invoke(this, targetCount, splitItems, ref actualSplitCount);
        }

        #endregion

        #region Remove

        public event IContainerItem.RemovableCheckHandler OnCheckRemovable;
        public event IContainerItem.RemoveHandler OnRemove;

        public virtual void CheckRemovable(int targetRemoveCount, out int actualRemoveCount)
        {
            actualRemoveCount = 0;
            OnCheckRemovable?.Invoke(this, targetRemoveCount, ref actualRemoveCount);
        }

        public virtual void Remove(int targetRemoveCount, out int actualRemoveCount)
        {
            actualRemoveCount = 0;
            OnRemove?.Invoke(this, targetRemoveCount, ref actualRemoveCount);
        }

        #endregion

        protected virtual void MakeDirty(bool local)
        {
            OnDirty?.Invoke(this, local);
        }

        private void OnComponentDirty(IDirtyable dirtyable, bool local)
        {
            OnDirty?.Invoke(this, local);
        }

        protected override void OnGetStringProperties(
            ICollection<(string propertyID, string propertyContent)> collection)
        {
            base.OnGetStringProperties(collection);

            if (countProperty != null)
            {
                collection?.Add((CountPropertyName, countProperty.GetValue().ToString()));
            }

            if (ownerProperty != null)
            {
                collection?.Add(("Owner", ownerProperty.GetValue()?.ToString()));
            }
        }
    }
}