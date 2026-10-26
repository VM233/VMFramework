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

        private static async UniTask OnInit(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            GameEventManager.Instance.AdoptDependencies(CoreSetting.GameEventGeneralSetting.directDependencies);

            var slice = new InitializationWorkSlice();
            foreach (var gameEventConfig in GamePrefabManager.GetAllActiveGamePrefabs<IGameEventConfig>())
            {
                await slice.YieldIfBudgetExpired(cancellationToken);
                GameEventManager.Instance.Register(gameEventConfig.id);
            }
        }
    }
}
