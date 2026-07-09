using VMFramework.GameEvents;

namespace VMFramework.Core.Pools
{
    public interface IPriorityPoolEventProvider
    {
        public IReadOnlyPriorityEvents<IPoolEventProvider.GetHandler> GetEvents { get; }
        
        public IReadOnlyPriorityEvents<IPoolEventProvider.ReturnHandler> ReturnEvents { get; }
    }
}