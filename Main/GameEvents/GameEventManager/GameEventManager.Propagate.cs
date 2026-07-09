using System.Runtime.CompilerServices;

namespace VMFramework.GameEvents
{
    public partial class GameEventManager
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Propagate(string id)
        {
            var gameEvent = GetGameEventStrictly<IParameterlessGameEvent>(id);
            
            gameEvent.Propagate();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Propagate<TArgument>(string id, TArgument argument)
        {
            var gameEvent = GetGameEventStrictly<IParameterizedGameEvent<TArgument>>(id);
            
            gameEvent.Propagate(argument);
        }
    }
}