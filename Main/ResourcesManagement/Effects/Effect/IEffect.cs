using VMFramework.GameLogicArchitecture;

namespace VMFramework.Effects
{
    public interface IEffect : IControllerGameItem, IGameItemDelayedReturnReceiver, IGameItemDelayedReturnDispatcher
    {
        
    }
}