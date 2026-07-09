using VMFramework.Configuration;
using VMFramework.GameEvents;
using VMFramework.Procedure;

namespace VMFramework.GameLogicArchitecture
{
    [ManagerCreationProvider(ManagerType.SettingCore)]
    public sealed partial class CoreSetting : GlobalSetting<CoreSetting, CoreSettingFile>
    {
        public static GameTagGeneralSetting GameTagGeneralSetting =>
            GlobalSettingFile == null ? null : GlobalSettingFile.gameTagGeneralSetting;
        
        public static GameEventGeneralSetting GameEventGeneralSetting =>
            GlobalSettingFile == null ? null : GlobalSettingFile.gameEventGeneralSetting;

        public static ColliderMouseEventGeneralSetting ColliderMouseEventGeneralSetting =>
            GlobalSettingFile == null ? null : GlobalSettingFile.colliderMouseEventGeneralSetting;
        
        public static GeneralConfigGeneralSetting GeneralConfigGeneralSetting =>
            GlobalSettingFile == null ? null : GlobalSettingFile.generalConfigGeneralSetting;
        
        public static CommonPresetGeneralSetting CommonPresetGeneralSetting =>
            GlobalSettingFile == null ? null : GlobalSettingFile.commonPresetGeneralSetting;
    }
}