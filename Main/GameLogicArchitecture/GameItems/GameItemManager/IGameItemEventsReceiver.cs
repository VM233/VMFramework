using System;

namespace VMFramework.GameLogicArchitecture
{
    [UnityEngine.Scripting.RequireImplementors]
    public interface IGameItemEventsReceiver
    {
        public event Action<IGameItem> OnGameItemCreated;
        public event Action<IGameItem> OnGameItemDestroyed;
    }
}
