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
    public sealed class GamePrefabRegisterInitializer : IGameInitializer, IInitializationProgressSource
    {
        public InitializationProgress Progress { get; private set; }
        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.PostInit, OnPostInit, this));
            actions.Add(new(InitializationOrder.InitComplete, OnInitComplete, this, progressSource: this));
        }

        private async UniTask OnPostInit(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            GamePrefabManager.Clear();

            var gamePrefabs = await GamePrefabCollectorManager.Collect(cancellationToken);

            var slice = new InitializationWorkSlice();

            foreach (var gamePrefab in gamePrefabs)
            {
                await slice.YieldIfBudgetExpired(cancellationToken);
                GamePrefabManager.RegisterGamePrefab(gamePrefab);
            }

            GamePrefabReferenceValidator.Validate(
                GamePrefabManager.GetAllGamePrefabs());
        }
        
        private async UniTask OnInitComplete(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Progress = new InitializationProgress(0, GamePrefabManager.GetGamePrefabsCount());
            var checkPipeline = new ActionProcessorPipeline<object, ICheckProcessor>();
            await checkPipeline.AutoCollectAsync(cancellationToken);
            var slice = new InitializationWorkSlice();
            
            foreach (var gamePrefab in GamePrefabManager.GetAllGamePrefabs())
            {
                await slice.YieldIfBudgetExpired(cancellationToken);
                await ConfigurationValidation.CheckAsync(gamePrefab, cancellationToken);
                checkPipeline.ProcessTarget(gamePrefab);
                Progress = new InitializationProgress(Progress.Completed + 1, Progress.Total);
            }
        }
    }
}
