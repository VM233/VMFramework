using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace VMFramework.Procedure
{
    /// <summary>
    /// Create the ManagerBehaviours in the very beginning of the game.
    /// </summary>
    [GameInitializerRegister(VMFrameworkInitializationDoneProcedure.ID, ProcedureLoadingType.OnEnter)]
    [Preserve]
    public sealed class ManagerBehaviourCreationInitializer : IGameInitializer
    {
        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.BeforeInitStart, OnBeforeInitStart, this));
        }

        private static void OnBeforeInitStart(Action onDone)
        {
            ManagerCreator.CreateManagers();
            
            onDone();
        }
    }
}
