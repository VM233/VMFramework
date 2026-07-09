#if UNITY_EDITOR
using VMFramework.Core;
using VMFramework.Localization;

namespace VMFramework.GameEvents
{
    public partial class KeyCodeTranslation : ILocalizedStringOwnerConfig
    {
        public virtual void RemoveInvalidLocalizedStrings()
        {
            if (translation != null && translation.IsValid() == false)
            {
                translation = null;
            }
        }

        public virtual void SetDefaultKeyValue(LocalizedStringAutoConfigSettings setting)
        {
            var key = keyCode.ToString().ToPascalCase() + "Name";
            var content = keyCode.ToString();
            LocalizedStringEditorUtility.SetDefaultKey(ref translation, setting.defaultTableName, key, content,
                replace: false);
        }
    }
}
#endif