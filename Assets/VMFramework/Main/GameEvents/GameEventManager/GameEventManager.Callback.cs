using System;
using System.Runtime.CompilerServices;

namespace VMFramework.GameEvents
{
    public partial class GameEventManager
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddCallback(string id, Delegate callback, int priority)
        {
            var gameEvent = GetGameEventStrictly(id);

            gameEvent.AddCallback(callback, priority);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddCallback(string id, Action callback, int priority)
        {
            var gameEvent = GetGameEventStrictly<IParameterlessGameEvent>(id);

            gameEvent.AddCallback(callback, priority);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddCallback<TArgument>(string id, Action<TArgument> callback, int priority)
        {
            var gameEvent = GetGameEventStrictly<IParameterizedGameEvent<TArgument>>(id);

            gameEvent.AddCallback(callback, priority);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveCallback(string id, Delegate callback)
        {
            var gameEvent = GetGameEventStrictly(id);
        
            gameEvent.RemoveCallback(callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveCallback(string id, Action callback)
        {
            var gameEvent = GetGameEventStrictly<IParameterlessGameEvent>(id);

            gameEvent.RemoveCallback(callback);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveCallback<TArgument>(string id, Action<TArgument> callback)
        {
            var gameEvent = GetGameEventStrictly<IParameterizedGameEvent<TArgument>>(id);

            gameEvent.RemoveCallback(callback);
        }
    }
}