using UnityEngine;

namespace VMFramework.UI
{
    public abstract class TracingPanelModifier : PanelModifier, ITracingPanelModifier
    {
        protected TracingPanelModifierConfig TracingModifierConfig => (TracingPanelModifierConfig)GamePrefab;

        protected bool EnableOverflow => TracingModifierConfig.enableScreenOverflow;

        protected bool AutoPivotCorrection => TracingModifierConfig.autoPivotCorrection;

        protected Vector2 DefaultPivot => TracingModifierConfig.defaultPivot;
        
        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnOpenEvent += OnOpen;
            Panel.OnPostCloseEvent += OnPostClose;
        }
        
        protected virtual void OnOpen(IUIPanel panel)
        {
            if (TracingModifierConfig.enableAutoMouseTracing)
            {
                TracingUIManager.StartTracing(this, TracingModifierConfig.persistentTracing);
            }
        }
        
        protected virtual void OnPostClose(IUIPanel panel)
        {
            TracingUIManager.StopTracing(this);
        }
        
        public abstract bool TryUpdatePosition(Vector2 screenPosition);
        
        public abstract void SetPivot(Vector2 pivot);
    }
}