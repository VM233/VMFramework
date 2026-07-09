using System.Collections.Generic;

namespace VMFramework.UI
{
    public interface ISlotsPanelModifier
    {
        public delegate void OnSlotSourceChangedHandler(ISlotsPanelModifier modifier, SlotVisualElement slot);

        public delegate void OnSlotChangedHandler(ISlotsPanelModifier modifier, SlotVisualElement slot, bool added);

        public IReadOnlyCollection<SlotVisualElement> Slots { get; }

        public event OnSlotSourceChangedHandler OnSlotSourceChanged;

        public event OnSlotChangedHandler OnSlotChanged;
    }
}