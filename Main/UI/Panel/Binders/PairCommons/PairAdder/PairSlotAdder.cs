using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class PairSlotAdder : PanelModifier, ISlotsPanelModifier, IVisualElementGenerator
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName]
        [IsNotNullOrEmpty]
        public List<string> bindObjectsNames = new();

        [TitleGroup(ComponentNames.CONFIG)]
        [BindObjectsName]
        [IsNotNullOrEmpty]
        public string targetBindObjectsName;

        [TitleGroup(ComponentNames.CONFIG)]
        [IsNotNullOrEmpty]
        public VisualElementPath containerPath = new();

        [TitleGroup(ComponentNames.CONFIG)]
        public bool clearContainerOnOpen = true;

        [TitleGroup(ComponentNames.CONFIG)]
        [HideIf(nameof(clearContainerOnOpen))]
        public bool useExistingSlots = true;

        [TitleGroup(ComponentNames.CONFIG)]
        public bool enableTooltip = true;

        public IReadOnlyCollection<SlotVisualElement> Slots => slots;

        public event ISlotsPanelModifier.OnSlotSourceChangedHandler OnSlotSourceChanged;

        public event ISlotsPanelModifier.OnSlotChangedHandler OnSlotChanged;

        public event IVisualElementGenerator.GenerateVisualElementHandler OnGenerateVisualElement;

        protected VisualElement container;

        protected readonly HashSet<SlotVisualElement> slots = new();
        protected readonly List<SlotVisualElement> existingSlots = new();
        protected readonly HashSet<SlotVisualElement> usedExistingSlots = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            this.UIToolkitPanel().OnGenerateVisualElement += OnRootGenerate;
            Panel.OnOpen += OnOpen;
            Panel.OnPostClose += OnPostClose;
            Panel.BindObjectsManager.OnBindObjectPreChanged += OnBindObjectPreChanged;
        }

        protected virtual void OnRootGenerate(IVisualElementGenerator generator, VisualElement root)
        {
            if (clearContainerOnOpen)
            {
                var container = containerPath.MandatoryQuery(this.RootVisualElement(), nameof(containerPath));
                container.Clear();
            }
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            container = containerPath.MandatoryQuery(this.RootVisualElement(), nameof(containerPath));

            slots.Clear();
            existingSlots.Clear();
            usedExistingSlots.Clear();

            if (clearContainerOnOpen == false && useExistingSlots)
            {
                foreach (var visualElement in container.Children())
                {
                    if (visualElement is SlotVisualElement slot)
                    {
                        slots.Add(slot);
                        existingSlots.Add(slot);

                        slot.dataSource = null;
                        slot.tooltipManager.SetTooltip(null);
                        OnSlotChanged?.Invoke(this, slot, added: true);
                        OnSlotSourceChanged?.Invoke(this, slot);
                    }
                }
            }
        }

        protected virtual void OnPostClose(IUIPanel panel)
        {
            foreach (var slot in existingSlots)
            {
                slot.RemoveFromHierarchy();
                OnSlotChanged?.Invoke(this, slot, added: false);
            }

            slots.Clear();
            existingSlots.Clear();
            usedExistingSlots.Clear();
        }

        protected virtual void OnBindObjectPreChanged(string bindName, object bindObject, object parentObject,
            bool added)
        {
            if (bindObjectsNames.Contains(bindName) == false)
            {
                return;
            }

            if (added)
            {
                AddItemEntry(bindObject);
            }
            else
            {
                RemoveItemEntry(bindObject);
            }
        }

        protected virtual void AddItemEntry(object value)
        {
            foreach (var existingSlot in existingSlots)
            {
                if (usedExistingSlots.Contains(existingSlot))
                {
                    continue;
                }

                existingSlot.dataSource = value;

                if (enableTooltip)
                {
                    existingSlot.tooltipManager.SetTooltip(value);
                }

                this.UIToolkitPanel().BindVisualElementsManager.Add(targetBindObjectsName, value, existingSlot);
                usedExistingSlots.Add(existingSlot);

                OnSlotSourceChanged?.Invoke(this, existingSlot);

                return;
            }

            var root = GenerateVisualElement();
            var slot = (SlotVisualElement)root;
            slot.dataSource = value;

            if (enableTooltip)
            {
                slot.tooltipManager.SetTooltip(value);
            }

            container.Add(slot);
            slots.Add(slot);

            OnSlotChanged?.Invoke(this, slot, added: true);
            OnSlotSourceChanged?.Invoke(this, slot);
        }

        protected virtual void RemoveItemEntry(object value)
        {
            if (this.UIToolkitPanel().BindVisualElementsManager.Remove(targetBindObjectsName, value, out var slotObject))
            {
                var slot = (SlotVisualElement)slotObject;
                slot.dataSource = null;
                slot.tooltipManager.SetTooltip(null);

                if (existingSlots.Contains(slot) == false)
                {
                    slot.RemoveFromHierarchy();
                    if (slots.Remove(slot))
                    {
                        OnSlotChanged?.Invoke(this, slot, added: false);
                    }
                }
                else
                {
                    usedExistingSlots.Remove(slot);
                    OnSlotSourceChanged?.Invoke(this, slot);
                }
            }
        }

        public virtual VisualElement GenerateVisualElement()
        {
            var slot = new SlotVisualElement();
            OnGenerateVisualElement?.Invoke(this, slot);
            return slot;
        }
    }
}