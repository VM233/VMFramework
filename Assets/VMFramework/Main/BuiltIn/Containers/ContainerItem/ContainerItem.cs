using System;
using Sirenix.OdinInspector;
using VMFramework.GameLogicArchitecture;
using VMFramework.Properties;

namespace VMFramework.Containers
{
    public abstract partial class ContainerItem : GeneralVisualGameItem, IContainerItem
    {
        #region Fields and Properties

        [ShowInInspector]
        public IContainer SourceContainer { get; private set; }

        [ShowInInspector]
        public BaseIntProperty<IContainerItem> count;
        
        [ShowInInspector]
        public abstract int MaxStackCount { get; }
        
        public int SlotIndex { get; private set; }
        
        public event Action<IContainerItem, int, int> OnCountChangedEvent
        {
            add => count.OnValueChanged += value;
            remove => count.OnValueChanged -= value;
        }
        
        IContainer IContainerItem.SourceContainer
        {
            get => SourceContainer;
            set => SourceContainer = value;
        }

        int IContainerItem.Count
        {
            get => count.value;
            set => count.value = value;
        }

        #endregion

        #region Add and Remove Events

        protected virtual void OnAddedToContainer(IContainer container, int slotIndex)
        {
            
        }

        protected virtual void OnRemovedFromContainer(IContainer container)
        {
            
        }

        void IContainerItem.OnAddedToContainer(IContainer container, int slotIndex)
        {
            SlotIndex = slotIndex;
            OnAddedToContainer(container, slotIndex);
        }

        void IContainerItem.OnRemovedFromContainer(IContainer container)
        {
            OnRemovedFromContainer(container);
        }

        #endregion

        #region Pool Events

        protected override void OnGet()
        {
            base.OnGet();
            
            count = new(this, 1);
        }

        #endregion
    }
}