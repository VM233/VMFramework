using System;
using System.Collections.Generic;
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

        private static void OnPreInit(Action onDone)
        {
            CoreSetting.GameTagGeneralSetting.CheckGameTags();
            CoreSetting.GameTagGeneralSetting.InitGameTags();
            
            onDone();
        }
    }
}