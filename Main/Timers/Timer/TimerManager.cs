using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Profiling;
using VMFramework.Core;
using VMFramework.Procedure;

namespace VMFramework.Timers
{
    /// <inheritdoc cref="ITimerManager"/>
    [ManagerCreationProvider(ManagerType.TimerCore)]
    [DisallowMultipleComponent]
    public partial class TimerManager : ManagerBehaviour<ITimerManager>, ITimerManager
    {
        public const int INITIAL_QUEUE_SIZE = 100;
        public const int QUEUE_SIZE_GAP = 50;
        
        public double CurrentTime => currentTime;
        
        protected readonly GenericArrayPriorityQueue<ITimer<double>, double> queue = new(INITIAL_QUEUE_SIZE);
        
        [ShowInInspector]
        protected double currentTime = 0;

        protected override void Awake()
        {
            base.Awake();

            currentTime = 0;
            queue.Clear();
        }
        
        protected virtual void Update()
        {
            currentTime += Time.deltaTime;

            while (queue.Count > 0)
            {
                if (currentTime < queue.First.Priority)
                {
                    break;
                }
                
                var timer = queue.Dequeue();
                
                Profiler.BeginSample($"{timer.GetType().Name}.OnTimed");
                timer.OnTimed();
                Profiler.EndSample();
            }
        }
        
        public void Add(ITimer<double> timer, float delay)
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
            
            double expectedTime = currentTime + delay;
            queue.Enqueue(timer, expectedTime);
            
            timer.OnStart(currentTime, expectedTime);
        }
        
        public void Stop(ITimer<double> timer)
        {
            queue.Remove(timer);
            
            timer.OnStopped(currentTime);
        }
        
        public bool Contains(ITimer<double> timer)
        {
            return queue.Contains(timer);
        }
    
        public bool TryStop(ITimer<double> timer)
        {
            if (queue.Contains(timer))
            {
                Stop(timer);
                return true;
            }
            
            return false;
        }

        public bool TryStopAndAdd(ITimer<double> timer, float delay)
        {
            var result = TryStop(timer);
            Add(timer, delay);
            return result;
        }
    }
}