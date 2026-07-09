using Sirenix.OdinInspector;
using VMFramework.Localization;
using VMFramework.OdinExtensions;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GeneralSetting : ILocalizationTableNameOwner
    {
#if UNITY_EDITOR
        [LabelText("Default Localization Table"), TabGroup(TAB_GROUP_NAME, LOCALIZABLE_SETTING_CATEGORY)]
        [InfoBox("Localization Settings is disabled", VisibleIf = "@!" + nameof(LocalizationEnabled))]
        [TableName]
        [EnableIf(nameof(LocalizationEnabled))]
#endif
        public string defaultLocalizationTableName;

        string ILocalizationTableNameOwner.LocalizationTableName => defaultLocalizationTableName;
    }
}
