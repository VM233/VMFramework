using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Containers;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class ContainerItemSlotRenderModifier : PanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        public bool considerIconTypes = true;

        [TitleGroup(ComponentNames.CONFIG)]
        [ShowIf(nameof(considerIconTypes))]
        [CommonPreset(IconMeta.TYPE_PRESET_KEY)]
        [IsNotNullOrEmpty]
        public List<string> iconTypes = new();

        protected ISlotsPanelModifier slotsModifier;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            slotsModifier = GetComponent<ISlotsPanelModifier>();
            slotsModifier.OnSlotSourceChanged += OnSlotSourceChanged;
        }

        protected virtual void OnSlotSourceChanged(ISlotsPanelModifier modifier, SlotVisualElement slot)
        {
            if (slot.dataSource == null)
            {
                SetNull(slot);
                return;
            }

            if (slot.dataSource is IContainerItem containerItem)
            {
                SetItem(slot, containerItem);
            }
        }

        protected virtual void SetItem(SlotVisualElement slot, IContainerItem containerItem)
        {
            if (containerItem.Count.GetValue() == 1)
            {
                slot.Description = string.Empty;
            }
            else
            {
                slot.Description = containerItem.Count.ToString();
            }

            slot.Icon = GetIcon(containerItem);
        }

        protected virtual Sprite GetIcon(IContainerItem containerItem)
        {
            if (considerIconTypes)
            {
                if (containerItem is IIconManagerProvider { IconManager: { } iconManager })
                {
                    foreach (var iconType in iconTypes)
                    {
                        var icon = iconManager.GetIcon(iconType);

                        if (icon != null)
                        {
                            return icon;
                        }
                    }
                }
            }

            if (containerItem is IIconOwner iconOwner)
            {
                return iconOwner.Icon;
            }

            return null;
        }

        protected virtual void SetNull(SlotVisualElement slot)
        {
            slot.Icon = null;
            slot.Description = string.Empty;
        }
    }
}