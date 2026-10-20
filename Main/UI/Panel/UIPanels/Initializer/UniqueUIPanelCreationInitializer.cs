using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Scripting;
using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;

namespace VMFramework.UI 
{
    [GameInitializerRegister(GameInitializationDoneProcedure.ID, ProcedureLoadingType.OnEnter)]
    [Preserve]
    public sealed class UniqueUIPanelCreationInitializer : IGameInitializer
    {
        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.InitComplete, OnInitComplete, this));
        }

        private static async UniTask OnInitComplete(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var slice = new InitializationWorkSlice();

            foreach (var config in GamePrefabManager.GetAllActiveGamePrefabs<IUIPanelConfig>())
            {
                if (config.IsUnique)
                {
                    await slice.YieldIfBudgetExpired(cancellationToken);
                    UIPanelManager.Instance.CreatePanel(config.id);
                }
            }
        }
    }
}
