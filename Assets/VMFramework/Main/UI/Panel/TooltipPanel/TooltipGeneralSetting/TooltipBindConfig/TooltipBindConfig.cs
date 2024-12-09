using VMFramework.Configuration;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public sealed class TooltipBindConfig : GameTagBasedConfigBase
    {
        [UIPresetID(typeof(ITooltipConfig))]
        [IsNotNullOrEmpty]
        public string tooltipID;
    }
}