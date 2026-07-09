using UnityEngine;
using UnityEngine.Profiling;
using VMFramework.Core;
using VMFramework.Procedure;

namespace VMFramework.Timers
{
    [ManagerCreationProvider(ManagerType.TimerCore)]
    [DisallowMultipleComponent]
    public partial class LogicTickTimerManager : ManagerBehaviour<ILogicTickTimerManager>, ILogicTickTimerManager
    {
        public const int INITIAL_QUEUE_SIZE = 100;
        public const int QUEUE_SIZE_GAP = 50;
        
        protected ulong Tick => LogicTickManager.Instance.Tick;
        
        protected readonly GenericArrayPriorityQueue<ITimer<ulong>, ulong> queue = new(INITIAL_QUEUE_SIZE);

        protected override void Awake()
        {
            base.Awake();

            queue.Clear();
        }

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();
            
            LogicTickManager.Instance.OnTick += OnTick;
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
    }
}