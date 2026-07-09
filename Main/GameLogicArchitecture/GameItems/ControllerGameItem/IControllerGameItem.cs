using VMFramework.Core;
using VMFramework.Core.Pools;

namespace VMFramework.GameLogicArchitecture
{
    public interface IControllerGameItem : IGameItem, IController, IPoolEventProvider, IPriorityPoolEventProvider
    {
        
    }
}