using System;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.GameEvents
{
    public class InputSystemGameEventConfig : GameEventConfig
    {
        public override Type GameItemType => typeof(InputSystemGameEvent);
        
        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [InputActionID]
        public Guid inputActionID;
        
        public InputAction InputAction { get; private set; }

        protected override void OnInit()
        {
            base.OnInit();
            
            InputAction = InputSystem.actions.FindAction(inputActionID);

            if (InputAction == null)
            {
                UnityEngine.Debug.LogError($"Input action with ID {inputActionID} not found.");
            }
        }
    }
}