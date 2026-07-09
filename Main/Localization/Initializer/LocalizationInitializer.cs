using System;
using System.Collections.Generic;
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

        private static async void OnInit(Action onDone)
        {
            if (LocalizationSettings.Instance == null)
            {
                onDone();
                return;
            }
            
            await UniTask.WaitUntil(() => LocalizationSettings.InitializationOperation.IsDone);
            
            UnityEngine.Debug.Log("Localization initialization complete.");
            
            onDone();
        }
    }
}
