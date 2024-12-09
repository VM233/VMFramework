// using System.Collections.Generic;
// using System.Linq;
// using Newtonsoft.Json;
// using Sirenix.OdinInspector;
// using UnityEngine;
// using VMFramework.Core;
// using VMFramework.OdinExtensions;
//
// namespace VMFramework.GameLogicArchitecture
// {
//     public abstract partial class GameTypedGamePrefab : GamePrefab, IGameTypedGamePrefab
//     {
//         #region Configs
//
// #if UNITY_EDITOR
//         [LabelText("Game Types"),
//          TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY, SdfIconType.Info, TextColor = "blue")]
//         [OnCollectionChanged(nameof(OnInitialGameTypesIDChangedGUI))]
//         [PropertyOrder(-6000)]
//         [GameTypeID]
//         [ListDrawerSettings(ShowFoldout = false)]
// #endif
//         [SerializeField, JsonProperty(Order = -6000)]
//         private List<string> initialGameTypesID = new();
//
//         #endregion
//
//         #region Config Properties
//
//         public IList<string> InitialGameTypesID
//         {
//             get => initialGameTypesID;
//             init => initialGameTypesID = value.ToList();
//         }
//
//         #endregion
//
//         #region Game Type
//
//         [TabGroup(TAB_GROUP_NAME, RUNTIME_DATA_CATEGORY, SdfIconType.Bug, TextColor = "orange")]
//         [ShowInInspector]
//         private GameTypeSet gameTypeSet;
//
//         public IGameTypeSet GameTypeSet
//         {
//             get
//             {
//                 if (gameTypeSet != null)
//                 {
//                     return gameTypeSet;
//                 }
//                 
//                 gameTypeSet = new(this);
//
//                 gameTypeSet.OnAddLeafGameType += OnAddLeafGameType;
//                 gameTypeSet.OnRemoveLeafGameType += OnRemoveLeafGameType;
//
//                 gameTypeSet.AddGameTypes(InitialGameTypesID);
//                 
//                 return gameTypeSet;
//             }
//         }
//
//         [TabGroup(TAB_GROUP_NAME, RUNTIME_DATA_CATEGORY)]
//         [ShowInInspector]
//         [HideInEditorMode]
//         [JsonIgnore]
//         public GameType UniqueGameType { get; private set; }
//
//         private void OnAddLeafGameType(IReadOnlyGameTypeSet gameTypeSet, GameType gameType)
//         {
//             UniqueGameType ??= gameType;
//         }
//
//         private void OnRemoveLeafGameType(IReadOnlyGameTypeSet gameTypeSet, GameType gameType)
//         {
//             if (UniqueGameType == gameType)
//             {
//                 UniqueGameType = null;
//
//                 if (this.gameTypeSet.HasGameType())
//                 {
//                     UniqueGameType = this.gameTypeSet.LeafGameTypes.Choose();
//                 }
//             }
//         }
//
//         #endregion
//         
//         #region Init & Check
//
//         public override void CheckSettings()
//         {
//             base.CheckSettings();
//
//             if (InitialGameTypesID == null)
//             {
//                 return;
//             }
//
//             foreach (var gameTypeID in InitialGameTypesID)
//             {
//                 if (GameType.HasGameType(gameTypeID) == false)
//                 {
//                     Debugger.LogWarning($"{this} : Game Type ID {gameTypeID} of does not exist.");
//                 }
//             }
//         }
//
//         #endregion
//     }
// }