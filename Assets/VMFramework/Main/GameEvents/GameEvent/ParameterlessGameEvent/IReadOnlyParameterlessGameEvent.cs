using System;

namespace VMFramework.GameEvents
{
    public interface IReadOnlyParameterlessGameEvent : IReadOnlyGameEvent
    {
        public void AddCallback(Action callback, int priority);
        
        public void RemoveCallback(Action callback);

        void IReadOnlyGameEvent.AddCallback(Delegate callback, int priority)
        {
            AddCallback((Action)callback, priority);
        }

        void IReadOnlyGameEvent.RemoveCallback(Delegate callback)
        {
            RemoveCallback((Action)callback);
        }
    }
}