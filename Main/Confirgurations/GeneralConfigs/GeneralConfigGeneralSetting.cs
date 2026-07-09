using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Configuration 
{
    public sealed partial class GeneralConfigGeneralSetting : GeneralSetting
    {
        public const string GENERAL_CONFIGS_CATEGORY = "General Configs";
        
        [TabGroup(TAB_GROUP_NAME, GENERAL_CONFIGS_CATEGORY)]
        public List<GeneralConfig> generalConfigs = new();

        public override void CheckSettings()
        {
            base.CheckSettings();
            
            generalConfigs.CheckSettingsAndSkipNulls();
        }

        protected override void OnInit()
        {
            base.OnInit();
            
            generalConfigs?.InitAndSkipNulls();
        }
    }
}