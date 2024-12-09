using Sirenix.OdinInspector;
using UnityEngine.Serialization;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI
{
    [GlobalSettingFileConfig(FileName = nameof(UISettingFile))]
    public sealed partial class UISettingFile : GlobalSettingFile
    {
        public const string UI_CATEGORY = "UI";
        
        [TabGroup(TAB_GROUP_NAME, UI_CATEGORY)]
        [Required]
        public UIPanelGeneralSetting uiPanelGeneralSetting;
        
        [TabGroup(TAB_GROUP_NAME, UI_CATEGORY)]
        [Required]
        public UIPanelProcedureGeneralSetting uiPanelProcedureGeneralSetting;

        [FormerlySerializedAs("debugUIPanelGeneralSetting")]
        [TabGroup(TAB_GROUP_NAME, UI_CATEGORY)]
        [Required]
        public DebugPanelGeneralSetting debugPanelGeneralSetting;

        [TabGroup(TAB_GROUP_NAME, UI_CATEGORY)]
        [Required]
        public TooltipGeneralSetting tooltipGeneralSetting;

        [TabGroup(TAB_GROUP_NAME, UI_CATEGORY)]
        [Required]
        public ContextMenuGeneralSetting contextMenuGeneralSetting;
        
        [TabGroup(TAB_GROUP_NAME, UI_CATEGORY)]
        [Required]
        public ContainerUIGeneralSetting containerUIGeneralSetting;
    }
}