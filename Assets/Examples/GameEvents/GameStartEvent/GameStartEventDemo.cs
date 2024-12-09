using System;
using System.Collections.Generic;
using UnityEngine;
using VMFramework.Core.Linq;
using VMFramework.GameEvents;
using VMFramework.Procedure;

namespace VMFramework.Examples
{
    [ManagerCreationProvider("Demo")]
    public sealed class GameStartEventDemo : ManagerBehaviour<GameStartEventDemo>
    {
        protected override void GetInitializationActions(ICollection<InitializationAction> actions)
        {
            base.GetInitializationActions(actions);
            
            actions.Add(new InitializationAction(InitializationOrder.InitComplete, OnInitComplete, this));
        }

        private void OnInitComplete(Action onDone)
        {
            // Add a callback to the GameStartEvent
            GameEventManager.AddCallback(GameStartEventConfig.ID, (GameStartEventArguments arguments) =>
            {
                Debug.LogWarning($"Game Started with {arguments.playerCount} players");
            }, GameEventPriority.SUPER);
            
            // Propagate the GameStartEvent
            GameEventManager.Propagate(GameStartEventConfig.ID, new GameStartEventArguments(2));
            
            onDone();
        }
    }
}