using VMFramework.Configuration;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public sealed class ContextMenuBindConfig : GameTagBasedConfigBase
    {
        [UIPresetID(typeof(IContextMenuConfig))]
        [IsNotNullOrEmpty]
        public string contextMenuID;
    }
}