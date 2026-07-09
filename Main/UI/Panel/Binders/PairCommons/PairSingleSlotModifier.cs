using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class PairSingleSlotModifier : PanelModifier, ISlotsPanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName]
        [IsNotNullOrEmpty]
        public string bindObjectsName;

        [TitleGroup(ComponentNames.CONFIG)]
        [VisualElementPathSettings(typeof(SlotVisualElement), IsFromLocalProvider = true)]
        [IsNotNullOrEmpty]
        public VisualElementPath slotPath = new();

        public IReadOnlyCollection<SlotVisualElement> Slots => slots.Values;

        public event ISlotsPanelModifier.OnSlotSourceChangedHandler OnSlotSourceChanged;
        public event ISlotsPanelModifier.OnSlotChangedHandler OnSlotChanged;

        protected readonly Dictionary<VisualElement, SlotVisualElement> slots = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            this.UIToolkitPanel().BindVisualElementsManager.OnBindVisualElementChanged += OnBindVisualElementChanged;
        }

        protected virtual void OnBindVisualElementChanged(string bindName, object bindObject,
            VisualElement visualElement, bool added)
        {
            if (bindName != bindObjectsName)
            {
                return;
            }

            if (added)
            {
                var slot = slotPath.MandatoryQuery<SlotVisualElement>(visualElement, nameof(slotPath));
                slot.dataSource = bindObject;
                slot.tooltipManager.SetTooltip(bindObject);
                slots.Add(visualElement, slot);

                OnSlotChanged?.Invoke(this, slot, added: true);
                OnSlotSourceChanged?.Invoke(this, slot);
            }
            else
            {
                slots.Remove(visualElement, out var slot);
                OnSlotChanged?.Invoke(this, slot, added: false);
            }
        }
    }
}