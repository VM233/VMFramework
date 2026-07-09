using UnityEngine.InputSystem;
using VMFramework.Core;

namespace VMFramework.GameEvents
{
    public class InputSystemGameEvent : ParameterizedGameEvent<InputAction.CallbackContext>
    {
        public InputSystemGameEventConfig InputSystemGameEventConfig => (InputSystemGameEventConfig)GamePrefab;
        
        public InputAction InputAction => InputSystemGameEventConfig.InputAction;

        protected override void OnGet()
        {
            base.OnGet();

            InputAction.started += OnAction;
            InputAction.performed += OnAction;
            InputAction.canceled += OnAction;
        }

        protected override void OnReturn()
        {
            base.OnReturn();

            InputAction.started -= OnAction;
            InputAction.performed -= OnAction;
            InputAction.canceled -= OnAction;
        }

        protected virtual void OnAction(InputAction.CallbackContext context)
        {
            if (IsEnabled.GetValue() == false)
            {
                return;
            }

            if (context.performed)
            {
                Propagate(context);
            }
            else
            {
                Propagate(context, propagateAction: false);
            }

            if (IsDebugging)
            {
                UnityEngine.Debug.LogWarning(context);
            }
        }
    }
}