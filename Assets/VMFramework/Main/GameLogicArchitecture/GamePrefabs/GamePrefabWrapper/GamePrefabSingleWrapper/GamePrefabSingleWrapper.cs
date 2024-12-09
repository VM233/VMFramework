using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Linq;

namespace VMFramework.GameLogicArchitecture
{
    [CreateAssetMenu(fileName = "New GamePrefabSingleWrapper", menuName = "VMFramework/GamePrefabSingleWrapper")]
    public sealed partial class GamePrefabSingleWrapper : GamePrefabWrapper, INameOwner
    {
        [HideLabel]
        [SerializeField]
        private IGamePrefab gamePrefab;

        public override string id => gamePrefab?.id;

        public override void GetGamePrefabs(ICollection<IGamePrefab> gamePrefabsCollection)
        {
            gamePrefabsCollection.Add(gamePrefab);

            if (gamePrefab is IGamePrefabProvider provider)
            {
                provider.GetGamePrefabs(gamePrefabsCollection);
            }
        }

        public override void InitGamePrefabs(IReadOnlyCollection<IGamePrefab> gamePrefabs)
        {
            if (gamePrefab != null)
            {
                Debugger.LogWarning($"{nameof(GamePrefabSingleWrapper)}({name}) already has a gamePrefab." +
                                    $"But it will be replaced with the new gamePrefab.");
            }

            var count = gamePrefabs.Count;
            
            if (count > 1)
            {
                Debugger.LogError($"GamePrefabSingleWrapper can only hold one gamePrefab, but {count} were provided.");
            }

            gamePrefab = gamePrefabs.FirstNotNull();
        }

        #region Interface Implementation

        string INameOwner.Name
        {
            get
            {
                if (gamePrefab == null)
                {
                    if (this != null)
                    {
                        return name;
                    }

                    return $"Null {nameof(GamePrefabSingleWrapper)}";
                }

                return gamePrefab.Name;
            }
        }

        #endregion
    }
}