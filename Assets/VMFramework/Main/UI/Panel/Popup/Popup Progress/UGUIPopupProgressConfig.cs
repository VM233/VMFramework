using System;
using Sirenix.OdinInspector;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class UGUIPopupProgressConfig : UGUIPopupConfig
    {
        public override Type GameItemType => typeof(UGUIPopupProgress);

        [TabGroup(TAB_GROUP_NAME, POPUP_SETTING_CATEGORY)]
        [UGUIName]
        [IsNotNullOrEmpty]
        public string progressName;
    }
}
