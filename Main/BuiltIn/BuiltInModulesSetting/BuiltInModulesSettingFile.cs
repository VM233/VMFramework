using Sirenix.OdinInspector;
using VMFramework.Containers;

namespace VMFramework.GameLogicArchitecture
{
    [GlobalSettingFileConfig(FileName = nameof(BuiltInModulesSettingFile))]
    public sealed partial class BuiltInModulesSettingFile : GlobalSettingFile
    {
        public const string BUILTIN_MODULE_CATEGORY = "Builtin Modules";

        [TabGroup(TAB_GROUP_NAME, BUILTIN_MODULE_CATEGORY)]
        [Required]
        public ContainerGeneralSetting containerGeneralSetting;
    }
}