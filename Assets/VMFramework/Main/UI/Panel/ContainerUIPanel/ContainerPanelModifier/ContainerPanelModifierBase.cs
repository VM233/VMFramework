using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using VMFramework.Containers;
using VMFramework.Core;
using VMFramework.Core.Linq;

namespace VMFramework.UI
{
    public abstract class ContainerPanelModifierBase : UIToolkitPanelModifier
    {
        protected ContainerPanelModifierBaseConfig ContainerModifierBaseConfig => (ContainerPanelModifierBaseConfig)GamePrefab;
        
        protected IContainerUIPanel ContainerUIPanel => (IContainerUIPanel)Panel;
        
        [ShowInInspector]
        public IContainer BindContainer { get; private set; }
        
        public event Action<IContainerUIPanel, IContainer> OnUnbindContainerEvent;

        public event Action<IContainerUIPanel, IContainer> OnBindContainerEvent;
        
        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            Panel.OnPostCloseEvent += OnPostClose;
            ContainerUIPanel.OnBindContainerAdded += OnBindContainerAdded;
            ContainerUIPanel.OnBindContainerRemoved += OnBindContainerRemoved;
        }

        private void OnBindContainerRemoved(IContainerUIPanel containerUIPanel, IContainer container)
        {
            if (BindContainer == container)
            {
                SetBindContainer(null);
            }
        }

        private void OnBindContainerAdded(IContainerUIPanel containerUIPanel, IContainer container)
        {
            SetBindContainer(container);
        }

        private void OnPostClose(IUIPanel panel)
        {
            SetBindContainer(null);
            
            slotIndicesLookup.Clear();
            slotsLookup.Clear();
        }

        #region Bind Container

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryAsSlotProvider(IContainerItem item, out ISlotProvider slotProvider)
        {
            slotProvider = item as ISlotProvider;

            if (slotProvider == null)
            {
                Debugger.LogWarning($"Item : {item} is not a {nameof(ISlotProvider)}");
                return false;
            }
            
            return true;
        }
        
        private void OnItemCountChanged(IContainer container, int slotIndex, IContainerItem item,
            int previousCount, int currentCount)
        {
            if (IsDebugging)
            {
                Debugger.Log($"序号为{slotIndex}的槽位的物品{item}，数量由{previousCount}变为{currentCount}");
            }

            ISlotProvider slotProvider = null;

            if (item != null && TryAsSlotProvider(item, out slotProvider) == false)
            {
                return;
            }

            if (TryGetSlots(slotIndex, out var slots))
            {
                foreach (var slot in slots)
                {
                    slot.SetSlotProvider(slotProvider);
                }
            }
        }

        private void OnItemChanged(IContainer container, int slotIndex, IContainerItem item)
        {
            ISlotProvider slotProvider = null;

            if (item != null && TryAsSlotProvider(item, out slotProvider) == false)
            {
                return;
            }

            if (TryGetSlots(slotIndex, out var slots))
            {
                foreach (var slot in slots)
                {
                    slot.SetSlotProvider(slotProvider);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateAllSlots()
        {
            if (BindContainer == null)
            {
                return;
            }
            
            foreach (var (slotIndex, item) in BindContainer.GetAllItems().Enumerate())
            {
                OnItemChanged(BindContainer, slotIndex, item);
            }
        }

        private void OnContainerSizeChanged(IContainer container, int currentSize)
        {
            if (BindContainer != null && Panel.IsOpened)
            {
                BuildSlots();
            }
        }

        public void SetBindContainer(IContainer newBindContainer)
        {
            if (Panel.IsOpened == false && newBindContainer != null)
            {
                throw new InvalidOperationException($"Cannot bind container before panel is opened!");
            }

            if (BindContainer != null)
            {
                BindContainer.OnAfterItemChangedEvent -= OnItemChanged;
                BindContainer.OnItemCountChangedEvent -= OnItemCountChanged;
                BindContainer.OnSizeChangedEvent -= OnContainerSizeChanged;

                BindContainer.Close();

                OnUnbindContainerEvent?.Invoke(ContainerUIPanel, BindContainer);
            }

            BindContainer = newBindContainer;

            if (BindContainer != null)
            {
                BindContainer.OnAfterItemChangedEvent += OnItemChanged;
                BindContainer.OnItemCountChangedEvent += OnItemCountChanged;
                BindContainer.OnSizeChangedEvent += OnContainerSizeChanged;

                BuildSlots();
                OnBindContainerEvent?.Invoke(ContainerUIPanel, BindContainer);

                BindContainer.Open();
            }
        }

        #endregion

        #region Slots

        [ShowInInspector]
        private readonly Dictionary<SlotVisualElement, int> slotIndicesLookup = new();
        
        [ShowInInspector]
        private readonly Dictionary<int, HashSet<SlotVisualElement>> slotsLookup = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BuildSlots()
        {
            slotIndicesLookup.Clear();
            slotsLookup.Clear();

            foreach (var distributorConfig in ContainerModifierBaseConfig.slotDistributorConfigs)
            {
                BuildSlots(distributorConfig);
            }
            
            UpdateAllSlots();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void BuildSlots(ContainerSlotDistributorConfig distributorConfig)
        {
            var parent = RootVisualElement.QueryStrictly(distributorConfig.parentName, nameof(RootVisualElement));

            int index = distributorConfig.StartIndex;
            int count = distributorConfig.Count;
            
            foreach (var slot in parent.GetAll<SlotVisualElement>())
            {
                if (count <= 0)
                {
                    break;
                }
                
                SetSlot(index, slot);
                
                index++;
                count--;
            }

            if (BindContainer == null || distributorConfig.autoFill == false)
            {
                return;
            }
            
            var container = RootVisualElement.QueryStrictly(distributorConfig.ContainerName, nameof(RootVisualElement));

            for (int slotIndex = index; slotIndex < BindContainer.Count; slotIndex++)
            {
                var newSlot = new SlotVisualElement()
                {
                    DisplayNoneIfNull = distributorConfig.slotsDisplayNoneIfNull
                };
                
                UIToolkitPanel.AddVisualElement(container, newSlot);
                
                SetSlot(slotIndex, newSlot);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void SetSlot(int slotIndex, SlotVisualElement slot)
        {
            if (slotIndicesLookup.TryAdd(slot, slotIndex) == false)
            {
                Debugger.LogWarning($"Slot with index: {slotIndex} already exists in slotIndicesLookup!");
                return;
            }

            var set = slotsLookup.GetValueOrAddNew(slotIndex);

            if (set.Add(slot) == false)
            {
                Debugger.LogWarning($"Slot with index: {slotIndex} already exists in slotsLookup!");
                return;
            }
            
            OnSetSlot(slotIndex, slot);
        }

        protected virtual void OnSetSlot(int slotIndex, SlotVisualElement slot)
        {
            
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool TryGetSlots(int slotIndex, out HashSet<SlotVisualElement> slots)
        {
            if (slotsLookup.TryGetValue(slotIndex, out slots))
            {
                return slots.Count > 0;
            }

            if (IsDebugging)
            {
                Debugger.LogWarning($"Failed to find slots with index: {slotIndex} in {nameof(slotsLookup)}!");
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool TryGetSlotIndex(SlotVisualElement slot, out int slotIndex)
        {
            return slotIndicesLookup.TryGetValue(slot, out slotIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool TryGetSlotIndexWithWarning(SlotVisualElement slot, out int slotIndex)
        {
            if (slotIndicesLookup.TryGetValue(slot, out slotIndex))
            {
                return true;
            }

            Debugger.LogWarning($"Failed to find slot index for slot: {slot} in {nameof(slotIndicesLookup)}!");

            return false;
        }

        #endregion
    }
}