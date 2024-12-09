using System;
using System.Collections.Generic;
using VMFramework.Core.Linq;
using VMFramework.Procedure;

namespace VMFramework.Examples
{
    [ManagerCreationProvider("Demo")]
    public sealed class CustomInitializationManagerBehaviour
        : ManagerBehaviour<CustomInitializationManagerBehaviour>
    {
        protected override void GetInitializationActions(ICollection<InitializationAction> actions)
        {
            base.GetInitializationActions(actions);
            
            actions.Add(new InitializationAction(InitializationOrder.InitComplete, OnInitComplete, this));
        }

        private void OnInitComplete(Action onDone)
        {
            // Write your initialization code here.

            onDone();
        }
    }
}