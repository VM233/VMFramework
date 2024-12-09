using VMFramework.GameEvents;

namespace VMFramework.UI
{
    public sealed class UICloseOnEventTriggeredPanelModifier : UIToolkitPanelModifier
    {
        private UICloseOnEventTriggeredPanelModifierConfig ModifierConfig =>
            (UICloseOnEventTriggeredPanelModifierConfig)GamePrefab;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            GameEventManager.AddCallback<bool>(ModifierConfig.uiCloseGameEventID, OnUIPanelClose,
                GameEventPriority.TINY);
        }

        protected override void OnClear()
        {
            base.OnClear();

            GameEventManager.RemoveCallback<bool>(ModifierConfig.uiCloseGameEventID, OnUIPanelClose);
        }

        private void OnUIPanelClose(bool value)
        {
            if (value)
            {
                Panel.Close();
            }
        }
    }
}