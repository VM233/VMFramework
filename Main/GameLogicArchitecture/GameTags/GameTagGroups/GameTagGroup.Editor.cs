using VMFramework.Localization;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GameTagGroup : ILocalizedStringOwnerConfig
    {
        public virtual void RemoveInvalidLocalizedStrings()
        {
            foreach (var gameTagInfo in gameTagInfos)
            {
                if (gameTagInfo is ILocalizedStringOwnerConfig ownerConfig)
                {
                    ownerConfig.RemoveInvalidLocalizedStrings();
                }
            }
        }

        public virtual void SetDefaultKeyValue(LocalizedStringAutoConfigSettings setting)
        {
            foreach (var gameTagInfo in gameTagInfos)
            {
                if (gameTagInfo is ILocalizedStringOwnerConfig ownerConfig)
                {
                    ownerConfig.SetDefaultKeyValue(setting);
                }
            }
        }
    }
}