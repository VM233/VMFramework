using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
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

        private static UniTask OnInit(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            foreach (var gameEventConfig in GamePrefabManager.GetAllActiveGamePrefabs<IGameEventConfig>())
            {
                var gameEvent = GameItemManager.Instance.Get<IGameEvent>(gameEventConfig.id);
                GameEventManager.Instance.Register(gameEvent);
            }

            return UniTask.CompletedTask;
        }
    }
}
