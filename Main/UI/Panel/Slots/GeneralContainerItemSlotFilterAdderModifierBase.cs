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

        protected EventCallback<PointerEnterEvent> onPointerEnterFunc;
        protected EventCallback<PointerLeaveEvent> onPointerLeaveFunc;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            slotsModifier = GetComponent<IContainerSlotsModifier>();

            slotsModifier.OnSlotChanged+= OnSlotChanged;

            onPointerEnterFunc = OnPointerEnter;
            onPointerLeaveFunc = OnPointerLeave;
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
                SlotGlobalFiltersManager.Instance.RemoveFilter(filterID, slot);
                slot.UnregisterCallback(onPointerEnterFunc);
                slot.UnregisterCallback(onPointerLeaveFunc);
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
            SlotGlobalFiltersManager.Instance.AddFilter(filterID, slot, filter);
        }

        protected virtual void OnPointerLeave(PointerLeaveEvent evt)
        {
            var slot = (SlotVisualElement)evt.target;
            SlotGlobalFiltersManager.Instance.RemoveFilter(filterID, slot);
        }
    }
}