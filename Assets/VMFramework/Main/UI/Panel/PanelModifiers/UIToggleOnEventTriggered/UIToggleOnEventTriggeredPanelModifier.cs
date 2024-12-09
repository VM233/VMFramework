using VMFramework.GameEvents;

namespace VMFramework.UI
{
    public sealed class UIToggleOnEventTriggeredPanelModifier : UIToolkitPanelModifier
    {
        private UIToggleOnEventTriggeredPanelModifierConfig ModifierConfig =>
            (UIToggleOnEventTriggeredPanelModifierConfig)GamePrefab;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            GameEventManager.AddCallback(ModifierConfig.uiToggleGameEventID, Panel.Toggle, GameEventPriority.TINY);
        }

        protected override void OnClear()
        {
            base.OnClear();
            
            GameEventManager.RemoveCallback(ModifierConfig.uiToggleGameEventID, Panel.Toggle);
        }
    }
}