#if FISHNET
using System;
using System.Collections.Generic;
using UnityEngine;
using VMFramework.Core.Linq;
using VMFramework.GameEvents;
using VMFramework.Network;
using VMFramework.Procedure;

namespace VMFramework.Containers
{
    public partial class ContainerManager
    {
        private static readonly Action<IContainer> onContainerOpenFunc = OnContainerOpen;
        private static readonly Action<IContainer> onContainerCloseFunc = OnContainerClose;
        
        protected override void GetInitializationActions(ICollection<InitializationAction> actions)
        {
            base.GetInitializationActions(actions);
            
            actions.Add(new(InitializationOrder.InitComplete, OnInitComplete, this));
        }

        private static void OnInitComplete(Action onDone)
        {
            ContainerCreateEvent.AddCallback(OnContainerCreate, GameEventPriority.SUPER);
            ContainerDestroyEvent.AddCallback(OnContainerDestroy, GameEventPriority.SUPER);
            
            onDone();
        }

        private static void OnContainerCreate(ContainerCreateEvent gameEvent)
        {
            if (gameEvent.container == null)
            {
                Debug.LogError("Container is null in ContainerCreateEvent");
                return;
            }
            
            gameEvent.container.OnOpenEvent += onContainerOpenFunc;
            gameEvent.container.OnCloseEvent += onContainerCloseFunc;
        }

        private static void OnContainerDestroy(ContainerDestroyEvent gameEvent)
        {
            if (gameEvent.container == null)
            {
                Debug.LogError("Container is null in ContainerDestroyEvent");
                return;
            }
            
            gameEvent.container.OnOpenEvent -= onContainerOpenFunc;
            gameEvent.container.OnCloseEvent -= onContainerCloseFunc;
        }
        
        private static void OnContainerOpen(IContainer container)
        {
            UUIDCoreManager.Observe(container);
        }

        private static void OnContainerClose(IContainer container)
        {
            UUIDCoreManager.Unobserve(container);
        }
    }
}
#endif