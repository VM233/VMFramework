using UnityEngine;
using UnityEngine.Profiling;
using VMFramework.Core;
using VMFramework.Procedure;
#if UNITY_EDITOR && ODIN_INSPECTOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
#endif

namespace VMFramework.Timers
{
    [ManagerCreationProvider(ManagerType.TimerCore)]
    [DisallowMultipleComponent]
    public class LogicTickTimerManager : ManagerBehaviour<ILogicTickTimerManager>, ILogicTickTimerManager
    {
        public const int INITIAL_QUEUE_SIZE = 100;
        public const int QUEUE_SIZE_GAP = 50;
        
        private ILogicTickManager tickSource;

        protected ulong Tick => tickSource.Tick;
        
        protected readonly GenericArrayPriorityQueue<ITimer<ulong>, ulong> queue = new(INITIAL_QUEUE_SIZE);

        protected override void Awake()
        {
            base.Awake();

            queue.Clear();
        }

        protected override void OnBeforeInitStart()
        {
            tickSource = LogicTickManager.Instance;
            base.OnBeforeInitStart();
            
            tickSource.OnTick += OnTick;
        }

        protected override void OnDestroy()
        {
            try
            {
                if (tickSource is not null)
                {
                    tickSource.OnTick -= OnTick;
                }
            }
            finally
            {
                base.OnDestroy();
            }
        }

        protected virtual void OnTick()
        {
            while (queue.Count > 0)
            {
                if (Tick < queue.First.Priority)
                {
                    break;
                }
                
                var timer = queue.Dequeue();
                
                Profiler.BeginSample($"{timer.GetType().Name}.OnTimed");
                timer.OnTimed();
                Profiler.EndSample();
            }
        }

        public void Add(ITimer<ulong> timer, uint delay)
        {
            if (delay <= 0)
            {
                UnityEngine.Debug.LogWarning($"Delay : {delay} must be greater than 0.");
            }
            
            int capacity = queue.Capacity;
            if (queue.Count >= capacity)
            {
                queue.Resize(capacity + QUEUE_SIZE_GAP);
            }
            
            ulong expectedTick = Tick + delay;
            queue.Enqueue(timer, expectedTick);
            
            timer.OnStart(Tick, expectedTick);
        }

        public void Stop(ITimer<ulong> timer)
        {
            queue.Remove(timer);
            
            timer.OnStopped(Tick);
        }
        
        public bool Contains(ITimer<ulong> timer)
        {
            return queue.Contains(timer);
        }
    
        public bool TryStop(ITimer<ulong> timer)
        {
            if (queue.Contains(timer))
            {
                Stop(timer);
                return true;
            }
            
            return false;
        }

        public bool TryStopAndAdd(ITimer<ulong> timer, uint delay)
        {
            var result = TryStop(timer);
            Add(timer, delay);
            return result;
        }

#if UNITY_EDITOR && ODIN_INSPECTOR
        [ShowInInspector]
        [EnableGUI]
        private List<ITimer<ulong>> allTimers => queue.ToList();

        [Button]
        private bool ContainsTimer(ITimer<ulong> timer)
        {
            return Contains(timer);
        }
#endif
    }
}
