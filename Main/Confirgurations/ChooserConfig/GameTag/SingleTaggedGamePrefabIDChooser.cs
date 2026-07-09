using System;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    [Serializable]
    public class SingleTaggedGamePrefabIDChooser : IChooser<string>
    {
        [GameTagID]
        [IsNotNullOrEmpty]
        public string gameTagID;

        public SingleTaggedGamePrefabIDChooser(string gameTagID)
        {
            this.gameTagID = gameTagID;
        }

        public string GetRandomItem(Random random)
        {
            return GamePrefabManager.GetRandomIDByGameTag(random, gameTagID);
        }

        public IChooser<string> GenerateNewChooser()
        {
            return new SingleTaggedGamePrefabIDChooser(gameTagID);
        }
    }
}