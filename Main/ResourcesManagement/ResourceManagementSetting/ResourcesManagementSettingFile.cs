using Sirenix.OdinInspector;
using VMFramework.Effects;
using VMFramework.ResourcesManagement;

namespace VMFramework.GameLogicArchitecture
{
    [GlobalSettingFileConfig(FileName = nameof(ResourcesManagementSettingFile))]
    public sealed partial class ResourcesManagementSettingFile : GlobalSettingFile
    {
        public const string RESOURCES_MANAGEMENT_CATEGORY = "Resources Management";

        [TabGroup(TAB_GROUP_NAME, RESOURCES_MANAGEMENT_CATEGORY)]
        [Required]
        public SpriteGeneralSetting spriteGeneralSetting;

        [TabGroup(TAB_GROUP_NAME, RESOURCES_MANAGEMENT_CATEGORY)]
        [Required]
        public EffectGeneralSetting effectGeneralSetting;
    }
}