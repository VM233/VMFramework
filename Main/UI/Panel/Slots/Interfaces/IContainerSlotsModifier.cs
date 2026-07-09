using System.Collections.Generic;
using VMFramework.Containers;

namespace VMFramework.UI
{
    public interface IContainerSlotsModifier : ISlotsPanelModifier
    {
        public bool TryGetSlots(int slotIndex, out IReadOnlyCollection<SlotVisualElement> slots);

        public bool TryGetContainerAndIndex(SlotVisualElement slot, out IContainer container, out int slotIndex);
    }
}