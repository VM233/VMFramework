using VMFramework.Containers;

namespace VMFramework.UI
{
    public readonly struct GeneralContainerItemSlotFilter : ISlotFilter
    {
        public readonly IContainer container;
        public readonly int slotIndex;

        public GeneralContainerItemSlotFilter(IContainer container, int slotIndex)
        {
            this.container = container;
            this.slotIndex = slotIndex;
        }

        public bool? IsMatch(SlotVisualElement slot)
        {
            if (slot.dataSource is not IContainerItem item)
            {
                return null;
            }

            return container.IsMatchFilters(slotIndex, item, null);
        }
    }
}