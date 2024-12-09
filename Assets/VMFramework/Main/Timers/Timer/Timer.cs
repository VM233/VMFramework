using System;
using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.Timers
{
    public partial class Timer : ITimer
    {
        private readonly Action<Timer> onTimed;
        private readonly Action<Timer> onStopped;
        private double priority;
        
        public double ExpectedTime => priority;
        
        public double StoppedTime { get; private set; }
        
        public double StartedTime { get; private set; }

        public Timer(Action<Timer> onTimed, Action<Timer> onStopped = null)
        {
            this.onTimed = onTimed;
            this.onStopped = onStopped;
        }

        #region Interface Implementation

        double IGenericPriorityQueueNode<double>.Priority
        {
            get => priority;
            set => priority = value;
        }

        int IGenericPriorityQueueNode<double>.QueueIndex { get; set; }

        long IGenericPriorityQueueNode<double>.InsertionIndex { get; set; }

        void ITimer.OnStart(double startedTime, double expectedTime)
        {
            this.StartedTime = startedTime;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ITimer.OnTimed()
        {
            onTimed?.Invoke(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ITimer.OnStopped(double stoppedTime)
        {
            this.StoppedTime = stoppedTime;
            onStopped?.Invoke(this);
        }

        #endregion
    }
}