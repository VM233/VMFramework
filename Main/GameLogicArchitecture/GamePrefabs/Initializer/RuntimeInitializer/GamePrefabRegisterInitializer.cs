using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Scripting;
using VMFramework.Configuration;
using VMFramework.Procedure;

namespace VMFramework.GameLogicArchitecture
{
    [GameInitializerRegister(VMFrameworkInitializationDoneProcedure.ID, ProcedureLoadingType.OnEnter)]
    [Preserve]
    public sealed class GamePrefabRegisterInitializer : IGameInitializer
    {
        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.PostInit, OnPostInit, this));
            actions.Add(new(InitializationOrder.InitComplete, OnInitComplete, this));
        }

        private static async UniTask OnPostInit(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            GamePrefabManager.Clear();

            var gamePrefabs = await GamePrefabCollectorManager.Collect()
                .AttachExternalCancellation(cancellationToken);

            var slice = new InitializationWorkSlice();

            foreach (var gamePrefab in gamePrefabs)
            {
                cancellationToken.ThrowIfCancellationRequested();
                GamePrefabManager.RegisterGamePrefab(gamePrefab);
                await slice.YieldIfBudgetExpired(cancellationToken);
            }

            GamePrefabReferenceValidator.Validate(
                GamePrefabManager.GetAllGamePrefabs());
        }
        
        private static async UniTask OnInitComplete(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var checkPipeline = new ActionProcessorPipeline<object, ICheckProcessor>();
            checkPipeline.AutoCollect();
            var slice = new InitializationWorkSlice();
            
            foreach (var gamePrefab in GamePrefabManager.GetAllGamePrefabs())
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (gamePrefab is IAsyncCheckableConfig asynchronous)
                    await asynchronous.CheckSettingsAsync(cancellationToken);
                else gamePrefab.CheckSettings();
                checkPipeline.ProcessTarget(gamePrefab);
                await slice.YieldIfBudgetExpired(cancellationToken);
            }
        }
    }
}
