#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Core.Editor;

namespace VMFramework.UI
{
    public partial class UIPanelConfig
    {
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();

            sortingOrderConfig ??= new(UIPanelGeneralSetting.DEFAULT_SORTING_ORDER_ID);
            modifiersConfigs ??= new();
        }
    }
}
#endif