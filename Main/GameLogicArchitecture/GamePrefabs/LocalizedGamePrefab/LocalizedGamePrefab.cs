using Sirenix.OdinInspector;
using System;
using UnityEngine.Localization;
using VMFramework.Localization;
#if UNITY_EDITOR
using VMFramework.Core;
#endif

namespace VMFramework.GameLogicArchitecture
{
    [Serializable]
    public abstract class LocalizedGamePrefab : GamePrefab, ILocalizedGamePrefab
#if UNITY_EDITOR
        , ILocalizedStringOwnerConfig
#endif
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

        LocalizedString ILocalizedDescriptionOwner.DescriptionReference => hasDescription ? description : null;

        #endregion

#if UNITY_EDITOR
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
#endif
    }
}
