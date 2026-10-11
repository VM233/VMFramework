using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Containers;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class GeneralContainerItemSlotFilterAdderModifierBase : PanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        public bool autoAddSlotFilter = true;

        [TitleGroup(ComponentNames.CONFIG)]
        [GamePrefabID(typeof(SlotFilterConfig))]
        [IsNotNullOrEmpty]
        public string filterID;

        protected IContainerSlotsModifier slotsModifier;
        private SlotGlobalFiltersManager filtersSource;

        protected EventCallback<PointerEnterEvent> onPointerEnterFunc;
        protected EventCallback<PointerLeaveEvent> onPointerLeaveFunc;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            filtersSource = SlotGlobalFiltersManager.Instance;
            slotsModifier = GetComponent<IContainerSlotsModifier>();
            onPointerEnterFunc = OnPointerEnter;
            onPointerLeaveFunc = OnPointerLeave;
            slotsModifier.OnSlotChanged += OnSlotChanged;
            foreach (var slot in slotsModifier.Slots)
                OnSlotChanged(slotsModifier, slot, added: true);
        }

        protected override void OnDeinitialize()
        {
            var failures = new UIPanelReleaseFailures(nameof(GeneralContainerItemSlotFilterAdderModifierBase));
            slotsModifier.OnSlotChanged -= OnSlotChanged;
            foreach (var slot in slotsModifier.Slots)
                failures.Attempt("slot-filter.retire", () => OnSlotChanged(slotsModifier, slot, added: false));
            slotsModifier = null;
            filtersSource = null;
            failures.Attempt("base.deinitialize", () => base.OnDeinitialize());
            failures.ThrowIfFailed();
        }

        protected virtual void OnSlotChanged(ISlotsPanelModifier modifier, SlotVisualElement slot, bool added)
        {
            if (added)
            {
                slot.RegisterCallback(onPointerEnterFunc);
                slot.RegisterCallback(onPointerLeaveFunc);
            }
            else
            {
                try { filtersSource.RemoveFilter(filterID, slot); }
                finally
                {
                    slot.UnregisterCallback(onPointerEnterFunc);
                    slot.UnregisterCallback(onPointerLeaveFunc);
                }
            }
        }

        protected virtual bool CanAddGeneralSlotFilter(SlotVisualElement slot, IContainer container, int slotIndex)
        {
            return true;
        }

        protected virtual void OnPointerEnter(PointerEnterEvent evt)
        {
            if (autoAddSlotFilter == false)
            {
                return;
            }

            var slot = (SlotVisualElement)evt.target;

            if (slotsModifier.TryGetContainerAndIndex(slot, out var container, out var slotIndex) == false)
            {
                return;
            }

            if (container == null)
            {
                return;
            }

            if (CanAddGeneralSlotFilter(slot, container, slotIndex) == false)
            {
                return;
            }

            var filter = new GeneralContainerItemSlotFilter(container, slotIndex);
            filtersSource.AddFilter(filterID, slot, filter);
        }

        protected virtual void OnPointerLeave(PointerLeaveEvent evt)
        {
            var slot = (SlotVisualElement)evt.target;
            filtersSource.RemoveFilter(filterID, slot);
        }
    }
}
