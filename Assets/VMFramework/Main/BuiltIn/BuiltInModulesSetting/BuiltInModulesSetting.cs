using VMFramework.Cameras;
using VMFramework.Containers;
using VMFramework.Procedure;
using VMFramework.Properties;

namespace VMFramework.GameLogicArchitecture
{
    [ManagerCreationProvider(ManagerType.SettingCore)]
    public sealed partial class BuiltInModulesSetting
        : GlobalSetting<BuiltInModulesSetting, BuiltInModulesSettingFile>
    {
        public static GamePropertyGeneralSetting GamePropertyGeneralSetting =>
            GlobalSettingFile == null ? null : GlobalSettingFile.gamePropertyGeneralSetting;

        public static TooltipPropertyGeneralSetting TooltipPropertyGeneralSetting =>
            GlobalSettingFile == null ? null : GlobalSettingFile.tooltipPropertyGeneralSetting;

        public static CameraGeneralSetting CameraGeneralSetting =>
            GlobalSettingFile == null ? null : GlobalSettingFile.cameraGeneralSetting;

        public static ContainerGeneralSetting ContainerGeneralSetting =>
            GlobalSettingFile == null ? null : GlobalSettingFile.containerGeneralSetting;
    }
}