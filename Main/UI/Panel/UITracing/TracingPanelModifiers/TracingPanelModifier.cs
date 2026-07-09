using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public abstract class TracingPanelModifier : PanelModifier, ITracingPanelModifier
    {
        [SuffixLabel("Left bottom corner is (0, 0)"), TitleGroup(ComponentNames.CONFIG)]
        [MinValue(0), MaxValue(1)]
        public Vector2 defaultPivot = new(0, 1);

        [TitleGroup(ComponentNames.CONFIG)]
        public bool enableScreenOverflow;

        [TitleGroup(ComponentNames.CONFIG)]
        public bool enableAutoMouseTracing = false;

        [TitleGroup(ComponentNames.CONFIG)]
        [ShowIf(nameof(enableAutoMouseTracing))]
        [ToggleButtons("Persistent Tracing", "Single-Shot Tracing")]
        public bool persistentTracing = true;
        
        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnOpen += OnOpen;
            Panel.OnPostClose += OnPostClose;
        }
        
        protected virtual void OnOpen(IUIPanel panel)
        {
            if (enableAutoMouseTracing)
            {
                TracingUIManager.Instance.StartTracing(this, persistentTracing);
            }
        }
        
        protected virtual void OnPostClose(IUIPanel panel)
        {
            TracingUIManager.Instance.StopTracing(this);
        }
        
        public abstract bool TryUpdateScreenPosition(Vector2 screenPosition);
        
        public abstract void SetPivot(Vector2 pivot);
    }
}