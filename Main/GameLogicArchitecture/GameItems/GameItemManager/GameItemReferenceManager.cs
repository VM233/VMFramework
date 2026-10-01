using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.Procedure;

namespace VMFramework.GameLogicArchitecture
{
    [ManagerCreationProvider(ManagerType.GameItemCore)]
    public class GameItemReferenceManager : ManagerBehaviour<GameItemReferenceManager>
    {
        public delegate void InitializeHandler(IGameItem gameItem);

        public event InitializeHandler OnInitialize;

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        protected readonly Dictionary<string, IGameItem> references = new();

        protected override void Awake()
        {
            base.Awake();

            references.Clear();
        }

        public virtual IGameItem Get(string id)
        {
            if (references.TryGetValue(id, out var reference))
            {
                return reference;
            }

            if (GamePrefabManager.ContainsGamePrefab(id) == false)
            {
                return null;
            }

            var gameItem = GameItemManager.Instance.Get(id);
            try
            {
                if (gameItem.TryAsGameObject(out var gameItemObject))
                {
                    gameItemObject.transform.SetParent(transform);
                }

                OnInitialize?.Invoke(gameItem);
                if (gameItemObject != null)
                {
                    gameItemObject.SetActive(false);
                }
                references.Add(id, gameItem);
                return gameItem;
            }
            catch
            {
                GameItemManager.Instance.Return(gameItem);
                throw;
            }
        }
    }
}
