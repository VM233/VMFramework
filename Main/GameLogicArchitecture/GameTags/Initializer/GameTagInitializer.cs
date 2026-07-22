using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Scripting;
using VMFramework.Procedure;

namespace VMFramework.GameLogicArchitecture
{
    [GameInitializerRegister(VMFrameworkInitializationDoneProcedure.ID, ProcedureLoadingType.OnEnter)]
    [Preserve]
    public sealed class GameTagInitializer : IGameInitializer
    {
        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.PreInit, OnPreInit, this));
        }

        private static UniTask OnPreInit(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            CoreSetting.GameTagGeneralSetting.CheckGameTags();
            CoreSetting.GameTagGeneralSetting.InitGameTags();
            return UniTask.CompletedTask;
        }
    }
}
