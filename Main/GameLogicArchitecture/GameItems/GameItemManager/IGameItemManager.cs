using System;

namespace VMFramework.GameLogicArchitecture
{
    public interface IGameItemManager
    {
        public event Action<IGameItem> OnGameItemCreated;
        public event Action<IGameItem> OnGameItemDestroyed;

        public IGameItem Get(string id);
        
        public TGameItem Get<TGameItem>(string id)
            where TGameItem : IGameItem;

        public void Return(IGameItem gameItem);

        public void PrewarmUntil(string id, int count);
    }
}