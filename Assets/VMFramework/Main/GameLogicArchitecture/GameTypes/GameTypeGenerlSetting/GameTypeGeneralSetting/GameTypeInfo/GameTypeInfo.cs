// using System.Collections.Generic;
// using Newtonsoft.Json;
// using Sirenix.OdinInspector;
// using VMFramework.Configuration;
// using VMFramework.Core;
// using VMFramework.Localization;
// using VMFramework.OdinExtensions;
//
// namespace VMFramework.GameLogicArchitecture
// {
//     public partial class GameTypeGeneralSetting
//     {
//         private class GameTypeInfo : BaseConfig, IChildrenProvider<GameTypeInfo>, IIDOwner<string>, INameOwner,
//             ILocalizedNameOwner
//         {
//             [LabelText("ID")]
//             [IsNotNullOrEmpty, IsGameTypeID]
//             [JsonProperty]
//             public string id;
//
//             [LabelText("Sub Game Types")]
//             [JsonProperty]
//             public List<GameTypeInfo> subtypes = new();
//
//             [HideInEditorMode]
//             public string parentID;
//
//             #region Interface Implementation
//
//             string IIDOwner<string>.id => id;
//
//             string INameOwner.Name => id.ToPascalCase(" ");
//
//             public IEnumerable<GameTypeInfo> GetChildren() => subtypes;
//
//             IReadOnlyLocalizedStringReference ILocalizedNameOwner.NameReference => new LocalizedStringReference()
//             {
//                 defaultValue = id.ToPascalCase(" ")
//             };
//
//             #endregion
//         }
//     }
// }