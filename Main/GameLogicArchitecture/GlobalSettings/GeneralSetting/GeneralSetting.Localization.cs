#if UNITY_EDITOR
using VMFramework.Localization;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GeneralSetting : ILocalizedStringOwnerConfig
    {
        public virtual bool LocalizationEnabled => false;

        public virtual void RemoveInvalidLocalizedStrings()
        {
            
        }

        public virtual void SetDefaultKeyValue(LocalizedStringAutoConfigSettings setting)
        {
            
        }
    }
}
#endif
