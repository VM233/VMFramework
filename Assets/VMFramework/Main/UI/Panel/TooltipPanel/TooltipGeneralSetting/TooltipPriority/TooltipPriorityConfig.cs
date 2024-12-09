using VMFramework.Configuration;

namespace VMFramework.UI
{
    public sealed class TooltipPriorityConfig : DictionaryConfigPriorityConfig
    {
        public TooltipPriorityConfig() : base()
        {
        }
        
        public TooltipPriorityConfig(int priority) : base(priority)
        {
            
        }

        public TooltipPriorityConfig(string presetID) : base(presetID)
        {
            
        }

        public override IDictionaryConfigs<string, PriorityPreset> GetDictionaryConfigs()
        {
            return UISetting.TooltipGeneralSetting.tooltipPriorityPresets;
        }
    }
}