#if UNITY_EDITOR
using VMFramework.Core;
using VMFramework.Localization;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GameTagInfo : ILocalizedStringOwnerConfig
    {
        public virtual void RemoveInvalidLocalizedStrings()
        {
            if (hasName && name != null && name.IsValid() == false)
            {
                name = new();
            }

            if (hasDescription && description != null && description.IsValid() == false)
            {
                description = new();
            }
        }

        public virtual void SetDefaultKeyValue(LocalizedStringAutoConfigSettings setting)
        {
            var defaultTableName = setting.defaultTableName;
            if (defaultTableName.IsNullOrEmpty())
            {
                defaultTableName = CoreSetting.GameTagGeneralSetting.defaultLocalizationTableName;
            }

            if (hasName)
            {
                var nameKey = id.ToPascalCase() + "TagName";
                var nameContent = id.ToPascalCase(" ");

                LocalizedStringEditorUtility.SetDefaultKey(ref name, defaultTableName, nameKey, nameContent,
                    replace: false);
            }

            if (hasDescription)
            {
                var descriptionKey = id.ToPascalCase() + "TagDescription";

                LocalizedStringEditorUtility.SetDefaultKey(ref description, defaultTableName, descriptionKey,
                    string.Empty, replace: false);
            }
        }
    }
}
#endif