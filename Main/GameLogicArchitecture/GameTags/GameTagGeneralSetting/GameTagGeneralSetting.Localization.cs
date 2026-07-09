#if UNITY_EDITOR
using VMFramework.Localization;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GameTagGeneralSetting
    {
        public override bool LocalizationEnabled => true;

        public override void RemoveInvalidLocalizedStrings()
        {
            base.RemoveInvalidLocalizedStrings();
            
            foreach (var gameTagInfo in EnumerateGameTagInfos())
            {
                if (gameTagInfo is ILocalizedStringOwnerConfig ownerConfig)
                {
                    ownerConfig.RemoveInvalidLocalizedStrings();
                }
            }
        }

        public override void SetDefaultKeyValue(LocalizedStringAutoConfigSettings setting)
        {
            base.SetDefaultKeyValue(setting);

            foreach (var gameTagInfo in EnumerateGameTagInfos())
            {
                if (gameTagInfo is ILocalizedStringOwnerConfig ownerConfig)
                {
                    ownerConfig.SetDefaultKeyValue(setting);
                }
            }
        }
    }
}
#endif