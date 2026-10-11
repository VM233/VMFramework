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

        private TracingUIManager tracingSource;
        
        protected override void OnInitialize()
        {
            base.OnInitialize();
            SubscribeTracingLifecycle();
        }

        protected virtual void SubscribeTracingLifecycle()
        {
            Panel.OnOpen += OnOpen;
            Panel.OnPostClose += OnPostClose;
        }

        protected virtual void UnsubscribeTracingLifecycle()
        {
            Panel.OnOpen -= OnOpen;
            Panel.OnPostClose -= OnPostClose;
        }

        protected override void OnDeinitialize()
        {
            var failures = new UIPanelReleaseFailures(nameof(TracingPanelModifier));
            failures.Attempt("tracing-lifecycle.unsubscribe", UnsubscribeTracingLifecycle);
            failures.Attempt("tracing-source.release", ReleaseTracingSource);
            failures.Attempt("base.deinitialize", base.OnDeinitialize);
            failures.ThrowIfFailed();
        }
        
        protected virtual void OnOpen(IUIPanel panel)
        {
            tracingSource = TracingUIManager.Instance;
            if (enableAutoMouseTracing)
            {
                tracingSource.StartTracing(this, persistentTracing);
            }
        }
        
        protected virtual void OnPostClose(IUIPanel panel)
        {
            ReleaseTracingSource();
        }

        private void ReleaseTracingSource()
        {
            var source = tracingSource;
            tracingSource = null;
            if (!ReferenceEquals(source, null)) source.StopTracing(this);
        }
        
        public abstract bool TryUpdateScreenPosition(Vector2 screenPosition);
        
        public abstract void SetPivot(Vector2 pivot);
    }
}
