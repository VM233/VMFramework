using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Containers;
using VMFramework.Core;
using VMFramework.Core.Linq;
using VMFramework.Core.Pools;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class UIToolkitContainerModifierBase : PanelModifier,
        IContainerSlotsModifier, IRefreshable
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName(SingleModeLimit = BindObjectsNameAttribute.SingleModeLimitType.Single)]
        [IsNotNullOrEmpty]
        public string bindObjectsName;

        [TitleGroup(ComponentNames.CONFIG)]
        public List<ContainerSlotDistributorConfig> slotDistributorConfigs = new();

        public event ISlotsPanelModifier.OnSlotChangedHandler OnSlotChanged;

        public event ISlotsPanelModifier.OnSlotSourceChangedHandler OnSlotSourceChanged;

        protected bool refreshTag = false;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnPostClose += OnPostClose;
            Panel.BindObjectsManager.OnBindObjectChanged += OnBindObjectChanged;
        }

        protected virtual void OnPostClose(IUIPanel panel)
        {
            ClearSlots();
        }

        protected virtual void Update()
        {
            if (refreshTag)
            {
                refreshTag = false;
                Refresh();
            }
        }

        protected virtual void OnRefresh()
        {
            foreach (var slot in Slots)
            {
                OnSlotSourceChanged?.Invoke(this, slot);
            }
        }

        public void Refresh()
        {
            OnRefresh();
        }

        #region Bind Container

        protected virtual void OnItemChanged(IContainer container, int slotIndex, IContainerItem item)
        {
            refreshTag = true;

            if (TryGetSlotsSet(slotIndex, out var slots))
            {
                foreach (var slot in slots)
                {
                    slot.dataSource = item;
                }
            }

            SetTooltip(slotIndex, item);
        }

        protected virtual void OnItemDirty(IContainer container, int index, IContainerItem item, bool local)
        {
            refreshTag = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateAllSlots()
        {
            if (Panel.BindObjectsManager.GetObject(bindObjectsName) is not IContainer container)
            {
                return;
            }

            foreach (var (slotIndex, item) in container.GetAllItems().Enumerate())
            {
                OnItemChanged(container, slotIndex, item);
            }
        }

        protected virtual void OnContainerSizeChanged(IContainer container, int currentSize)
        {
            if (Panel.BindObjectsManager.GetObject(bindObjectsName) is IContainer && Panel.IsOpened)
            {
                BuildSlots();
            }
        }

        protected virtual void OnBindObjectChanged(string bindName, object bindObject, object parentObject, bool added)
        {
            if (bindName != bindObjectsName)
            {
                return;
            }

            if (bindObject is IContainer container)
            {
                if (added)
                {
                    container.OnAfterItemChangedEvent += OnItemChanged;
                    container.OnItemDirtyEvent += OnItemDirty;
                    container.OnSizeChangedEvent += OnContainerSizeChanged;

                    BuildSlots();
                }
                else
                {
                    container.OnAfterItemChangedEvent -= OnItemChanged;
                    container.OnItemDirtyEvent -= OnItemDirty;
                    container.OnSizeChangedEvent -= OnContainerSizeChanged;

                    ClearSlots();
                }
            }
        }

        #endregion

        #region Slots

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        private readonly Dictionary<SlotVisualElement, int> slotIndicesLookup = new();

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        private readonly Dictionary<int, HashSet<SlotVisualElement>> slotsLookup = new();

        public IReadOnlyCollection<SlotVisualElement> Slots => slotIndicesLookup.Keys;

        [TitleGroup(ComponentNames.RUNTIME)]
        [Button]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void BuildSlots()
        {
            ClearSlots();

            foreach (var distributorConfig in slotDistributorConfigs)
            {
                BuildSlots(distributorConfig);
            }

            UpdateAllSlots();

            Refresh();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ClearSlots()
        {
            foreach (var (slot, slotIndex) in slotIndicesLookup)
            {
                OnClearSlot(slotIndex, slot);
                OnSlotChanged?.Invoke(this, slot, added: false);
            }

            slotIndicesLookup.Clear();
            slotsLookup.Clear();
        }

        protected virtual void OnClearSlot(int slotIndex, SlotVisualElement slot)
        {

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void BuildSlots(ContainerSlotDistributorConfig distributorConfig)
        {
            var parent = GetSlotsParent(distributorConfig);

            var container = (IContainer)Panel.BindObjectsManager.GetObject(bindObjectsName);

            if (distributorConfig.findMethod == SlotFindMethod.BySlotNames)
            {
                BuildSlotsBySlotNames(distributorConfig, parent, container);
                return;
            }

            int index = distributorConfig.StartIndex;
            int count = distributorConfig.Count;

            var slots = ListPool<SlotVisualElement>.Default.Get();
            slots.Clear();

            if (distributorConfig.findMethod == SlotFindMethod.Default)
            {
                slots.AddRange(parent.QueryAll<SlotVisualElement>());
            }
            else if (distributorConfig.findMethod == SlotFindMethod.ByName)
            {
                slots.AddRange(parent.QueryAllByName<SlotVisualElement>(distributorConfig.slotName));
            }
            else
            {
                UnityEngine.Debug.LogError($"Unsupported find method: {distributorConfig.findMethod}!");
                slots.ReturnToDefaultPool();
                return;
            }

            foreach (var slot in slots)
            {
                if (count <= 0)
                {
                    break;
                }

                if (distributorConfig.removeExtraSlots)
                {
                    if (container != null && index >= container.Count)
                    {
                        slot.RemoveFromHierarchy();
                        index++;
                        count--;
                        continue;
                    }
                }

                SetSlot(index, slot);

                index++;
                count--;
            }

            slots.ReturnToDefaultPool();

            if (container == null)
            {
                return;
            }

            if (distributorConfig.isFinite && distributorConfig.autoFill == false)
            {
                return;
            }

            var slotContainer = this.RootVisualElement()
                .QueryStrictly(distributorConfig.ContainerName, nameof(distributorConfig.ContainerName));

            for (int slotIndex = index; slotIndex < container.Count; slotIndex++)
            {
                var newSlot = new SlotVisualElement();

                slotContainer.Add(newSlot);

                SetSlot(slotIndex, newSlot);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private VisualElement GetSlotsParent(ContainerSlotDistributorConfig distributorConfig)
        {
            var root = this.RootVisualElement();

            if (distributorConfig.findMethod == SlotFindMethod.BySlotNames &&
                string.IsNullOrWhiteSpace(distributorConfig.parentName))
            {
                return root;
            }

            return root.QueryStrictly(distributorConfig.parentName, nameof(distributorConfig.parentName));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void BuildSlotsBySlotNames(ContainerSlotDistributorConfig distributorConfig,
            VisualElement parent, IContainer container)
        {
            var slots = ListPool<SlotVisualElement>.Default.Get();
            slots.Clear();
            var parentName = string.IsNullOrWhiteSpace(distributorConfig.parentName)
                ? "panel root"
                : distributorConfig.parentName;

            foreach (var binding in distributorConfig.slotNameBindings)
            {
                if (string.IsNullOrWhiteSpace(binding.slotName))
                {
                    UnityEngine.Debug.LogWarning("Slot name binding has an empty slot name!");
                    continue;
                }

                if (binding.slotIndex < 0)
                {
                    UnityEngine.Debug.LogWarning(
                        $"Slot name binding for slot: {binding.slotName} has an invalid index: {binding.slotIndex}!");
                    continue;
                }

                slots.Clear();
                slots.AddRange(parent.QueryAllByName<SlotVisualElement>(binding.slotName));

                if (slots.Count <= 0)
                {
                    UnityEngine.Debug.LogWarning(
                        $"Failed to find slot: {binding.slotName} under parent: {parentName}!");
                    continue;
                }

                foreach (var slot in slots)
                {
                    if (distributorConfig.removeExtraSlots && container != null &&
                        binding.slotIndex >= container.Count)
                    {
                        slot.RemoveFromHierarchy();
                        continue;
                    }

                    SetSlot(binding.slotIndex, slot);
                }
            }

            slots.ReturnToDefaultPool();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void SetSlot(int slotIndex, SlotVisualElement slot)
        {
            if (slotIndicesLookup.TryAdd(slot, slotIndex) == false)
            {
                UnityEngine.Debug.LogWarning($"Slot with index: {slotIndex} already exists in slotIndicesLookup!");
                return;
            }

            var set = slotsLookup.GetOrCreate(slotIndex);

            if (set.Add(slot) == false)
            {
                UnityEngine.Debug.LogWarning($"Slot with index: {slotIndex} already exists in slotsLookup!");
                return;
            }

            OnSetSlot(slotIndex, slot);

            OnSlotChanged?.Invoke(this, slot, added: true);
        }

        protected virtual void OnSetSlot(int slotIndex, SlotVisualElement slot)
        {

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool TryGetSlotsSet(int slotIndex, out HashSet<SlotVisualElement> slots)
        {
            if (slotsLookup.TryGetValue(slotIndex, out slots))
            {
                return slots.Count > 0;
            }

            // UnityEngine.Debug.LogWarning($"Failed to find slots with index: {slotIndex} in {nameof(slotsLookup)}!");

            return false;
        }

        public bool TryGetSlots(int slotIndex, out IReadOnlyCollection<SlotVisualElement> slots)
        {
            if (slotsLookup.TryGetValue(slotIndex, out var slotsSet))
            {
                slots = slotsSet;
                return slotsSet.Count > 0;
            }

            slots = null;
            return false;
        }

        public bool TryGetContainerAndIndex(SlotVisualElement slot, out IContainer container, out int slotIndex)
        {
            container = (IContainer)Panel.BindObjectsManager.GetObject(bindObjectsName);
            return slotIndicesLookup.TryGetValue(slot, out slotIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool TryGetSlotIndexWithWarning(SlotVisualElement slot, out int slotIndex)
        {
            if (slotIndicesLookup.TryGetValue(slot, out slotIndex))
            {
                return true;
            }

            UnityEngine.Debug.LogWarning($"Failed to find slot index for slot: {slot} in {nameof(slotIndicesLookup)}!");

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void SetTooltip(int slotIndex, object tooltipData)
        {
            if (TryGetSlotsSet(slotIndex, out var slots))
            {
                foreach (var slot in slots)
                {
                    slot.tooltipManager.SetTooltip(tooltipData);
                }
            }
        }

        #endregion
    }
}
