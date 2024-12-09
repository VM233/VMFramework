using System;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Timers;

namespace VMFramework.Examples
{
    public class SpellTimerExample : ITimer
    {
        private double expectedTime;

        [ShowInInspector]
        public float Cooldown
        {
            get => (float)(expectedTime - TimerManager.currentTime).ClampMin(0);
            set
            {
                if (value <= 0)
                {
                    if (Cooldown > 0)
                    {
                        TimerManager.Stop(this);
                    }
                    
                    return;
                }

                if (TimerManager.Contains(this))
                {
                    TimerManager.Stop(this);
                }
                
                TimerManager.Add(this, value);
            }
        }

        public event Action<SpellTimerExample> OnCooldownEnd;

        #region Timer Implementation

        void ITimer.OnStart(double startedTime, double expectedTime)
        {
            this.expectedTime = expectedTime;
        }

        void ITimer.OnTimed()
        {
            OnCooldownEnd?.Invoke(this);
        }

        #endregion

        #region Priority Queue Node Implementation

        double IGenericPriorityQueueNode<double>.Priority { get; set; }

        int IGenericPriorityQueueNode<double>.QueueIndex { get; set; }

        long IGenericPriorityQueueNode<double>.InsertionIndex { get; set; }

        #endregion
    }
}