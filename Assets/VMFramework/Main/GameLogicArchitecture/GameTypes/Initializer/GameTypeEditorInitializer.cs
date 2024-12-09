// #if UNITY_EDITOR
// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using VMFramework.Procedure;
// using VMFramework.Procedure.Editor;
//
// namespace VMFramework.GameLogicArchitecture.Editor
// {
//     internal sealed class GameTypeEditorInitializer : IEditorInitializer
//     {
//         void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
//         {
//             actions.Add(new(InitializationOrder.PreInit, OnPreInit, this));
//         }
//
//         private static void OnPreInit(Action onDone)
//         {
//             if (CoreSetting.GameTypeGeneralSetting == null)
//             {
//                 Debug.LogError($"{nameof(GameTypeGeneralSetting)} is not set. Please set it in the ${nameof(CoreSetting)}.");
//                 
//                 onDone();
//                 return;
//             }
//             
//             CoreSetting.GameTypeGeneralSetting.InitGameTypeInfo();
//             
//             onDone();
//         }
//     }
// }
// #endif