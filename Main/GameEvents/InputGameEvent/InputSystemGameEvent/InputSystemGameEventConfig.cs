using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.GameEvents
{
    [Serializable]
    public class InputSystemGameEventConfig : GameEventConfig
    {
        public override Type GameItemType => typeof(InputSystemGameEvent);
        
        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [InputActionID]
        public Guid inputActionID;

        [SerializeField]
        [HideInInspector]
        private string nativeInputActionID;
        
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

        protected override void OnPrepareNativeSerializationMigration()
        {
            base.OnPrepareNativeSerializationMigration();
            nativeInputActionID = inputActionID.ToString("D");
        }
    }
}
