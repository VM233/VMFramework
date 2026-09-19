using System;
using System.Collections.Generic;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.GameEvents
{
    [Serializable]
    public class GameEventDependencyNode : ICheckableConfig
    {
        [GamePrefabID(typeof(IGameEventConfig))]
        public string gameEventID;

        [UnityEngine.SerializeReference]
        public List<GameEventDependencyNode> children = new();

        public void CheckSettings()
        {
            gameEventID.AssertIsNotNullOrWhiteSpace(nameof(gameEventID));

            children?.CheckSettings();
        }
    }
}
