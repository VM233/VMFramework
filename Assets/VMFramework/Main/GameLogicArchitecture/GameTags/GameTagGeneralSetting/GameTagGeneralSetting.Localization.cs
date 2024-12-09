#if UNITY_EDITOR
using VMFramework.Core.Editor;
using VMFramework.Localization;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GameTagGeneralSetting
    {
        public override bool LocalizationEnabled => true;

        public override void AutoConfigureLocalizedString(LocalizedStringAutoConfigSettings settings)
        {
            base.AutoConfigureLocalizedString(settings);

            if (gameTagInfos is null)
            {
                return;
            }

            foreach (var gameTagInfo in gameTagInfos)
            {
                if (gameTagInfo is ILocalizedStringOwnerConfig ownerConfig)
                {
                    ownerConfig.AutoConfigureLocalizedString(settings);
                }
            }
            
            if (settings.save)
            {
                this.EnforceSave();
            }
            else
            {
                this.SetEditorDirty();
            }
        }

        public override void SetKeyValueByDefault()
        {
            base.SetKeyValueByDefault();

            if (gameTagInfos is null)
            {
                return;
            }

            foreach (var gameTagInfo in gameTagInfos)
            {
                if (gameTagInfo is ILocalizedStringOwnerConfig ownerConfig)
                {
                    ownerConfig.SetKeyValueByDefault();
                }
            }
        }
    }
}
#endif