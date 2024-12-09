using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Procedure;

namespace VMFramework.Timers
{
    [ManagerCreationProvider(ManagerType.TimerCore)]
    [DisallowMultipleComponent]
    public sealed partial class TimerManager : ManagerBehaviour<TimerManager>
    {
        private const int INITIAL_QUEUE_SIZE = 100;
        private const int QUEUE_SIZE_GAP = 50;
        
        private static readonly GenericArrayPriorityQueue<ITimer, double> queue = new(INITIAL_QUEUE_SIZE);
        
        private static double _currentTime = 0;

        public static double currentTime => _currentTime;

        /// <summary>
        /// Adds a timer to the queue with a delay.
        /// O(log n)
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="delay"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(ITimer timer, float delay)
        {
            int capacity = queue.capacity;
            if (queue.count >= capacity)
            {
                queue.Resize(capacity + QUEUE_SIZE_GAP);
            }
            
            double expectedTime = _currentTime + delay;
            queue.Enqueue(timer, expectedTime);
            
            timer.OnStart(_currentTime, expectedTime);
        }

        /// <summary>
        /// Removes a timer from the queue.
        /// O(log n)
        /// if the timer is not in the queue, it will throw an exception.
        /// So it's better to use Contains() before calling this method.
        /// </summary>
        /// <param name="timer"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Stop(ITimer timer)
        {
            queue.Remove(timer);
            
            timer.OnStopped(_currentTime);
        }
        
        /// <summary>
        /// Checks if a timer is in the queue.
        /// O(1)
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(ITimer timer)
        {
            return queue.Contains(timer);
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryStop(ITimer timer)
        {
            if (queue.Contains(timer))
            {
                Stop(timer);
                return true;
            }
            
            return false;
        }

        private void Update()
        {
            _currentTime += Time.deltaTime;

            while (queue.count > 0)
            {
                if (_currentTime < queue.first.Priority)
                {
                    break;
                }
                
                ITimer timer = queue.Dequeue();
                
                timer.OnTimed();
            }
        }
    }
}