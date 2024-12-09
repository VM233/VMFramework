using System;
using System.Collections.Generic;
using UnityEngine.Scripting;
using VMFramework.Procedure;

namespace VMFramework.Properties
{
    [GameInitializerRegister(VMFrameworkInitializationDoneProcedure.ID, ProcedureLoadingType.OnEnter)]
    [Preserve]
    public sealed class GamePropertyInitializer : IGameInitializer
    {
        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.InitComplete, OnInitComplete, this));
        }

        private static void OnInitComplete(Action onDone)
        {
            GamePropertyManager.Init();
            
            onDone();
        }
    }
}