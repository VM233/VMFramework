using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

namespace VMFramework.Procedure
{
    [GameInitializerRegister(GameInitializationDoneProcedure.ID, ProcedureLoadingType.OnEnter)]
    [Preserve]
    public sealed class ManagerBehaviourInitializer : IGameInitializer
    {
        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            var managerBehaviours = ManagerBehaviourCollector.Collect().ToList();

            foreach (var managerBehaviour in managerBehaviours)
            {
                managerBehaviour.GetInitializationActions(actions);
            }
        }
    }
}