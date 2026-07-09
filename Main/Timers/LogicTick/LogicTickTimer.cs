using System;
using Sirenix.OdinInspector;
using VMFramework.Core;

namespace VMFramework.Timers
{
    public class LogicTickTimer : ITimer<ulong>
    {
        [ShowInInspector]
        public readonly object owner;
        
        protected Action timedCallback;
        protected Action stoppedCallback;
        
        public LogicTickTimer(object owner, Action timedCallback, Action stoppedCallback = null)
        {
            this.owner = owner;
            this.timedCallback = timedCallback;
            this.stoppedCallback = stoppedCallback;
        }
        
        void ITimer<ulong>.OnStopped(ulong stoppedTime)
        {
            stoppedCallback?.Invoke();
        }

        void ITimer<ulong>.OnTimed()
        {
            timedCallback?.Invoke();
        }

        #region Proprity Queue Node

        ulong IGenericPriorityQueueNode<ulong>.Priority { get; set; }

        int IGenericPriorityQueueNode<ulong>.QueueIndex { get; set; }

        long IGenericPriorityQueueNode<ulong>.InsertionIndex { get; set; }

        #endregion
    }
}