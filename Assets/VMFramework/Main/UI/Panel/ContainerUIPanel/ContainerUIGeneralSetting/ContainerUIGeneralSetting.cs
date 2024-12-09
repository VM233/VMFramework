using VMFramework.Configuration;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI 
{
    public sealed partial class ContainerUIGeneralSetting : GeneralSetting
    {
        public DictionaryConfigs<string, ContainerUIBindConfig> containerUIBindConfigs = new();

        public override void CheckSettings()
        {
            base.CheckSettings();
            
            containerUIBindConfigs.CheckSettings();
        }

        protected override void OnInit()
        {
            base.OnInit();
            
            containerUIBindConfigs.Init();
        }
    }
}