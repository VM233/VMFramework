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

            foreach (var gamePrefab in gamePrefabs)
            {
                cancellationToken.ThrowIfCancellationRequested();
                GamePrefabManager.RegisterGamePrefab(gamePrefab);
            }
        }
        
        private static UniTask OnInitComplete(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var checkPipeline = new ActionProcessorPipeline<object, ICheckProcessor>();
            checkPipeline.AutoCollect();
            
            foreach (var gamePrefab in GamePrefabManager.GetAllGamePrefabs())
            {
                gamePrefab.CheckSettings();
                checkPipeline.ProcessTarget(gamePrefab);
            }

            return UniTask.CompletedTask;
        }
    }
}
