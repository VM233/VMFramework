using System;
using System.Collections.Generic;
using UnityEngine;
using VMFramework.Core.Linq;
using VMFramework.GameEvents;
using VMFramework.Procedure;

namespace VMFramework.Examples
{
    [ManagerCreationProvider("Demo")]
    public sealed class ColliderMouseEventDemo : ManagerBehaviour<ColliderMouseEventDemo>
    {
        protected override void GetInitializationActions(ICollection<InitializationAction> actions)
        {
            base.GetInitializationActions(actions);
            
            actions.Add(new InitializationAction(InitializationOrder.InitComplete, OnInitComplete, this));
        }

        private void OnInitComplete(Action onDone)
        {
            ColliderMouseEventManager.AddCallback(MouseEventType.PointerEnter, OnPointerEnter);
            ColliderMouseEventManager.AddCallback(MouseEventType.PointerExit, OnPointerLeave);
            
            onDone();
        }

        private void OnPointerEnter(ColliderMouseEventTrigger trigger)
        {
            Debug.Log("Pointer Entered: " + trigger.name);
        }
        
        private void OnPointerLeave(ColliderMouseEventTrigger trigger)
        {
            Debug.Log("Pointer Left: " + trigger.name);
        }
    }
}