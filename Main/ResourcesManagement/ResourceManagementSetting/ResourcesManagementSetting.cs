using VMFramework.Effects;
using VMFramework.Procedure;
using VMFramework.ResourcesManagement;

namespace VMFramework.GameLogicArchitecture
{
    [ManagerCreationProvider(ManagerType.SettingCore)]
    public sealed class ResourcesManagementSetting
        : GlobalSetting<ResourcesManagementSetting, ResourcesManagementSettingFile>
    {
        public static SpriteGeneralSetting SpriteGeneralSetting =>
            GlobalSettingFile == null ? null : GlobalSettingFile.spriteGeneralSetting;
        
        public static EffectGeneralSetting EffectGeneralSetting =>
            GlobalSettingFile == null ? null : GlobalSettingFile.effectGeneralSetting;
    }
}