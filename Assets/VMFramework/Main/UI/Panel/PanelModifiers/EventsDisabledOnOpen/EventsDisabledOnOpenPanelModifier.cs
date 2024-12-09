using VMFramework.GameEvents;

namespace VMFramework.UI
{
    public sealed class EventsDisabledOnOpenPanelModifier : UIToolkitPanelModifier
    {
        private EventsDisabledOnOpenPanelModifierConfig ModifierConfig =>
            (EventsDisabledOnOpenPanelModifierConfig)GamePrefab;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            Panel.OnOpenEvent += OnOpen;
            Panel.OnPostCloseEvent += OnClose;
        }
        
        protected override void OnClear()
        {
            base.OnClear();
            
            GameEventManager.Enable(ModifierConfig.gameEventDisabledOnOpen, Panel);
        }

        public void OnOpen(IUIPanel panel)
        {
            GameEventManager.Disable(ModifierConfig.gameEventDisabledOnOpen, panel);
        }

        public void OnClose(IUIPanel panel)
        {   
            GameEventManager.Enable(ModifierConfig.gameEventDisabledOnOpen, panel);
        }
    }
}