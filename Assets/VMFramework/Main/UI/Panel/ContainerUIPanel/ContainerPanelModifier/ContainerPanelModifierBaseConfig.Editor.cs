#if UNITY_EDITOR
using System;

namespace VMFramework.UI
{
    public partial class ContainerPanelModifierBaseConfig
    {
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();

            slotDistributorConfigs ??= new();
        }
    }
}
#endif