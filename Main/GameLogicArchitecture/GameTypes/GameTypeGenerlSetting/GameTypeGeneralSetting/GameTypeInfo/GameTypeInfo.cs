// using System.Collections.Generic;
// using Sirenix.OdinInspector;
// using VMFramework.Core;
// using VMFramework.Localization;
// using VMFramework.OdinExtensions;
//
// namespace VMFramework.GameLogicArchitecture
// {
//     public partial class GameTypeGeneralSetting
//     {
//         private class GameTypeInfo : IChildrenProvider<GameTypeInfo>, IIDOwner<string>, INameOwner,
//             ILocalizedNameOwner
//         {
//             [LabelText("ID")]
//             [IsNotNullOrEmpty, IsGameTypeID]
//             public string id;
//
//             [LabelText("Sub Game Types")]
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
