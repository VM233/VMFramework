using System;
using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.Timers
{
    public class Timer<TValue> : ITimer<TValue>
    {
        public Action<Timer<TValue>> OnTimed { get; set; }
        
        public Action<Timer<TValue>> OnStopped { get; set; }
        
        private TValue priority;

        public TValue ExpectedTime => priority;

        public TValue StoppedTime { get; private set; }

        public TValue StartedTime { get; private set; }

        #region Interface Implementation

        TValue IGenericPriorityQueueNode<TValue>.Priority
        {
            get => priority;
            set => priority = value;
        }

        int IGenericPriorityQueueNode<TValue>.QueueIndex { get; set; }

        long IGenericPriorityQueueNode<TValue>.InsertionIndex { get; set; }

        void ITimer<TValue>.OnStart(TValue startedTime, TValue expectedTime)
        {
            StartedTime = startedTime;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ITimer<TValue>.OnTimed()
        {
            OnTimed?.Invoke(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ITimer<TValue>.OnStopped(TValue stoppedTime)
        {
            StoppedTime = stoppedTime;
            OnStopped?.Invoke(this);
        }

        #endregion
    }
}