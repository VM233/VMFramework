// using System.Collections.Generic;
// using Sirenix.OdinInspector;
// using UnityEngine;
//
// namespace VMFramework.GameEvents
// {
//     public partial class InputActionGroup
//     {
// #if UNITY_EDITOR
//         [ListDrawerSettings(ShowFoldout = false, DefaultExpandedState = true,
//             CustomAddFunction = nameof(AddInputActionToListGUI))]
// #endif
//         public List<InputAction> actions = new();
//
//         public InputActionGroup()
//         {
//             actions.Add(new());
//         }
//
//         public InputActionGroup(KeyCode keyCode,
//             KeyBoardTriggerType keyBoardTriggerType)
//         {
//             InputAction action;
//             action.type = InputType.KeyBoardOrMouseOrJoyStick;
//             action.keyCode = keyCode;
//             action.keyBoardTriggerType = keyBoardTriggerType;
//             action.holdThreshold = 0;
//
//             actions.Add(action);
//         }
//     }
// }
