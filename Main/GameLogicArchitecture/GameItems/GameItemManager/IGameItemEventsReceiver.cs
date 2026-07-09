using System;

namespace VMFramework.GameLogicArchitecture
{
    public interface IGameItemEventsReceiver
    {
        public event Action<IGameItem> OnGameItemCreated;
        public event Action<IGameItem> OnGameItemDestroyed;
    }
}