using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
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

        private static UniTask OnBeforeInitStart(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ManagerCreator.CreateManagers();
            return UniTask.CompletedTask;
        }
    }
}
