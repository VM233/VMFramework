﻿// using Newtonsoft.Json;
// using Sirenix.OdinInspector;
// using VMFramework.Localization;
//
// namespace VMFramework.GameLogicArchitecture
// {
//     public abstract partial class LocalizedGameTypedGamePrefab : GameTypedGamePrefab, 
//         ILocalizedGameTypedGamePrefab
//     {
//         #region Configs
//
//         [LabelText(SdfIconType.FileEarmarkPersonFill),
//          TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
//         [JsonProperty(Order = -5000), PropertyOrder(-5000)]
//         public LocalizedStringReference name = new();
//
//         #endregion
//
//         #region Interface Implementations
//
//         string INameOwner.Name => name;
//
//         public IReadOnlyLocalizedStringReference NameReference => name;
//
//         #endregion
//     }
// }