using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Localization.Settings;
using UnityEngine.Scripting;
using VMFramework.Core;
using VMFramework.Procedure;

namespace VMFramework.Localization
{
    [GameInitializerRegister(VMFrameworkInitializationDoneProcedure.ID, ProcedureLoadingType.OnEnter)]
    [Preserve]
    public sealed class LocalizationInitializer : IGameInitializer
    {
        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.Init, OnInit, this));
        }

        private static async UniTask OnInit(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (LocalizationSettings.Instance == null)
            {
                return;
            }
            
            await UniTask.WaitUntil(() => LocalizationSettings.InitializationOperation.IsDone,
                cancellationToken: cancellationToken);
            
            UnityEngine.Debug.Log("Localization initialization complete.");
        }
    }
}
