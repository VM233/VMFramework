using System.Collections.Generic;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.GameEvents
{
    public class GameEventDependencyNode : BaseConfig
    {
        [GamePrefabID(typeof(IGameEventConfig))]
        public string gameEventID;
        
        public List<GameEventDependencyNode> children = new();

        public override void CheckSettings()
        {
            base.CheckSettings();
            
            gameEventID.AssertIsNotNullOrWhiteSpace(nameof(gameEventID));
            
            children?.CheckSettings();
        }

        protected override void OnInit()
        {
            base.OnInit();
            
            children?.Init();
        }
    }
}