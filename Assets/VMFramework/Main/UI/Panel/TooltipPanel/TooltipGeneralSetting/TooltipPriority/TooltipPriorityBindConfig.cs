using Sirenix.OdinInspector;
using VMFramework.Configuration;

namespace VMFramework.UI
{
    public sealed partial class TooltipPriorityBindConfig : GameTagBasedConfigBase
    {
        public TooltipPriorityConfig priority = new();
    }
}