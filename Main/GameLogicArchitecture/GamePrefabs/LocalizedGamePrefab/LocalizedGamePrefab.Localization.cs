#if UNITY_EDITOR
using VMFramework.Core;
using VMFramework.Localization;

namespace VMFramework.GameLogicArchitecture
{
    public partial class LocalizedGamePrefab : ILocalizedStringOwnerConfig
    {
        public virtual void RemoveInvalidLocalizedStrings()
        {
            if (name != null && name.IsValid() == false)
            {
                name = null;
            }

            if (description != null && description.IsValid() == false)
            {
                description = null;
            }
        }

        protected virtual void SetDefaultKeyValue(LocalizedStringAutoConfigSettings setting)
        {
            var key = id.ToPascalCase() + "Name";
            var content = id.RemoveWordsSuffix(IDSuffix.GetWords()).ToPascalCase(" ");

            LocalizedStringEditorUtility.SetDefaultKey(ref name, setting.defaultTableName, key, content,
                replace: false);

            if (hasDescription)
            {
                key = id.ToPascalCase() + "Description";

                LocalizedStringEditorUtility.SetDefaultKey(ref description, setting.defaultTableName, key, content: "",
                    replace: false);
            }
        }

        void ILocalizedStringOwnerConfig.SetDefaultKeyValue(LocalizedStringAutoConfigSettings setting)
        {
            if (setting.defaultTableName.IsNullOrEmpty())
            {
                if (this.TryGetGamePrefabGeneralSetting(out var generalSetting))
                {
                    setting.defaultTableName = generalSetting.defaultLocalizationTableName;
                }
            }

            if (setting.defaultTableName.IsNullOrEmpty())
            {
                return;
            }

            SetDefaultKeyValue(setting);
        }
    }
}
#endif