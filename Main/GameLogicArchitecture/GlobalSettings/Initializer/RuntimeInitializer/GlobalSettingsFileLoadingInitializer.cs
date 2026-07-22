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
    public sealed class GlobalSettingsFileLoadingInitializer : IGameInitializer
    {
        private static readonly List<IGlobalSetting> globalSettings = new();

        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.InitStart, OnInitStart, this));
            actions.Add(new(InitializationOrder.Init, OnInit, this));
        }

        private static async UniTask OnInitStart(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var tasks = new List<UniTask>();
            
            globalSettings.Clear();
            
            foreach (var globalSetting in GlobalSettingCollector.Collect())
            {
                tasks.Add(globalSetting.LoadGlobalSettingFile());
                globalSettings.Add(globalSetting);
            }
            
            await UniTask.WhenAll(tasks).AttachExternalCancellation(cancellationToken);
        }

        private static UniTask OnInit(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            globalSettings.CheckSettings();
            return UniTask.CompletedTask;
        }
    }
}
