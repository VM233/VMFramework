using VMFramework.Configuration;

namespace VMFramework.UI
{
    public sealed class SortingOrderConfig : DictionaryConfigPriorityConfig
    {
        public SortingOrderConfig() : base()
        {
            
        }
        
        public SortingOrderConfig(int priority) : base(priority)
        {
            
        }

        public SortingOrderConfig(string presetID) : base(presetID)
        {
        }

        public override IDictionaryConfigs<string, PriorityPreset> GetDictionaryConfigs()
        {
            return UISetting.UIPanelGeneralSetting.sortingOrderPresets;
        }
    }
}