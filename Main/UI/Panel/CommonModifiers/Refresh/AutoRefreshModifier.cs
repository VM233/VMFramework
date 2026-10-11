using Sirenix.OdinInspector;
using UnityEngine.Profiling;
using VMFramework.Core;
using VMFramework.Timers;

namespace VMFramework.UI
{
    public class AutoRefreshModifier : PanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [MinValue(0)]
        public float refreshInterval = 0.1f;
        
        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected IRefreshable[] refreshables;

        private ITimerManager timerSource;
        private Timer<double> refreshTimer;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            refreshables = GetComponentsInChildren<IRefreshable>(includeInactive: true);
            
            Panel.OnOpen += OnOpen;
            Panel.OnPostClose += OnPostClose;
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            var source = TimerManager.Instance;
            var timer = new Timer<double> { OnTimed = OnRefreshTimer };
            source.Add(timer, refreshInterval);
            timerSource = source;
            refreshTimer = timer;
        }
        
        protected virtual void OnPostClose(IUIPanel panel)
        {
            var source = timerSource;
            var timer = refreshTimer;
            timerSource = null;
            refreshTimer = null;
            if (timer != null)
            {
                try { source.TryStop(timer); }
                finally { timer.OnTimed = null; }
            }
        }

        protected override void OnDeinitialize()
        {
            Panel.OnOpen -= OnOpen;
            Panel.OnPostClose -= OnPostClose;
            try { OnPostClose(Panel); }
            finally { base.OnDeinitialize(); }
        }

        private void OnRefreshTimer(Timer<double> timer)
        {
            if (!ReferenceEquals(refreshTimer, timer)) return;
            var source = timerSource;
            OnTimed();
            if (ReferenceEquals(refreshTimer, timer)) source.Add(timer, refreshInterval);
        }

        protected virtual void OnTimed()
        {
            foreach (var refreshable in refreshables)
            {
                Profiler.BeginSample($"{refreshable.GetType().Name} Refresh");
                try { refreshable.Refresh(); }
                finally { Profiler.EndSample(); }
            }
            
        }
    }
}
