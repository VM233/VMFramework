using System.Collections.Generic;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture
{
    public abstract partial class GamePrefabWrapper
        : ScriptableObject, IGamePrefabsProvider, INameOwner, IIDOwner<string>
    {
        public abstract void GetGamePrefabs(ICollection<IGamePrefab> gamePrefabsCollection);

        public abstract void InitGamePrefabs(IReadOnlyCollection<IGamePrefab> gamePrefabs);

        string INameOwner.Name => this == null ? "Null" : name;

        public abstract string id { get; }
    }
}
