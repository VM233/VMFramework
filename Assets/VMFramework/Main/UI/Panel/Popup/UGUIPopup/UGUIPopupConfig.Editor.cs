#if UNITY_EDITOR
using System;

namespace VMFramework.UI
{
    public partial class UGUIPopupConfig
    {
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();

            containerAnimation ??= new();
            startContainerAnimation ??= new();
            endContainerAnimation ??= new();
        }
    }
}
#endif