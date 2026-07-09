// #if UNITY_EDITOR
// namespace VMFramework.GameLogicArchitecture
// {
//     public partial class GameTypedGamePrefab
//     {
//         protected override void OnInspectorInit()
//         {
//             base.OnInspectorInit();
//             
//             initialGameTypesID ??= new();
//         }
//         
//         private void OnInitialGameTypesIDChangedGUI()
//         {
//             if (gameTypeSet == null)
//             {
//                 return;
//             }
//             
//             gameTypeSet.ClearGameTypes();
//
//             foreach (var gameTypeID in InitialGameTypesID)
//             {
//                 gameTypeSet.AddGameType(gameTypeID);
//             }
//         }
//     }
// }
// #endif