using Sirenix.OdinInspector;
using UnityEngine.Localization;
using VMFramework.Localization;

namespace VMFramework.GameLogicArchitecture
{
    public abstract partial class LocalizedGamePrefab : GamePrefab, ILocalizedGamePrefab
    {
        #region Configs

        [LabelText(SdfIconType.FileEarmarkPersonFill),
         TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY, SdfIconType.Info, TextColor = "blue")]
        [PropertyOrder(-5000)]
        public LocalizedString name;

        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        public bool hasDescription = false;

        [LabelText(SdfIconType.BlockquoteLeft), TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [ShowIf(nameof(hasDescription))]
        public LocalizedString description;

        #endregion

        #region Interface Implementations

        public string Name => name.GetGeneralString();

        LocalizedString ILocalizedNameOwner.NameReference => name;

        string IDescriptionOwner.Description => hasDescription ? description.GetGeneralString() : null;

        LocalizedString ILocalizedDescriptionOwner.DescriptionReference => description;

        #endregion
    }
}