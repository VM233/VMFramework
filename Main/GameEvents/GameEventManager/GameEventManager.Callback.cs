using System;
using System.Runtime.CompilerServices;

namespace VMFramework.GameEvents
{
    public partial class GameEventManager
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddCallback(string id, Delegate callback, int priority)
        {
            var gameEvent = GetGameEventStrictly(id);

            gameEvent.AddCallback(callback, priority);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddCallback(string id, Action callback, int priority)
        {
            var gameEvent = GetGameEventStrictly<IReadOnlyParameterlessGameEvent>(id);

            gameEvent.AddCallback(callback, priority);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddCallback<TArgument>(string id, Action<TArgument> callback, int priority)
        {
            var gameEvent = GetGameEventStrictly<IReadOnlyParameterizedGameEvent<TArgument>>(id);

            gameEvent.AddCallback(callback, priority);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveCallback(string id, Delegate callback)
        {
            var gameEvent = GetGameEventStrictly(id);
        
            gameEvent.RemoveCallback(callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveCallback(string id, Action callback)
        {
            var gameEvent = GetGameEventStrictly<IReadOnlyParameterlessGameEvent>(id);

            gameEvent.RemoveCallback(callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveCallback<TArgument>(string id, Action<TArgument> callback)
        {
            var gameEvent = GetGameEventStrictly<IReadOnlyParameterizedGameEvent<TArgument>>(id);

            gameEvent.RemoveCallback(callback);
        }
    }
}