using System;
using System.Collections.Generic;
using UnityEngine.Scripting;
using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;

namespace VMFramework.GameEvents
{
    [GameInitializerRegister(GameInitializationDoneProcedure.ID, ProcedureLoadingType.OnEnter)]
    [Preserve]
    public sealed class GameEventInitializer : IGameInitializer
    {
        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.PostInit, OnInit, this));
        }

        private static void OnInit(Action onDone)
        {
            foreach (var gameEventConfig in GamePrefabManager.GetAllActiveGamePrefabs<IGameEventConfig>())
            {
                var gameEvent = GameItemManager.Instance.Get<IGameEvent>(gameEventConfig.id);
                GameEventManager.Instance.Register(gameEvent);
            }

            onDone();
        }
    }
}