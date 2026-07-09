using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Linq;

namespace VMFramework.GameLogicArchitecture
{
    [CreateAssetMenu(fileName = "New GamePrefabMultipleWrapper", 
        menuName = FrameworkMeta.NAME + "/GamePrefabMultipleWrapper")]
    public sealed partial class GamePrefabMultipleWrapper : GamePrefabWrapper, INameOwner
    {
        [SerializeField]
#if UNITY_EDITOR
        [OnCollectionChanged(nameof(OnGamePrefabsChanged))]
        [ListDrawerSettings(HideAddButton = true)]
#endif
        private List<IGamePrefab> gamePrefabs = new();

        public override string id
        {
            get
            {
                if (gamePrefabs.IsNullOrEmpty())
                {
                    return null;
                }

                return gamePrefabs.FirstNotNull()?.id;
            }
        }

        public override void GetGamePrefabs(ICollection<IGamePrefab> gamePrefabsCollection)
        {
            if (gamePrefabs.IsNullOrEmpty())
            {
                return;
            }
            
            gamePrefabsCollection.AddRange(gamePrefabs);
            
            foreach (var gamePrefab in gamePrefabs)
            {
                if (gamePrefab is IGamePrefabsProvider provider)
                {
                    provider.GetGamePrefabs(gamePrefabsCollection);
                }
            }
        }

        public override void InitGamePrefabs(IReadOnlyCollection<IGamePrefab> gamePrefabs)
        {
            this.gamePrefabs ??= new();
            this.gamePrefabs.RemoveAllNull();
            this.gamePrefabs.AddRange(gamePrefabs.WhereNotNull());
        }

        string INameOwner.Name
        {
            get
            {
                if (gamePrefabs.IsNullOrEmpty() || gamePrefabs[0] == null)
                {
                    if (this != null)
                    {
                        return name;
                    }

                    return $"Null {nameof(GamePrefabMultipleWrapper)}";
                }

                return gamePrefabs[0].Name;
            }
        }
    }
}