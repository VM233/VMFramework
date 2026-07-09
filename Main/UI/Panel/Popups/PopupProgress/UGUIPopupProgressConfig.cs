using System;
using Sirenix.OdinInspector;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class UGUIPopupProgressConfig : UGUIPanelConfig
    {
        public override Type GameItemType => typeof(UGUIPopupProgress);

        [TabGroup(TAB_GROUP_NAME, UGUI_PANEL_CATEGORY)]
        [UGUIName]
        [IsNotNullOrEmpty]
        public string progressName;
    }
}
