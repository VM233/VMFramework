// using Sirenix.OdinInspector;
// using VMFramework.Localization;
//
// namespace VMFramework.GameLogicArchitecture
// {
//     public partial class GameTypeGeneralSetting
//     {
//         private partial class LocalizedGameTypeInfo : GameTypeInfo, INameOwner, ILocalizedNameOwner
//         {
//             [PropertyOrder(-5000)]
//             public LocalizedStringReference name = new();
//
//             #region Interface Implementation
//
//             string INameOwner.Name => name;
//
//             IReadOnlyLocalizedStringReference ILocalizedNameOwner.NameReference => name;
//
//             #endregion
//         }
//     }
// }