using System;

namespace VMFramework.GameEvents
{
    public interface IReadOnlyParameterizedGameEvent<out TArgument> : IReadOnlyParameterlessGameEvent
    {
        public void AddCallback(Action<TArgument> callback, int priority);
        
        public void RemoveCallback(Action<TArgument> callback);

        void IReadOnlyGameEvent.AddCallback(Delegate callback, int priority)
        {
            AddCallback((Action<TArgument>)callback, priority);
        }

        void IReadOnlyGameEvent.RemoveCallback(Delegate callback)
        {
            RemoveCallback((Action<TArgument>)callback);
        }
    }
}