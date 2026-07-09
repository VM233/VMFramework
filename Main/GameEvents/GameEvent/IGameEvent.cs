using VMFramework.Core;

namespace VMFramework.GameEvents
{
    public interface IGameEvent : IReadOnlyGameEvent, IValidCheckDispatcher
    {
        
    }
}