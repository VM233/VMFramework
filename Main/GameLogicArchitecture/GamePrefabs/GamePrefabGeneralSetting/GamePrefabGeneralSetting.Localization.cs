#if UNITY_EDITOR
#if PARREL_SYNC
using ParrelSync;
#endif
using VMFramework.Core;
using VMFramework.Localization;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GamePrefabGeneralSetting
    {
        public override bool LocalizationEnabled =>
            BaseGamePrefabType.IsDerivedFrom<ILocalizedGamePrefab>(false);

        public override void RemoveInvalidLocalizedStrings()
        {
            base.RemoveInvalidLocalizedStrings();
            
            foreach (var gamePrefabWrapper in initialGamePrefabProviders)
            {
                if (gamePrefabWrapper is not ILocalizedStringOwnerConfig ownerConfig)
                {
                    continue;
                }
                
                ownerConfig.RemoveInvalidLocalizedStrings();
            }
        }

        public override void SetDefaultKeyValue(LocalizedStringAutoConfigSettings setting)
        {
            base.SetDefaultKeyValue(setting);
            
            foreach (var gamePrefabWrapper in initialGamePrefabProviders)
            {
                if (gamePrefabWrapper is not ILocalizedStringOwnerConfig ownerConfig)
                {
                    continue;
                }
                
                ownerConfig.SetDefaultKeyValue(setting);
            }
        }
    }
}
#endif