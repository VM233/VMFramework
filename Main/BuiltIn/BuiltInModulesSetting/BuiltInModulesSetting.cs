using VMFramework.Containers;
using VMFramework.Procedure;

namespace VMFramework.GameLogicArchitecture
{
    [ManagerCreationProvider(ManagerType.SettingCore)]
    public sealed partial class BuiltInModulesSetting
        : GlobalSetting<BuiltInModulesSetting, BuiltInModulesSettingFile>
    {
        public static ContainerGeneralSetting ContainerGeneralSetting =>
            GlobalSettingFile == null ? null : GlobalSettingFile.containerGeneralSetting;
    }
}