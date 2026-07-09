using System.Collections.Generic;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI
{
    public class SlotFilterDisplayModifier : PanelModifier
    {
        protected ISlotsPanelModifier slotsModifier;

        protected readonly Dictionary<string, bool?> matchResults = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnOpen += OnOpen;
            Panel.OnPostClose += OnPostClose;

            slotsModifier = GetComponent<ISlotsPanelModifier>();
            slotsModifier.OnSlotSourceChanged += OnSlotSourceChanged;
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            SlotGlobalFiltersManager.Instance.OnFilterChanged += OnFilterChanged;
        }

        protected virtual void OnPostClose(IUIPanel panel)
        {
            SlotGlobalFiltersManager.Instance.OnFilterChanged -= OnFilterChanged;
        }

        protected virtual void OnSlotSourceChanged(ISlotsPanelModifier modifier, SlotVisualElement slot)
        {
            RefreshSlot(slot);
        }

        protected virtual void OnFilterChanged()
        {
            foreach (var slot in slotsModifier.Slots)
            {
                RefreshSlot(slot);
            }
        }

        protected virtual void RefreshSlot(SlotVisualElement slot)
        {
            matchResults.Clear();
            
            SlotGlobalFiltersManager.Instance.IsMatch(slot, matchResults);

            foreach (var (filterID, isMatch) in matchResults)
            {
                if (GamePrefabManager.TryGetGamePrefab(filterID, out SlotFilterConfig filterConfig) == false)
                {
                    continue;
                }

                if (isMatch.HasValue == false)
                {
                    slot.styleSheets.RemoveIfNotNull(filterConfig.matchedStyleSheet);
                    slot.styleSheets.RemoveIfNotNull(filterConfig.unmatchedStyleSheet);
                    slot.styleSheets.AddIfNotNull(filterConfig.nonFilteredStyleSheet);
                    continue;
                }

                if (isMatch.Value)
                {
                    slot.styleSheets.AddIfNotNull(filterConfig.matchedStyleSheet);
                    slot.styleSheets.RemoveIfNotNull(filterConfig.unmatchedStyleSheet);
                }
                else
                {
                    slot.styleSheets.RemoveIfNotNull(filterConfig.matchedStyleSheet);
                    slot.styleSheets.AddIfNotNull(filterConfig.unmatchedStyleSheet);
                }

                slot.styleSheets.RemoveIfNotNull(filterConfig.nonFilteredStyleSheet);
            }
        }
    }
}