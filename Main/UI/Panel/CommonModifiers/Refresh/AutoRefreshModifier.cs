using Sirenix.OdinInspector;
using UnityEngine.Profiling;
using VMFramework.Core;
using VMFramework.Timers;

namespace VMFramework.UI
{
    public class AutoRefreshModifier : PanelModifier, ITimer<double>
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [MinValue(0)]
        public float refreshInterval = 0.1f;
        
        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected IRefreshable[] refreshables;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            refreshables = GetComponentsInChildren<IRefreshable>(includeInactive: true);
            
            Panel.OnOpen += OnOpen;
            Panel.OnPostClose += OnPostClose;
        }

        protected virtual void OnOpen(IUIPanel panel)
        {
            TimerManager.Instance.Add(this, refreshInterval);
        }
        
        protected virtual void OnPostClose(IUIPanel panel)
        {
            TimerManager.Instance.TryStop(this);
        }

        protected virtual void OnTimed()
        {
            foreach (var refreshable in refreshables)
            {
                Profiler.BeginSample($"{refreshable.GetType().Name} Refresh");
                refreshable.Refresh();
                Profiler.EndSample();
            }
            
            TimerManager.Instance.Add(this, refreshInterval);
        }
        
        void ITimer<double>.OnTimed()
        {
            OnTimed();
        }

        #region Proprity Queue Node

        double IGenericPriorityQueueNode<double>.Priority { get; set; }

        int IGenericPriorityQueueNode<double>.QueueIndex { get; set; }

        long IGenericPriorityQueueNode<double>.InsertionIndex { get; set; }

        #endregion
    }
}