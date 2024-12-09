// using System.Collections.Generic;
// using System.Linq;
// using Newtonsoft.Json;
// using Sirenix.OdinInspector;
// using UnityEngine;
// using VMFramework.Core;
//
// namespace VMFramework.GameLogicArchitecture
// {
//     public sealed partial class GameTypeGeneralSetting : GeneralSetting
//     {
//         #region Categories
//
//         private const string GAME_TYPE_CATEGORY = "Game Type";
//
//         #endregion
//
//         #region Configs
//
// #if UNITY_EDITOR
//         [LabelText("Game Types"), TabGroup(TAB_GROUP_NAME, GAME_TYPE_CATEGORY)]
//         [OnValueChanged(nameof(OnSubrootGameTypeInfosChanged), true)]
//         [OnCollectionChanged(nameof(OnSubrootGameTypeInfosChanged))]
// #endif
//         [SerializeField, JsonProperty]
//         private List<GameTypeInfo> subrootGameTypeInfos = new();
//
//         #endregion
//
//         #region Init & Check
//
//         public void CheckGameTypeInfo()
//         {
//             if (subrootGameTypeInfos.Any(gameTypeInfo => gameTypeInfo == null))
//             {
//                 Debug.LogError($"There is a null {nameof(GameTypeInfo)} in {nameof(subrootGameTypeInfos)}.");
//             }
//             
//             foreach (var gameTypeInfo in subrootGameTypeInfos.PreorderTraverse(true))
//             {
//                 if (gameTypeInfo.id == null)
//                 {
//                     Debugger.LogWarning(
//                         $"Existing initial {nameof(gameTypeInfo)} has an empty {nameof(gameTypeInfo.id)}.");
//                     continue;
//                 }
//
//                 if (gameTypeInfo.id.IsWhiteSpace())
//                 {
//                     Debugger.LogWarning(
//                         $"Existing initial {nameof(gameTypeInfo)} has an empty {nameof(gameTypeInfo.id)} after trimming.");
//                     continue;
//                 }
//             }
//         }
//
//         public void InitGameTypeInfo()
//         {
//             GameType.Clear();
//
//             subrootGameTypeInfos ??= new();
//
//             foreach (var subrootGameTypeInfo in subrootGameTypeInfos)
//             {
//                 if (subrootGameTypeInfo == null)
//                 {
//                     continue;
//                 }
//
//                 if (subrootGameTypeInfo.id.IsNullOrWhiteSpace())
//                 {
//                     continue;
//                 }
//                 
//                 GameType.CreateSubroot(subrootGameTypeInfo.id, ((ILocalizedNameOwner)subrootGameTypeInfo).NameReference);
//             }
//
//             HashSet<GameTypeInfo> validGameTypeInfos = new();
//
//             foreach (var gameTypeInfo in subrootGameTypeInfos.PreorderTraverse(true))
//             {
//                 if (gameTypeInfo == null)
//                 {
//                     continue;
//                 }
//
//                 if (gameTypeInfo.id.IsNullOrWhiteSpace())
//                 {
//                     continue;
//                 }
//                 
//                 gameTypeInfo.subtypes.Examine(subtype => subtype.parentID = gameTypeInfo.id);
//                 
//                 validGameTypeInfos.Add(gameTypeInfo);
//             }
//
//             foreach (var gameTypeInfo in validGameTypeInfos)
//             {
//                 if (gameTypeInfo.parentID.IsNullOrWhiteSpace())
//                 {
//                     continue;
//                 }
//                 
//                 GameType.Create(gameTypeInfo.id, ((ILocalizedNameOwner)gameTypeInfo).NameReference,
//                     gameTypeInfo.parentID);
//             }
//         }
//
//         #endregion
//     }
// }
