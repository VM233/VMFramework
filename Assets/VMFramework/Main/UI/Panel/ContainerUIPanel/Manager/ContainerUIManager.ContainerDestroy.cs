using System;
using System.Collections.Generic;
using VMFramework.Containers;
using VMFramework.Core.Linq;
using VMFramework.Procedure;

namespace VMFramework.UI
{
    public partial class ContainerUIManager : IInitializer
    {
        protected override void GetInitializationActions(ICollection<InitializationAction> actions)
        {
            base.GetInitializationActions(actions);
            
            actions.Add(new(InitializationOrder.InitComplete, OnInitComplete, this));
        }

        private static void OnInitComplete(Action onDone)
        {
            ContainerDestroyEvent.AddCallback(OnContainerDestroy);
            
            onDone();
        }

        private static void OnContainerDestroy(ContainerDestroyEvent gameEvent)
        {
            foreach (var panel in openedPanels)
            {
                foreach (var container in panel.BindContainers)
                {
                    if (gameEvent.container == container)
                    {
                        panel.Close();
                        
                        break;
                    }
                }
            }
        }
    }
}