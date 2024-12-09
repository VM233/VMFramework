// using System;
// using System.Collections.Generic;
// using UnityEngine.Scripting;
// using VMFramework.Procedure;
//
// namespace VMFramework.GameLogicArchitecture
// {
//     [GameInitializerRegister(VMFrameworkInitializationDoneProcedure.ID, ProcedureLoadingType.OnEnter)]
//     [Preserve]
//     public sealed class GameTypeInitializer : IGameInitializer
//     {
//         void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
//         {
//             actions.Add(new(InitializationOrder.PreInit, OnPreInit, this));
//         }
//
//         private static void OnPreInit(Action onDone)
//         {
//             CoreSetting.GameTypeGeneralSetting.CheckGameTypeInfo();
//             CoreSetting.GameTypeGeneralSetting.InitGameTypeInfo();
//             
//             onDone();
//         }
//     }
// }