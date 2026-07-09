using Sirenix.OdinInspector;
using VMFramework.Configuration;
using VMFramework.GameEvents;

namespace VMFramework.GameLogicArchitecture
{
    [GlobalSettingFileConfig(FileName = nameof(CoreSettingFile))]
    public sealed partial class CoreSettingFile : GlobalSettingFile
    {
        public const string CORE_CATEGORY = "Core";
        
        [TabGroup(TAB_GROUP_NAME, CORE_CATEGORY)]
        [Required]
        public GameTagGeneralSetting gameTagGeneralSetting;
        
        [TabGroup(TAB_GROUP_NAME, CORE_CATEGORY)]
        [Required]
        public GameEventGeneralSetting gameEventGeneralSetting;

        [TabGroup(TAB_GROUP_NAME, CORE_CATEGORY)]
        [Required]
        public ColliderMouseEventGeneralSetting colliderMouseEventGeneralSetting;
        
        [TabGroup(TAB_GROUP_NAME, CORE_CATEGORY)]
        [Required]
        public GeneralConfigGeneralSetting generalConfigGeneralSetting;
        
        [TabGroup(TAB_GROUP_NAME, CORE_CATEGORY)]
        [Required]
        public CommonPresetGeneralSetting commonPresetGeneralSetting;
    }
}